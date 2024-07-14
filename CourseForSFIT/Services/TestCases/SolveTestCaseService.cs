using AutoMapper;
using Data.Entities;
using Dtos.Models.ExerciseModels;
using Dtos.Models.TestCaseModels;
using Dtos.Results.TestCaseResults;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Repositories.Repositories.IRepo;
using Repositories.unitOfWork;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.TestCases
{
    public interface ISolveTestCaseService
    {
        Task<object> SolveExerciseProblem(TestCaseSolve testCaseSolve);
    }
    public class SolveTestCaseService : ISolveTestCaseService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SolveTestCaseService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<object> SolveExerciseProblem(TestCaseSolve testCaseSolve)
        {
            try
            {
                
                int currentUserId = _httpContextAccessor.HttpContext.Items["UserId"] == null ? 0 : int.Parse(_httpContextAccessor.HttpContext.Items["UserId"] as string);
                UserExerciseAddDto userExercises = new UserExerciseAddDto() { ContentCode = testCaseSolve.ContentCode, UserId = currentUserId, ExerciseId = testCaseSolve.ExerciseId };             
                List<Task<string>> tasks = new List<Task<string>>();
                var testCases =  _unitOfWork.TestCaseRepository.GetAllTestCaseByExerciseId(testCaseSolve.ExerciseId).ToList();
                int numberOfRequests = testCases.Count;
                for (int i = 0; i < numberOfRequests; i++)
                {
                    CodeConfiguration requestData = new CodeConfiguration()
                    {
                        Language = testCaseSolve.Language,
                        Version = testCaseSolve.Version,
                        Files = new List<FileContent>() { new FileContent() { Content = testCaseSolve.ContentCode } },   
                        Stdin = testCases[i].InputData == null ? "" : await HandleFile.ReadFile("Inputs",testCases[i].InputData)
                    };
                    tasks.Add(PostRequestAsync(_httpClient, requestData));
                }
                string[] results = await Task.WhenAll(tasks);
                int index = 0;
                int scores = 0;
                foreach(string result in results)
                {
                    var dataConfigCodeReturn = JsonConvert.DeserializeObject<CodeConfigDataReturn>(result);
                    if(dataConfigCodeReturn?.Run?.Stdout.TrimEnd('\n') == await HandleFile.ReadFile("Outputs", testCases[index].ExpectedOutput))
                    {
                        scores++;
                    }
                    index++;
                }
                userExercises.SuccessRate = scores+"/"+numberOfRequests;
                UserExercise userExerciseAdd = _mapper.Map<UserExercise>(userExercises);
                await _unitOfWork.UserExerciseRepository.AddAsync(userExerciseAdd);
                await _unitOfWork.SaveAsync();
                return scores;
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"An error occurred while saving the entity changes: {innerException}");
            }
        }

        private static async Task<string> PostRequestAsync(HttpClient client, CodeConfiguration requestData)
        {
            string jsonRequestData = JsonConvert.SerializeObject(requestData);
            HttpContent content = new StringContent(jsonRequestData);
            HttpResponseMessage response = await client.PostAsync("https://emkc.org/api/v2/piston/execute", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}

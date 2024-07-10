using AutoMapper;
using Data.Entities;
using Dtos.Models;
using Dtos.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Repositories.Repositories.IRepo;
using Repositories.unitOfWork;
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
            /*
            try
            {
                
                int currentUserId = _httpContextAccessor.HttpContext.Items["UserId"] == null ? 0 : int.Parse(_httpContextAccessor.HttpContext.Items["UserId"] as string);
                UserExerciseAddDto userExercises = new UserExerciseAddDto() { ContentCode = testCaseSolve.ContentCode, UserId = currentUserId, ExerciseId = testCaseSolve.ExerciseId };
                UserExercise userExerciseAdd = _mapper.Map<UserExercise>(userExercises);
                await _unitOfWork.UserExerciseRepository.AddAsync(userExerciseAdd);
                await _unitOfWork.SaveAsync();
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
                        Stdin = testCases[i].InputData
                    };
                    tasks.Add(PostRequestAsync(_httpClient, requestData));
                }
                string[] results = await Task.WhenAll(tasks);
                int numberOfDataReturn = results.Length;
                List<UserResult> userResults = new List<UserResult>();
                List<string> dataReturn = new List<string>();
                for(int i = 0;i < numberOfDataReturn; i++)
                {
                    var dataConfigCodeReturn = JsonConvert.DeserializeObject<CodeConfigDataReturn>(results[i]);
                    var userResultAdd = new UserResultAdd()
                    {
                        TestCaseId = testCases[i].Id,
                        UserExerciseId = userExerciseAdd.Id,
                    };
                    if (dataConfigCodeReturn?.Run?.Stdout == testCases[i].ExpectedOutput)
                    {
                        userResultAdd.IsPass = true;
                    }
                    else
                    {
                        userResultAdd.IsPass = false;
                    }
                    dataReturn.Add(dataConfigCodeReturn.Run.Output);
                }
                await _unitOfWork.UserResultRepository.AddMany(userResults);
                await _unitOfWork.SaveAsync();
                return new ApiResponse<List<string>> { IsSuccess = true , Metadata = dataReturn };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
                */
            CodeConfiguration requestData = new CodeConfiguration()
            {
                Language = "c++",
                Version = "10.2.0",
                Files = new List<FileContent>() { new FileContent() { Content = "#include <iostream>\r\n\r\nint main() {\r\n    std::cout << \"Hello, World!\" << std::endl;\r\n    return 0;\r\n}" } },
            };
            
            string jsonRequestData = JsonConvert.SerializeObject(requestData);  
            HttpContent content = new StringContent(jsonRequestData);
            HttpResponseMessage response = await _httpClient.PostAsync("https://emkc.org/api/v2/piston/execute", content);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            var dataConfigCodeReturn = JsonConvert.DeserializeObject<CodeConfigDataReturn>(result);
            return dataConfigCodeReturn;
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

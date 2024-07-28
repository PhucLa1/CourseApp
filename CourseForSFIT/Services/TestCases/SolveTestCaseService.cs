using AutoMapper;
using Data.Entities;
using Dtos.Models.ExerciseModels;
using Dtos.Models.TestCaseModels;
using Dtos.Results;
using Dtos.Results.TestCaseResults;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Repositories.Repositories.Base;
using Shared;

namespace Services.TestCases
{
    public interface ISolveTestCaseService
    {
        Task<ApiResponse<List<bool>>> SolveExerciseProblem(TestCaseSolve testCaseSolve);        
    }
    public class SolveTestCaseService : ISolveTestCaseService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBaseRepository<TestCase> _testCaseRepository;
        private readonly IBaseRepository<UserExercise> _userExerciseRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<TestCaseSolve> _validatorTestCaseSolve;

        public SolveTestCaseService(HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor,
            IBaseRepository<TestCase> testCaseRepository,
            IBaseRepository<UserExercise> userExerciseRepository,
            IMapper mapper,
            IValidator<TestCaseSolve> validatorTestCaseSolve
            )
        {
            _testCaseRepository = testCaseRepository;
            _userExerciseRepository = userExerciseRepository;
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _validatorTestCaseSolve = validatorTestCaseSolve;
        }
        public async Task<ApiResponse<List<bool>>> SolveExerciseProblem(TestCaseSolve testCaseSolve)
        {
            try
            {
                var resultValidation = _validatorTestCaseSolve.Validate(testCaseSolve);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<List<bool>>.FailtureValidation(resultValidation.Errors);
                }
                int currentUserId = _httpContextAccessor.HttpContext.Items["UserId"] == null ? 0 : int.Parse(_httpContextAccessor.HttpContext.Items["UserId"] as string);
                UserExerciseAddDto userExercises = new UserExerciseAddDto() { ContentCode = testCaseSolve.ContentCode, UserId = currentUserId, ExerciseId = testCaseSolve.ExerciseId, Avatar = testCaseSolve.Avatar, Language = testCaseSolve.Language,Version = testCaseSolve.Version  };
                List<string> tasks = new List<string>();
                var testCases = await _testCaseRepository.GetAllQueryAble().Where(e => e.ExerciseId == testCaseSolve.ExerciseId).ToListAsync();
                int numberOfRequests = testCases.Count;
                for (int i = 0; i < numberOfRequests; i++)
                {
                    CodeConfiguration requestData = new CodeConfiguration()
                    {
                        Language = testCaseSolve.Language,
                        Version = testCaseSolve.Version,
                        Files = new List<FileContent>() { new FileContent() { Content = testCaseSolve.ContentCode } },
                        Stdin = testCases[i].InputData == null ? "" : await HandleFile.ReadFile("Inputs", testCases[i].InputData)
                    };
                    string request = await PostRequestAsync(_httpClient, requestData);
                    tasks.Add(request);
                }
                int index = 0;
                int scores = 0;
                List<bool> results = new List<bool>();
                foreach (string result in tasks)
                {
                    var dataConfigCodeReturn = JsonConvert.DeserializeObject<CodeConfigDataReturn>(result);
                    if (dataConfigCodeReturn?.Run?.Stdout?.TrimEnd('\n') == await HandleFile.ReadFile("Outputs", testCases[index].ExpectedOutput))
                    {
                        scores++;
                        results.Add(true);
                    }
                    else
                    {
                        results.Add(false);
                    }
                    index++;
                }          
                bool isSuccess = scores == numberOfRequests ? true :false;
                userExercises.SuccessRate = scores + "/" + numberOfRequests;
                userExercises.IsSuccess = isSuccess;
                UserExercise userExerciseAdd = _mapper.Map<UserExercise>(userExercises);
                await _userExerciseRepository.AddAsync(userExerciseAdd);
                await _userExerciseRepository.SaveChangeAsync();
                return new ApiResponse<List<bool>> { IsSuccess = isSuccess, Metadata = results };
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"An error occurred while saving the entity changes: {innerException}");
            }
        }



        private async Task<string> PostRequestAsync(HttpClient client, CodeConfiguration requestData)
        {
            string jsonRequestData = JsonConvert.SerializeObject(requestData);
            HttpContent content = new StringContent(jsonRequestData);
            HttpResponseMessage response = await client.PostAsync("https://emkc.org/api/v2/piston/execute", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}

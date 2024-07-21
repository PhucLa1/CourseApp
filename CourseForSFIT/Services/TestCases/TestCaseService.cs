using AutoMapper;
using Data.Entities;
using Dtos.Models.ExerciseModels;
using Dtos.Models.TestCaseModels;
using Dtos.Results;
using Dtos.Results.TestCaseResults;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.Base;
using Shared;

namespace Services.TestCases
{
    public interface ITestCaseService
    {
        Task<ApiResponse<TestCaseReturnDto>> GetAllTestCaseNotLockByExerciseId(int exerciseId);
        Task<ApiResponse<List<TestCaseDto>>> GetAllTestCaseByExerciseId(int exerciseId);
        Task<ApiResponse<bool>> DeleteTestCase(int id);
        Task<ApiResponse<int>> AddTestCase(int exerciseId, TestCaseExerciseAddDto testCaseExerciseAddDto);
        Task<ApiResponse<bool>> UpdateTestCase(int id, TestCaseExerciseUpdateDto testCaseExerciseUpdateDto);
    }
    public class TestCaseService : ITestCaseService
    {
        private readonly IBaseRepository<TestCase> _testCaseRepository;
        private readonly IMapper _mapper;
        public TestCaseService(IBaseRepository<TestCase> testCaseRepository, IMapper mapper)
        {
            _testCaseRepository = testCaseRepository;
            _mapper = mapper;
        }
        public async Task<ApiResponse<int>> AddTestCase(int exerciseId, TestCaseExerciseAddDto testCaseExerciseAddDto)
        {
            try
            {
                TestCase testCase = _mapper.Map<TestCase>(new TestCaseAddDto()
                {
                    ExerciseId = exerciseId,
                    ExpectedOutput = await HandleFile.Upload("Outputs", testCaseExerciseAddDto.ExpectedOutput),
                    InputData = await HandleFile.Upload("Inputs", testCaseExerciseAddDto.InputData),
                    IsLock = testCaseExerciseAddDto.IsLock
                });
                await _testCaseRepository.AddAsync(testCase);
                await _testCaseRepository.SaveChangeAsync();
                return new ApiResponse<int> { IsSuccess = true, Metadata = testCase.Id };
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException?.Message ?? ex.Message;
                // Log the exception details here
                Console.WriteLine($"Error: {innerException}");
                throw new Exception($"An error occurred while saving the entity changes: {innerException}");
            }
        }
        public async Task<ApiResponse<TestCaseReturnDto>> GetAllTestCaseNotLockByExerciseId(int exerciseId)
        {
            try
            {
                int testCaseLocksNumber = await _testCaseRepository.GetAllQueryAble().Where(e => e.ExerciseId == exerciseId && e.IsLock == true).CountAsync();

                string[] testCaseLocks = new string[testCaseLocksNumber];
                return new ApiResponse<TestCaseReturnDto>
                {
                    IsSuccess = true,
                    Metadata = new TestCaseReturnDto
                    {
                        testCaseDtos = _mapper.Map<List<TestCaseDto>>(await _testCaseRepository.GetAllQueryAble().Where(e => e.ExerciseId == exerciseId && e.IsLock == false).ToListAsync()),
                        totalTestCaseLockCounts = testCaseLocks
                    }
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<List<TestCaseDto>>> GetAllTestCaseByExerciseId(int exerciseId)
        {
            try
            {
                return new ApiResponse<List<TestCaseDto>> { IsSuccess = true, Metadata = _mapper.Map<List<TestCaseDto>>(await _testCaseRepository.GetAllQueryAble().Where(e => e.ExerciseId == exerciseId).ToListAsync()) };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> DeleteTestCase(int id)
        {
            try
            {
                var testCase = await _testCaseRepository.GetAllQueryAble().Where(e => e.Id == id).FirstAsync();
                await HandleFile.DeleteFile("Inputs", testCase.InputData);
                await HandleFile.DeleteFile("Outputs", testCase.ExpectedOutput);
                await _testCaseRepository.RemoveAsync(id);
                await _testCaseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateTestCase(int id, TestCaseExerciseUpdateDto testCaseExerciseUpdateDto)
        {
            try
            {
                var testCaseInDb = await _testCaseRepository.GetByIdAsync(id);
                if (testCaseExerciseUpdateDto.InputData != null)
                {
                    await HandleFile.DeleteFile("Inputs", testCaseInDb.InputData);
                    testCaseInDb.InputData = await HandleFile.Upload("Inputs", testCaseExerciseUpdateDto.InputData);
                }
                if (testCaseExerciseUpdateDto.ExpectedOutput != null)
                {
                    await HandleFile.DeleteFile("Outputs", testCaseInDb.ExpectedOutput);
                    testCaseInDb.ExpectedOutput = await HandleFile.Upload("Outputs", testCaseExerciseUpdateDto.ExpectedOutput);
                }
                testCaseInDb.IsLock = testCaseExerciseUpdateDto.IsLock;
                _testCaseRepository.Update(testCaseInDb);
                await _testCaseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

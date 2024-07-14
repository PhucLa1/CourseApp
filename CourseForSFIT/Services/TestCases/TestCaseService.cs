using AutoMapper;
using Data.Entities;
using Dtos.Models.TestCaseModels;
using Dtos.Results;
using Dtos.Results.TestCaseResults;
using Microsoft.EntityFrameworkCore;
using Repositories.unitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.TestCases
{
    public interface ITestCaseService
    {
        Task<ApiResponse<TestCaseReturnDto>> GetAllTestCaseNotLockByExerciseId(int exerciseId);
        Task<ApiResponse<List<TestCaseDto>>> GetAllTestCaseByExerciseId(int exerciseId);
        Task<ApiResponse<bool>> AddTestCase(TestCaseAddDto testCaseAddDto);
        Task<ApiResponse<bool>> DeleteTestCase(int id);
        Task<ApiResponse<bool>> UpdateTestCase(int id, TestCaseUpdateDto testCaseUpdateDto);
    }
    public class TestCaseService : ITestCaseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TestCaseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse<TestCaseReturnDto>> GetAllTestCaseNotLockByExerciseId(int exerciseId)
        {
            try
            {
                return new ApiResponse<TestCaseReturnDto> { IsSuccess = true , Metadata = new TestCaseReturnDto 
                { 
                    testCaseDtos = _mapper.Map<List<TestCaseDto>>(await _unitOfWork.TestCaseRepository.GetAllTestCaseNotLockByExerciseId(exerciseId)),
                    totalTestCaseCount = await _unitOfWork.TestCaseRepository.GetAllTestCaseByExerciseId(exerciseId).CountAsync()
                }};
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> AddTestCase(TestCaseAddDto testCaseAddDto)
        {
            try
            {
                await _unitOfWork.TestCaseRepository.AddAsync(_mapper.Map<TestCase>(testCaseAddDto));
                await _unitOfWork.SaveAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<List<TestCaseDto>>> GetAllTestCaseByExerciseId(int exerciseId)
        {
            try
            {
                return new ApiResponse<List<TestCaseDto>> { IsSuccess = true, Metadata = _mapper.Map<List<TestCaseDto>>(await _unitOfWork.TestCaseRepository.GetAllTestCaseByExerciseId(exerciseId).ToListAsync()) };
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> DeleteTestCase(int id)
        {
            try
            {
                await _unitOfWork.TestCaseRepository.RemoveAsync(id);
                await _unitOfWork.SaveAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> UpdateTestCase(int id,TestCaseUpdateDto testCaseUpdateDto)
        {
            try
            {
                await _unitOfWork.TestCaseRepository.UpdateAsync(id, _mapper.Map<TestCase>(testCaseUpdateDto));
                await _unitOfWork.SaveAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

using AutoMapper;
using Data.Entities;
using Dtos.Models;
using Dtos.Results;
using Dtos.Results.TagExerciseResults;
using Microsoft.AspNetCore.Mvc;
using Repositories.unitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Exercises
{
    public interface ITagExerciseService
    {
        Task<ApiResponse<IEnumerable<TagExerciseDto>>> GetAllTagExercise();
        Task<ApiResponse<IEnumerable<TagExerciseAdminDto>>> GetAllTagExerciseAdminDto();
        Task<ApiResponse<string>> GetTagExercisesById(int id);
        Task<ApiResponse<bool>> UpdateTagExercise(int id,TagExerciseUpdateDto tagExerciseUpdateDto);
        Task<ApiResponse<bool>> DeleteTagExercise(int id);
        Task<ApiResponse<bool>> AddTagExercise(TagExerciseAddDto tagExerciseAddDto);
    }
    public class TagExerciseService : ITagExerciseService
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TagExerciseService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse<IEnumerable<TagExerciseDto>>> GetAllTagExercise()
        {
            try
            {
                IEnumerable<TagExercise> tagExercises = await _unitOfWork.TagExerciseRepository.GetAllAsync();
                return new ApiResponse<IEnumerable<TagExerciseDto>> { Metadata = _mapper.Map<List<TagExerciseDto>>(tagExercises), IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<IEnumerable<TagExerciseAdminDto>>> GetAllTagExerciseAdminDto()
        {
            try
            {
                return new ApiResponse<IEnumerable<TagExerciseAdminDto>> { Metadata = _mapper.Map<List<TagExerciseAdminDto>>(await _unitOfWork.TagExerciseRepository.GetAllAsync()), IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<string>> GetTagExercisesById(int id)
        {
            try
            {
                TagExercise tagExercise = await _unitOfWork.TagExerciseRepository.GetByIdAsync(id);
                return new ApiResponse<string> { IsSuccess = true, Metadata = tagExercise.TagName};
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> UpdateTagExercise(int id, TagExerciseUpdateDto tagExerciseUpdateDto)
        {
            try
            {
                var res = await _unitOfWork.TagExerciseRepository.GetByIdAsync(id);
                res.TagName = tagExerciseUpdateDto.TagName;
                await _unitOfWork.TagExerciseRepository.UpdateAsync(id, res);
                await _unitOfWork.SaveAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> DeleteTagExercise(int id)
        {
            try
            {
                await _unitOfWork.TagExerciseRepository.RemoveAsync(id);
                await _unitOfWork.SaveAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> AddTagExercise(TagExerciseAddDto tagExerciseAddDto)
        {
            try
            {
                await _unitOfWork.TagExerciseRepository.AddAsync(_mapper.Map<TagExercise>(tagExerciseAddDto));
                await _unitOfWork.SaveAsync();
                return new ApiResponse<bool> {IsSuccess = true };
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

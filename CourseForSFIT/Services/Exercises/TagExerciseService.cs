using AutoMapper;
using Data.Entities;
using Dtos.Models.ExerciseModels;
using Dtos.Results;
using Dtos.Results.ExerciseResults;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MimeKit.Tnef;
using Repositories.Repositories.Base;

namespace Services.Exercises
{
    public interface ITagExerciseService
    {
        Task<ApiResponse<IEnumerable<TagExerciseDto>>> GetAllTagExercise();
        Task<ApiResponse<IEnumerable<TagExerciseDto>>> GetTagExercisesByExerciseId(int exerciseId);
        Task<ApiResponse<IEnumerable<TagExerciseAdminDto>>> GetAllTagExerciseAdminDto();
        Task<ApiResponse<string>> GetTagExercisesById(int id);
        Task<ApiResponse<bool>> UpdateTagExercise(int id, TagExerciseUpdateDto tagExerciseUpdateDto);
        Task<ApiResponse<bool>> DeleteTagExercise(int id);
        Task<ApiResponse<bool>> AddTagExercise(TagExerciseAddDto tagExerciseAddDto);
        Task<ApiResponse<bool>> AddDataSample();
    }
    public class TagExerciseService : ITagExerciseService
    {
        private readonly IBaseRepository<TagExercise> _tagExerciseRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<TagExerciseUpdateDto> _validatorTagExerciseUpdateDto;
        private readonly IValidator<TagExerciseAddDto> _validatorTagExerciseAddDto;
        public TagExerciseService(IBaseRepository<TagExercise> tagExerciseRepository, 
            IMapper mapper,
            IValidator<TagExerciseUpdateDto> validatorTagExerciseUpdateDto,
            IValidator<TagExerciseAddDto> validatorTagExerciseAddDto)
        {
            _tagExerciseRepository = tagExerciseRepository;
            _mapper = mapper;
            _validatorTagExerciseAddDto = validatorTagExerciseAddDto;
            _validatorTagExerciseUpdateDto = validatorTagExerciseUpdateDto;
        }
        public async Task<ApiResponse<IEnumerable<TagExerciseDto>>> GetAllTagExercise()
        {
            try
            {
                IEnumerable<TagExercise> tagExercises = await _tagExerciseRepository.GetAllAsync();
                return new ApiResponse<IEnumerable<TagExerciseDto>> { Metadata = _mapper.Map<List<TagExerciseDto>>(tagExercises), IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<IEnumerable<TagExerciseDto>>> GetTagExercisesByExerciseId(int exerciseId)
        {
            try
            {
                return new ApiResponse<IEnumerable<TagExerciseDto>> { Metadata = _mapper.Map<List<TagExerciseDto>>(await _tagExerciseRepository.GetAllQueryAble().Include(e => e.ExerciseHasTags).Where(e => e.ExerciseHasTags != null && e.ExerciseHasTags.Select(e => e.ExerciseId).Contains(exerciseId)).ToListAsync()), IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> AddDataSample()
        {
            try
            {
                List<TagExerciseAddDto> exerciseAddDtos = new List<TagExerciseAddDto>
                {
                    new TagExerciseAddDto(){ Name = "Người mới"},
                        new TagExerciseAddDto() { Name = "Sắp xếp" },
                        new TagExerciseAddDto() { Name = "Tìm kiếm" },
                        new TagExerciseAddDto() { Name = "Đệ quy" },
                        new TagExerciseAddDto() { Name = "Quy hoạch động" },
                        new TagExerciseAddDto() { Name = "Đồ thị" },
                        new TagExerciseAddDto() { Name = "Lý thuyết số" },
                        new TagExerciseAddDto() { Name = "Thuật toán tham lam" },
                        new TagExerciseAddDto() { Name = "Chia để trị" },
                        new TagExerciseAddDto() { Name = "Backtracking" },
                        new TagExerciseAddDto() { Name = "Duyệt cây" }
                };
                await _tagExerciseRepository.AddManyAsync(_mapper.Map<List<TagExercise>>(exerciseAddDtos));
                await _tagExerciseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
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
                return new ApiResponse<IEnumerable<TagExerciseAdminDto>> { Metadata = _mapper.Map<List<TagExerciseAdminDto>>(await _tagExerciseRepository.GetAllAsync()), IsSuccess = true };
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
                TagExercise tagExercise = await _tagExerciseRepository.GetByIdAsync(id);
                return new ApiResponse<string> { IsSuccess = true, Metadata = tagExercise.Name };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> UpdateTagExercise(int id, TagExerciseUpdateDto tagExerciseUpdateDto)
        {
            try
            {
                var resultValidation = _validatorTagExerciseUpdateDto.Validate(tagExerciseUpdateDto);
                if(!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                var res = await _tagExerciseRepository.GetByIdAsync(id);
                res.Name = tagExerciseUpdateDto.Name.Trim();
                await _tagExerciseRepository.UpdateAsync(id, res);
                await _tagExerciseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> DeleteTagExercise(int id)
        {
            try
            {
                await _tagExerciseRepository.RemoveAsync(id);
                await _tagExerciseRepository.SaveChangeAsync();
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
                var resultValidation = _validatorTagExerciseAddDto.Validate(tagExerciseAddDto);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                var tagAdd = _mapper.Map<TagExercise>(tagExerciseAddDto);
                tagAdd.Name = tagExerciseAddDto.Name.Trim();
                await _tagExerciseRepository.AddAsync(tagAdd);
                await _tagExerciseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

using AutoMapper;
using Data.Entities;
using Dtos.Models.ExerciseModels;
using Dtos.Results;
using Dtos.Results.ExerciseResults;
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
        public TagExerciseService(IBaseRepository<TagExercise> tagExerciseRepository, IMapper mapper)
        {
            _tagExerciseRepository = tagExerciseRepository;
            _mapper = mapper;
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
                    new TagExerciseAddDto(){ TagName = "Người mới"},
                        new TagExerciseAddDto() { TagName = "Sắp xếp" },
                        new TagExerciseAddDto() { TagName = "Tìm kiếm" },
                        new TagExerciseAddDto() { TagName = "Đệ quy" },
                        new TagExerciseAddDto() { TagName = "Quy hoạch động" },
                        new TagExerciseAddDto() { TagName = "Đồ thị" },
                        new TagExerciseAddDto() { TagName = "Lý thuyết số" },
                        new TagExerciseAddDto() { TagName = "Thuật toán tham lam" },
                        new TagExerciseAddDto() { TagName = "Chia để trị" },
                        new TagExerciseAddDto() { TagName = "Backtracking" },
                        new TagExerciseAddDto() { TagName = "Duyệt cây" }
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
                return new ApiResponse<string> { IsSuccess = true, Metadata = tagExercise.TagName };
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
                var res = await _tagExerciseRepository.GetByIdAsync(id);
                res.TagName = tagExerciseUpdateDto.TagName;
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
                await _tagExerciseRepository.AddAsync(_mapper.Map<TagExercise>(tagExerciseAddDto));
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

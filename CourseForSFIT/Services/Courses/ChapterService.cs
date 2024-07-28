using Data.Entities;
using Dtos.Models.CourseModels;
using Dtos.Results;
using Dtos.Results.CourseResults;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.Base;

namespace Services.Courses
{
    public interface IChapterService
    {
        Task<ApiResponse<List<ChapterDto>>> GetAllChaptesByCourseId(int courseId);
        Task<ApiResponse<bool>> AddNewChapter(ChapterAdd chapterAdd);
        Task<ApiResponse<string>> UpdateChapter(int id, string chapterName);
        Task<ApiResponse<bool>> DeleteChapter(int id);
    }
    public class ChapterService : IChapterService
    {
        private readonly IBaseRepository<Chapter> _chapterRepository;
        private readonly IValidator<ChapterAdd> _chapterAddValidator;
        public ChapterService(IBaseRepository<Chapter> chapterRepository, IValidator<ChapterAdd> chapterAddValidator) { _chapterRepository = chapterRepository; _chapterAddValidator = chapterAddValidator; }
        public async Task<ApiResponse<List<ChapterDto>>> GetAllChaptesByCourseId(int courseId)
        {
            try
            {
                return new ApiResponse<List<ChapterDto>> { IsSuccess = true, Metadata = await _chapterRepository.GetAllQueryAble().Where(e => e.CourseId == courseId).Select(e => new ChapterDto() { Name = e.Name, Id = e.Id }).ToListAsync() };
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> AddNewChapter(ChapterAdd chapterAdd)
        {
            try
            {
                var resultValidation = _chapterAddValidator.Validate(chapterAdd);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                Chapter chapter = new Chapter() { Name = chapterAdd.Name.Trim(), CourseId = chapterAdd.CourseId };
                await _chapterRepository.AddAsync(chapter);
                await _chapterRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<string>> UpdateChapter(int id, string chapterName)
        {
            try
            {
                if(chapterName.Trim() == "")
                {
                    return new ApiResponse<string> { Message = ["Không thể truyền vào chuỗi rỗng"] };
                }  
                Chapter chapter = await _chapterRepository.GetByIdAsync(id);
                chapter.Name = chapterName.Trim();
                _chapterRepository.Update(chapter);
                await _chapterRepository.SaveChangeAsync();
                return new ApiResponse<string> { IsSuccess = true, Metadata = chapterName.Trim() };
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> DeleteChapter(int id)
        {
            try
            {
                await _chapterRepository.RemoveAsync(id);
                await _chapterRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

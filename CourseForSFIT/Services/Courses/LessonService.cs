using AutoMapper;
using Data.Entities;
using Dtos.Models.CourseModels;
using Dtos.Results;
using Dtos.Results.CourseResults;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NPOI.HPSF;
using NPOI.SS.Formula.Functions;
using Repositories.Repositories.Base;

namespace Services.Courses
{
    public interface ILessonService
    {
        Task<ApiResponse<List<LessonDto>>> GetLessonByChapterId(int chapterId);
        Task<ApiResponse<int>> AddNewLesson(LessonAddDto lessonAdd);
        Task<ApiResponse<bool>> DeleteLesson(int id);
        Task<ApiResponse<bool>> UpdateChunkFile(IFormFile chunkFile, int chunkIndex, int totalChunk, int id);
    }
    public class LessonService : ILessonService
    {
        private readonly IBaseRepository<Lesson> _lessonRepository;
        private readonly IValidator<LessonAddDto> _lessonAddValidator;
        private readonly IMapper _mapper;

        public LessonService(IBaseRepository<Lesson> lessonRepository,
            IMapper mapper,
            IValidator<LessonAddDto> lessonAddValidator
            )
        {
            _lessonRepository = lessonRepository;
            _mapper = mapper;
            _lessonAddValidator = lessonAddValidator;

        }

        public async Task<ApiResponse<List<LessonDto>>> GetLessonByChapterId(int chapterId)
        {
            try
            {
                return new ApiResponse<List<LessonDto>> { IsSuccess = true, Metadata = _mapper.Map<List<LessonDto>>(await _lessonRepository.GetAllQueryAble().Where(e => e.ChapterId == chapterId).ToListAsync()) };
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"An error occurred while saving the entity changes: {innerException}");
            }
        }
        public async Task<ApiResponse<int>> AddNewLesson(LessonAddDto lessonAdd)
        {
            try
            {
                var resultValidation = _lessonAddValidator.Validate(lessonAdd);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<int>.FailtureValidation(resultValidation.Errors);
                }
                lessonAdd.Name = lessonAdd.Name.Trim();
                Lesson lesson = _mapper.Map<Lesson>(lessonAdd);
                lesson.Name = lesson.Name.Trim();
                await _lessonRepository.AddAsync(lesson);
                await _lessonRepository.SaveChangeAsync();

                return new ApiResponse<int> { IsSuccess = true, Metadata = lesson.Id };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> DeleteLesson(int id)
        {
            try
            {
                await _lessonRepository.RemoveAsync(id);
                await _lessonRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> UpdateChunkFile(IFormFile chunkFile, int chunkIndex, int totalChunk, int id)
        {
            try
            {
                var lessonLast = await _lessonRepository.GetByIdAsync(id);
                lessonLast.TotalChunk = totalChunk;
                string _tempPath = Path.Combine(Directory.GetCurrentDirectory(), "VideoUploads", DateTime.Now.Ticks.ToString());
                
                if (!Directory.Exists(_tempPath))
                {
                    Directory.CreateDirectory(_tempPath);
                }
                var extension = "." + chunkFile.FileName.Split('.')[chunkFile.FileName.Split('.').Length - 1];
                string fileName = chunkIndex + extension;
                var exactPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "VideoUploads", fileName);
                using (var stream = new FileStream(exactPath, FileMode.Create))
                {
                    await chunkFile.CopyToAsync(stream);
                }
                lessonLast.ChunkIndex = chunkIndex;
                if (chunkIndex < totalChunk)
                {
                    lessonLast.StatusUpload = StatusUpload.Loading;
                }
                else
                {
                    lessonLast.StatusUpload = StatusUpload.Success;
                    lessonLast.Status = LessonStatus.Public;
                }
                _lessonRepository.Update(lessonLast);
                await _lessonRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

using Data.Entities;
using Dtos.Models.CourseModels;
using Dtos.Models.ExerciseModels;
using Dtos.Results;
using Dtos.Results.CourseResults;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Repositories.Repositories.Base;
using Shared;

namespace Services.Courses
{
    public interface ICourseService
    {
        Task<ApiResponse<bool>> AddNewCourse(CourseAddDto courseAddDto);
        Task<ApiResponse<bool>> UpdateCourse(int id, CourseUpdateDto courseUpdateDto);
        Task<ApiResponse<List<CourseShowAdminDto>>> GetCourseByOptionsAdminPage(CourseRequest courseRequest);
        Task<ApiResponse<bool>> DeleteCourse(int id);
        Task<ApiResponse<CourseDto>> GetCourseById(int id);
    }
    public class CourseService : ICourseService
    {
        private readonly IBaseRepository<Course> _courseRepository;
        private readonly IBaseRepository<CourseType> _courseTypeRepository;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IValidator<CourseAddDto> _courseAddValidator;
        public CourseService(IBaseRepository<Course> courseRepository, 
            IBaseRepository<CourseType> courseTypeRepository, 
            IBaseRepository<User> userRepository,
            IValidator<CourseAddDto> courseAddValidator
            )
        {
            _courseRepository = courseRepository;
            _courseTypeRepository = courseTypeRepository;
            _userRepository = userRepository;
            _courseAddValidator = courseAddValidator;
        }
        public async Task<ApiResponse<bool>> AddNewCourse(CourseAddDto courseAddDto)
        {
            try
            {
                var resultValidation = _courseAddValidator.Validate(courseAddDto);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                Course course = new Course()
                {
                    Name = courseAddDto.Name,
                    Description = courseAddDto.Description,
                    Status = courseAddDto.Status,
                    Thumbnail = courseAddDto.Thumbnail == null ? null : await HandleImage.Upload(courseAddDto.Thumbnail),
                    Prepared = JsonConvert.SerializeObject(courseAddDto.ListPrepared?.Select(s => s.Trim()).ToList()),
                    LearnAbout = JsonConvert.SerializeObject(courseAddDto.ListLearnAbout?.Select(s => s.Trim()).ToList()),
                    CourseTypeId = courseAddDto.CourseTypeId,
                };
                await _courseRepository.AddAsync(course);
                await _courseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"An error occurred while saving the entity changes: {innerException}");
            }
        }
        public async Task<ApiResponse<bool>> UpdateCourse(int id, CourseUpdateDto courseUpdateDto)
        {
            try
            {
                var resultValidation = _courseAddValidator.Validate(courseUpdateDto);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                Course courseInDb = await _courseRepository.GetByIdAsync(id);
                courseInDb.Name = courseUpdateDto.Name;
                courseInDb.Description = courseUpdateDto.Description;
                courseInDb.Prepared = JsonConvert.SerializeObject(courseUpdateDto.ListPrepared?.Select(s => s.Trim()).ToList());
                courseInDb.LearnAbout = JsonConvert.SerializeObject(courseUpdateDto.ListLearnAbout?.Select(s => s.Trim()).ToList());
                courseInDb.Status = courseUpdateDto.Status;
                courseInDb.CourseTypeId = courseUpdateDto.CourseTypeId;
                if (courseUpdateDto.Thumbnail != null)
                {
                    if (courseInDb.Thumbnail != null) await HandleFile.DeleteFile("Uploads", courseInDb.Thumbnail);
                    courseInDb.Thumbnail = await HandleImage.Upload(courseUpdateDto.Thumbnail);
                }
                _courseRepository.Update(courseInDb);
                await _courseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<List<CourseShowAdminDto>>> GetCourseByOptionsAdminPage(CourseRequest courseRequest)
        {
            try
            {
                IQueryable<CourseShowAdminDto> query = from ct in _courseTypeRepository.GetAllQueryAble()
                                                       join c in _courseRepository.GetAllQueryAble()
                                                       on ct.Id equals c.CourseTypeId
                                                       join u in _userRepository.GetAllQueryAble()
                                                       on c.CreatedBy equals u.Id
                                                       group new { c, u } by new { ct.Id, ct.Name } into grouped
                                                       select new CourseShowAdminDto
                                                       {
                                                           Id = grouped.Key.Id,
                                                           Name = grouped.Key.Name,
                                                           courseAdminDtos = grouped.Select(g => new CourseAdminDto
                                                           {
                                                               Id = g.c.Id,
                                                               Thumbnail = g.c.Thumbnail,
                                                               Name = g.c.Name,
                                                               CreatedByPerson = g.u.FirstName + " " + g.u.LastName // Assuming 'Name' is the user's name
                                                           }).ToList()
                                                       };
                if (courseRequest.CourseTypeId != null && courseRequest.CourseTypeId.Any())
                {
                    query = query.Where(e => courseRequest.CourseTypeId.Contains(e.Id));
                }
                var result = await query.ToListAsync();
                if (!string.IsNullOrEmpty(courseRequest.Name))
                {
                    result = result.Where(e => e.courseAdminDtos != null && e.courseAdminDtos.Any(c => c.Name.Contains(courseRequest.Name))).ToList();
                }
                return new ApiResponse<List<CourseShowAdminDto>> { IsSuccess = true, Metadata = result };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ApiResponse<bool>> DeleteCourse(int id)
        {
            try
            {
                Course courseInDb = await _courseRepository.GetByIdAsync(id);
                if (courseInDb.Thumbnail != null)
                {
                    if (courseInDb.Thumbnail != null) await HandleFile.DeleteFile("Uploads", courseInDb.Thumbnail);
                }
                await _courseRepository.RemoveAsync(id);
                await _courseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<CourseDto>> GetCourseById(int id)
        {
            try
            {
                var result = await _courseRepository.GetByIdAsync(id);
                return new ApiResponse<CourseDto> { IsSuccess = true, Metadata = new CourseDto(){Name = result.Name,Description = result.Description,CourseTypeId = result.CourseTypeId,ListLearnAbout = System.Text.Json.JsonSerializer.Deserialize<List<string>>(result.LearnAbout),ListPrepared = System.Text.Json.JsonSerializer.Deserialize<List<string>>(result.Prepared),Thumbnail = result.Thumbnail,Status = result.Status}};
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

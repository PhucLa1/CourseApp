using Data.Entities;
using Dtos.Models.CourseModels;
using Dtos.Results;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NPOI.HPSF;
using Repositories.Repositories.Base;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Courses
{
    public interface ICourseService
    {
        Task<ApiResponse<bool>> AddNewCourse(CourseAddDto courseAddDto);
        Task<ApiResponse<bool>> UpdateCourse(int id, CourseUpdateDto courseUpdateDto);
        Task<ApiResponse<List<CourseShowAdminDto>>> GetCourseByOptionsAdminPage(CourseRequest courseRequest);
        Task<ApiResponse<bool>> DeleteCourse(int id);
    }
    public class CourseService : ICourseService
    {
        private readonly IBaseRepository<Course> _courseRepository;
        private readonly IBaseRepository<CourseType> _courseTypeRepository;
        private readonly IBaseRepository<User> _userRepository;
        public CourseService(IBaseRepository<Course> courseRepository, IBaseRepository<CourseType> courseTypeRepository, IBaseRepository<User> userRepository)
        {
            _courseRepository = courseRepository;
            _courseTypeRepository = courseTypeRepository;
            _userRepository = userRepository;
        }
        public async Task<ApiResponse<bool>> AddNewCourse(CourseAddDto courseAddDto)
        {
            try
            {
                Course course = new Course()
                {
                    CourseName = courseAddDto.CourseName,
                    Description = courseAddDto.Description,
                    Status = courseAddDto.Status,
                    Thumbnail = courseAddDto.Thumbnail == null ? null : await HandleImage.Upload(courseAddDto.Thumbnail),
                    Prepared = JsonConvert.SerializeObject(courseAddDto.ListPrepared),
                    LearnAbout = JsonConvert.SerializeObject(courseAddDto.ListLearnAbout),
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
                Course courseInDb = await _courseRepository.GetByIdAsync(id);
                courseInDb.CourseName = courseUpdateDto.CourseName;
                courseInDb.Description = courseUpdateDto.Description;
                courseInDb.Prepared = JsonConvert.SerializeObject(courseUpdateDto.ListPrepared);
                courseInDb.LearnAbout = JsonConvert.SerializeObject(courseUpdateDto.ListLearnAbout);
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
                                                       group new { c, u } by new { ct.Id, ct.TypeName } into grouped
                                                       select new CourseShowAdminDto
                                                       {
                                                           Id = grouped.Key.Id,
                                                           TypeName = grouped.Key.TypeName,
                                                           courseAdminDtos = grouped.Select(g => new CourseAdminDto
                                                           {
                                                               Id = g.c.Id,
                                                               Thumbnail = g.c.Thumbnail,
                                                               CourseName = g.c.CourseName,
                                                               CreatedByPerson = g.u.FirstName + " " + g.u.LastName // Assuming 'Name' is the user's name
                                                           }).ToList()
                                                       };
                if (courseRequest.CourseTypeId != null && courseRequest.CourseTypeId.Any())
                {
                    query = query.Where(e => courseRequest.CourseTypeId.Contains(e.Id));
                }
                var result = await query.ToListAsync();
                if (!string.IsNullOrEmpty(courseRequest.CourseName))
                {
                    result = result.Where(e => e.courseAdminDtos != null && e.courseAdminDtos.Any(c => c.CourseName.Contains(courseRequest.CourseName))).ToList();
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
                await _courseRepository.RemoveAsync(id);
                await _courseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

using Data.Entities;
using Dtos.Models.CourseTypeModels;
using Dtos.Results;
using Dtos.Results.CourseTypeResults;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Courses
{
    public interface ICourseTypeService
    {
        Task<ApiResponse<List<CourseTypeDto>>> GetAllCourseTypes();
        Task<ApiResponse<bool>> AddNewCourseType(CourseTypeAdd courseTypeAdd);
        Task<ApiResponse<bool>> UpdateCourseType(int id, CourseTypeUpdate courseTypeUpdate);
        Task<ApiResponse<bool>> DeleteCourseType(int id);

    }
    public class CourseTypeService : ICourseTypeService
    {
        private readonly IBaseRepository<CourseType> _courseTypeRepository;
        public CourseTypeService(IBaseRepository<CourseType> courseTypeRepository)
        {
            _courseTypeRepository = courseTypeRepository;
        }
        public async Task<ApiResponse<List<CourseTypeDto>>> GetAllCourseTypes()
        {
            return new ApiResponse<List<CourseTypeDto>> { IsSuccess = true, Metadata = await _courseTypeRepository.GetAllQueryAble().Select(e => new CourseTypeDto() { Id = e.Id, TypeName = e.TypeName }).ToListAsync() };
        }
        public async Task<ApiResponse<bool>> AddNewCourseType(CourseTypeAdd courseTypeAdd)
        {
            await _courseTypeRepository.AddAsync(new CourseType() { TypeName = courseTypeAdd.TypeName });
            await _courseTypeRepository.SaveChangeAsync();
            return new ApiResponse<bool> { IsSuccess = true };
        }
        public async Task<ApiResponse<bool>> UpdateCourseType(int id, CourseTypeUpdate courseTypeUpdate)
        {
            var courseType = await _courseTypeRepository.GetByIdAsync(id);
            courseType.TypeName = courseTypeUpdate.TypeName;
            _courseTypeRepository.Update(courseType);
            await _courseTypeRepository.SaveChangeAsync();
            return new ApiResponse<bool> { IsSuccess = true };
        }
        public async Task<ApiResponse<bool>> DeleteCourseType(int id)
        {
            await _courseTypeRepository.RemoveAsync(id);
            await _courseTypeRepository.SaveChangeAsync();
            return new ApiResponse<bool> { IsSuccess = true };
        }
    }
}

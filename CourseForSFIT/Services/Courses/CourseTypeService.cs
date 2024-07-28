using Data.Entities;
using Dtos.Models.CourseTypeModels;
using Dtos.Results;
using Dtos.Results.CourseTypeResults;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.Base;

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
        private readonly IValidator<CourseTypeAdd> _courseTypeAddValidator;
        private readonly IValidator<CourseTypeUpdate> _courseTypeUpdateValidator;
        public CourseTypeService(IBaseRepository<CourseType> courseTypeRepository,
            IValidator<CourseTypeAdd> courseTypeAddValidator,
            IValidator<CourseTypeUpdate> courseTypeUpdateValidator
            )
        {
            _courseTypeRepository = courseTypeRepository;
            _courseTypeAddValidator = courseTypeAddValidator;
            _courseTypeUpdateValidator = courseTypeUpdateValidator;
        }
        public async Task<ApiResponse<List<CourseTypeDto>>> GetAllCourseTypes()
        {
            return new ApiResponse<List<CourseTypeDto>> { IsSuccess = true, Metadata = await _courseTypeRepository.GetAllQueryAble().Select(e => new CourseTypeDto() { Id = e.Id, Name = e.Name }).ToListAsync() };
        }
        public async Task<ApiResponse<bool>> AddNewCourseType(CourseTypeAdd courseTypeAdd)
        {
            var resultValidator = _courseTypeAddValidator.Validate(courseTypeAdd);
            if(!resultValidator.IsValid)
            {
                return ApiResponse<bool>.FailtureValidation(resultValidator.Errors);
            }
            await _courseTypeRepository.AddAsync(new CourseType() { Name = courseTypeAdd.Name.Trim() });
            await _courseTypeRepository.SaveChangeAsync();
            return new ApiResponse<bool> { IsSuccess = true };
        }
        public async Task<ApiResponse<bool>> UpdateCourseType(int id, CourseTypeUpdate courseTypeUpdate)
        {
            var resultValidator = _courseTypeUpdateValidator.Validate(courseTypeUpdate);
            if (!resultValidator.IsValid)
            {
                return ApiResponse<bool>.FailtureValidation(resultValidator.Errors);
            }
            var courseType = await _courseTypeRepository.GetByIdAsync(id);
            courseType.Name = courseTypeUpdate.Name.Trim();
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

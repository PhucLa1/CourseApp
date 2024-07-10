using AutoMapper;
using Data.Entities;
using Dtos.Models;
using Dtos.Results;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.IRepo;
using Repositories.unitOfWork;
using Shared;
using SixLabors.Fonts.Tables.AdvancedTypographic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Services.Exercises
{
    public interface IExerciseService
    {
        Task<ApiResponse<PagedResult<ExerciseDto>>> GetExerciseByOptionsPaginated(ExerciseRequest exerciseRequest, int pageNumber = 1, int pageSize = 10);
        Task<ApiResponse<string>> GetTopicExercise(int id);
        Task<object> Test();
    }
    public class ExerciseService : IExerciseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ExerciseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<ApiResponse<PagedResult<ExerciseDto>>> GetExerciseByOptionsPaginated(ExerciseRequest exerciseRequest, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                IQueryable<Exercise> exercises = _unitOfWork.ExerciseRepository.GetAllQueryAble();
                if (exerciseRequest.TagId != null && exerciseRequest.TagId.Any())
                {
                    var tagIds = exerciseRequest.TagId.ToList();
                    exercises = exercises.WhereIn("Tag", tagIds);
                }
                if (exerciseRequest.DifficultLevel != null && exerciseRequest.DifficultLevel.Any())
                {
                    exercises = exercises.Where(e => exerciseRequest.DifficultLevel.Contains(e.DifficultLevel));
                }
                if (1 == 1) //Cái này dành cho phần status sẽ xây dựng sauu
                {

                }
                if (exerciseRequest.Name != null)
                {
                    exercises = exercises.Where(e => e.ExerciseName.Contains(exerciseRequest.Name));
                }
                return new ApiResponse<PagedResult<ExerciseDto>> { IsSuccess = true, Metadata = HandlePagination<ExerciseDto>.PageList(pageNumber, exercises.Count(), pageSize, _mapper.Map<List<ExerciseDto>>(exercises.Skip((pageNumber - 1) * pageSize).Take(pageSize))) };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<object> Test()
        {
            var query = from e in _unitOfWork.ExerciseRepository.GetAllQueryAble()
                        join eht in _unitOfWork.ExerciseHasTagRepository.GetAllQueryAble()
                        on e.Id equals eht.ExerciseId
                        join et in _unitOfWork.TagExerciseRepository.GetAllQueryAble()
                        on eht.TagExerciseId equals et.Id
                        into exercise
                        select new
                        {
                            e,exercise
                        };          
             return query.ToList();
    }


        public async Task<ApiResponse<string>> GetTopicExercise(int id)
        {
            try
            {
                string topicExercise = await _unitOfWork.ExerciseRepository.GetFieldByIdAsync(id, e => e.ContentExercise);
                return new ApiResponse<string> { IsSuccess = true, Metadata = topicExercise };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

using AutoMapper;
using Data.Entities;
using Dtos.Models.AuthModels;
using Dtos.Models.ExerciseModels;
using Dtos.Models.TestCaseModels;
using Dtos.Results;
using Dtos.Results.ExerciseResults;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Repositories.unitOfWork;
using Shared;

namespace Services.Exercises
{
    public interface IExerciseService
    {
        Task<ApiResponse<PagedResult<ExerciseDto>>> GetExerciseByOptionsPaginated(ExerciseRequest exerciseRequest, int pageNumber = 1, int pageSize = 10);
        Task<ApiResponse<PagedResult<ExerciseAdminDto>>> GetAdminExerciseByOptionsPaginated(ExerciseRequest exerciseRequest, int pageNumber = 1, int pageSize = 10);
        Task<ApiResponse<TopicExercise>> GetTopicExercise(int id);
        Task<object> Test();
        Task<ApiResponse<bool>> AddExercise(ExerciseAddDto exerciseAddDto);
        Task<ApiResponse<bool>> DeleteExercise(int id);
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
                IQueryable<Exercise> exercises = _unitOfWork.ExerciseRepository.GetAllQueryAble().Include(e => e.ExerciseHasTags);
                if (exerciseRequest.TagId != null && exerciseRequest.TagId.Any())
                {
                    exercises = exercises.Where(e => e.ExerciseHasTags.Select(e => e.Id).ToList().Intersect(exerciseRequest.TagId).Any());
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
        public async Task<ApiResponse<bool>> DeleteExercise(int id)
        {
            try
            {
                await _unitOfWork.ExerciseRepository.RemoveAsync(id);
                await _unitOfWork.SaveAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<PagedResult<ExerciseAdminDto>>> GetAdminExerciseByOptionsPaginated(ExerciseRequest exerciseRequest, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                IQueryable<Exercise> exercises = _unitOfWork.ExerciseRepository.GetAllQueryAble().Include(e => e.ExerciseHasTags);
                if (exerciseRequest.TagId != null && exerciseRequest.TagId.Any())
                {
                    exercises = exercises.Where(e => e.ExerciseHasTags.Select(e => e.Id).ToList().Intersect(exerciseRequest.TagId).Any());
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
                IEnumerable<ExerciseAdminDto> exerciseAdminDtos = exercises.Select(e => new ExerciseAdminDto
                {
                    Id = e.Id,
                    ExerciseName = e.ExerciseName,
                    DifficultLevel = e.DifficultLevel,
                    NumberParticipants = e.NumberParticipants,
                    SuccessRate = e.SuccessRate,
                    Tags = _unitOfWork.TagExerciseRepository.GetAllQueryAble().Where(c => e.ExerciseHasTags.Select(e => e.Id).ToList().Contains(c.Id)).Select(e => e.TagName).ToList()
                }).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                return new ApiResponse<PagedResult<ExerciseAdminDto>> {
                    IsSuccess = true, 
                    Metadata = HandlePagination<ExerciseAdminDto>.PageList(pageNumber, exercises.Count(), pageSize, exerciseAdminDtos) 
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<object> Test()
        {
            return _unitOfWork.ExerciseRepository.GetAllQueryAble().Include(e => e.ExerciseHasTags).Select(e => new
            {
                c = e.SuccessRate,
                m = _unitOfWork.TagExerciseRepository.GetAllQueryAble().Where(c => e.ExerciseHasTags.Select(e => e.Id).ToList().Contains(c.Id)).Select(e => e.TagName).ToList(), 
            }).ToList();
        }
        public async Task<ApiResponse<bool>> AddExercise(ExerciseAddDto exerciseAddDto)
        {
            try
            {
                Exercise exercise = _mapper.Map<Exercise>(exerciseAddDto);
                await _unitOfWork.ExerciseRepository.AddAsync(exercise);
                await _unitOfWork.SaveAsync();
                
                List<ExerciseHasTagAddDto> exerciseHasTagAddDtos = new List<ExerciseHasTagAddDto>();
                List<TestCaseAddDto> testCases = new List<TestCaseAddDto>();
                if(exerciseAddDto.TagIds != null && exerciseAddDto.TagIds.Any())
                {
                    foreach(int tagId in exerciseAddDto.TagIds)
                    {
                        exerciseHasTagAddDtos.Add(new ExerciseHasTagAddDto() { ExerciseId = exercise.Id, TagExerciseId = tagId });
                    }
                    await _unitOfWork.ExerciseHasTagRepository.AddManyAsync(_mapper.Map<List<ExerciseHasTag>>(exerciseHasTagAddDtos));
                    
                }
                if(exerciseAddDto.TestCaseAddDtos != null && exerciseAddDto.TestCaseAddDtos.Any())
                {
                    foreach (TestCaseExerciseAddDto testCaseAddDto in exerciseAddDto.TestCaseAddDtos)
                    {
                        testCases.Add(new TestCaseAddDto() { 
                            ExerciseId = exercise.Id,
                            ExpectedOutput = await HandleFile.Upload("Outputs",testCaseAddDto.ExpectedOutput), 
                            InputData = await HandleFile.Upload("Inputs",testCaseAddDto.InputData), 
                            IsLock = testCaseAddDto.IsLock 
                        });
                    }
                    await _unitOfWork.TestCaseRepository.AddManyAsync(_mapper.Map<List<TestCase>>(testCases));
                }
                await _unitOfWork.SaveAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }catch (Exception ex)
            {
                var innerException = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"An error occurred while saving the entity changes: {innerException}");
            }
        }


        public async Task<ApiResponse<TopicExercise>> GetTopicExercise(int id)
        {
            try
            {
                string topicExercise = await _unitOfWork.ExerciseRepository.GetFieldByIdAsync(id, e => e.ContentExercise);
                var contentExercise = JsonConvert.DeserializeObject<TopicExercise>(topicExercise);
                return new ApiResponse<TopicExercise> { IsSuccess = true, Metadata = contentExercise };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

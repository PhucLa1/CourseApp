using AutoMapper;
using CacheManager.Core.Logging;
using Data.Entities;
using Dtos.Models.ExerciseModels;
using Dtos.Models.TestCaseModels;
using Dtos.Results;
using Dtos.Results.ExerciseResults;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using Repositories.Repositories.Base;
using Shared;

namespace Services.Exercises
{
    public interface IExerciseService
    {
        Task<ApiResponse<PagedResult<ExerciseDto>>> GetExerciseByOptionsPaginated(ExerciseRequest exerciseRequest, int pageNumber = 1, int pageSize = 10);
        Task<ApiResponse<PagedResult<ExerciseAdminDto>>> GetAdminExerciseByOptionsPaginated(ExerciseRequest exerciseRequest, int pageNumber = 1, int pageSize = 10);
        Task<ApiResponse<TopicExercise>> GetTopicExercise(int id);
        Task<ApiResponse<ContentExercise>> GetExerciseInfo(int id);
        Task<ApiResponse<bool>> AddExercise(ExerciseAddDto exerciseAddDto);
        Task<ApiResponse<bool>> DeleteExercise(int id);
        Task<ApiResponse<bool>> UpdateExercise(int id, ExerciseUpdateDto exerciseUpdateDto);
    }
    public class ExerciseService : IExerciseService
    {
        private readonly IBaseRepository<Exercise> _exerciseRepository;
        private readonly IBaseRepository<TagExercise> _tagExerciseRepository;
        private readonly IBaseRepository<ExerciseHasTag> _exerciseHasTagRepository;
        private readonly IBaseRepository<TestCase> _testCaseRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<ExerciseAddDto> _validatorExerciseAddDto;
        private readonly IValidator<ExerciseUpdateDto> _validatorExerciseUpdateDto;
        public ExerciseService(
            IBaseRepository<Exercise> exerciseRepository,
            IBaseRepository<TagExercise> tagExerciseRepository,
            IBaseRepository<ExerciseHasTag> exerciseHasTagRepository,
            IBaseRepository<TestCase> testCaseRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IValidator<ExerciseAddDto> validatorExerciseAddDto,
            IValidator<ExerciseUpdateDto> validatorExerciseUpdateDto
            )
        {
            _exerciseRepository = exerciseRepository;
            _tagExerciseRepository = tagExerciseRepository;
            _exerciseHasTagRepository = exerciseHasTagRepository;
            _testCaseRepository = testCaseRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _validatorExerciseAddDto = validatorExerciseAddDto;
            _validatorExerciseUpdateDto = validatorExerciseUpdateDto;
        }

        public async Task<ApiResponse<bool>> UpdateExercise(int id, ExerciseUpdateDto exerciseUpdateDto)
        {
            try
            {
                var resultValidation = _validatorExerciseUpdateDto.Validate(exerciseUpdateDto);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                var exerciseInDb = await _exerciseRepository.GetAllQueryAble().Where(e => e.Id == id).FirstAsync();
                exerciseInDb.Name = exerciseUpdateDto.Name;
                exerciseInDb.DifficultLevel = exerciseUpdateDto.Difficult;
                exerciseInDb.Content = JsonConvert.SerializeObject(exerciseUpdateDto.topicExercise);
                _exerciseRepository.Update(exerciseInDb);
                var exerciseHasTagsRemove = await _exerciseHasTagRepository.GetAllQueryAble().Where(e => e.ExerciseId == exerciseInDb.Id).ToListAsync();
                _exerciseHasTagRepository.GetDbSet().RemoveRange(exerciseHasTagsRemove);
                if (exerciseUpdateDto.TagIds != null && exerciseUpdateDto.TagIds.Any())
                {
                    List<ExerciseHasTagAddDto> exerciseHasTagAddDtos = new List<ExerciseHasTagAddDto>();
                    foreach (int tagId in exerciseUpdateDto.TagIds)
                    {
                        exerciseHasTagAddDtos.Add(new ExerciseHasTagAddDto() { ExerciseId = exerciseInDb.Id, TagExerciseId = tagId });
                    }
                    await _exerciseHasTagRepository.AddManyAsync(_mapper.Map<List<ExerciseHasTag>>(exerciseHasTagAddDtos));
                }
                await _exerciseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<PagedResult<ExerciseDto>>> GetExerciseByOptionsPaginated(ExerciseRequest exerciseRequest, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                IQueryable<Exercise> exercises = _exerciseRepository.GetAllQueryAble().Include(e => e.ExerciseHasTags);
                if (exerciseRequest.TagId != null && exerciseRequest.TagId.Any())
                {
                    exercises = exercises.Where(e => e.ExerciseHasTags != null && e.ExerciseHasTags.Select(e => e.TagExerciseId).AsQueryable().Intersect(exerciseRequest.TagId).Any());
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
                    exercises = exercises.Where(e => e.Name.Contains(exerciseRequest.Name));
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
                await _exerciseRepository.RemoveAsync(id);
                await _exerciseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<PagedResult<ExerciseAdminDto>>> GetAdminExerciseByOptionsPaginated(ExerciseRequest exerciseRequest, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                //int userId = _httpContextAccessor.HttpContext.Items["UserId"] == null ? 0 : int.Parse(_httpContextAccessor.HttpContext.Items["UserId"] as string);
                IQueryable<Exercise> exercises = _exerciseRepository.GetAllQueryAble().Include(e => e.ExerciseHasTags);              
                if (exerciseRequest.TagId != null && exerciseRequest.TagId.Any())
                {
                    exercises = exercises.Where(e => e.ExerciseHasTags != null && e.ExerciseHasTags.Select(e => e.TagExerciseId).AsQueryable().Intersect(exerciseRequest.TagId).Any());
                }
                if (exerciseRequest.DifficultLevel != null && exerciseRequest.DifficultLevel.Any())
                {
                    exercises = exercises.Where(e => exerciseRequest.DifficultLevel.Contains(e.DifficultLevel));
                }
                /*
                if (exerciseRequest.Status != null && exerciseRequest.Status.Any()) //Cái này dành cho phần status sẽ xây dựng sauu
                {
                    if (exerciseRequest.Status.Contains(1)) //Đã giải
                    {
                        exercises = exercises.Where(e => e.UserExercises != null && e.UserExercises.Any(ue => ue.UserId == userId) && e.UserExercises.Where(ue => ue.SuccessRate));
                    }
                }*/
                if (exerciseRequest.Name != null)
                {
                    exercises = exercises.Where(e => e.Name.Contains(exerciseRequest.Name));
                }
                IEnumerable<ExerciseAdminDto> exerciseAdminDtos = exercises.Select(e => new ExerciseAdminDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    DifficultLevel = e.DifficultLevel,
                    NumberParticipants = e.NumberParticipants,
                    SuccessRate = e.SuccessRate,
                    Tags = e.ExerciseHasTags != null ?_tagExerciseRepository.GetAllQueryAble().Where(c => e.ExerciseHasTags.Select(e => e.TagExerciseId).ToList().Contains(c.Id)).Select(e => e.Name).ToList() : null
                }).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                return new ApiResponse<PagedResult<ExerciseAdminDto>>
                {
                    IsSuccess = true,
                    Metadata = HandlePagination<ExerciseAdminDto>.PageList(pageNumber, exercises.Count(), pageSize, exerciseAdminDtos)
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> AddExercise(ExerciseAddDto exerciseAddDto)
        {
            try
            {
                var resultValidation = _validatorExerciseAddDto.Validate(exerciseAddDto);
                if(!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                Exercise exercise = _mapper.Map<Exercise>(exerciseAddDto);
                await _exerciseRepository.AddAsync(exercise);
                await _exerciseRepository.SaveChangeAsync();

                List<ExerciseHasTagAddDto> exerciseHasTagAddDtos = new List<ExerciseHasTagAddDto>();
                List<TestCaseAddDto> testCases = new List<TestCaseAddDto>();
                if (exerciseAddDto.TagIds != null && exerciseAddDto.TagIds.Any())
                {
                    foreach (int tagId in exerciseAddDto.TagIds)
                    {
                        exerciseHasTagAddDtos.Add(new ExerciseHasTagAddDto() { ExerciseId = exercise.Id, TagExerciseId = tagId });
                    }
                    await _exerciseHasTagRepository.AddManyAsync(_mapper.Map<List<ExerciseHasTag>>(exerciseHasTagAddDtos));

                }
                if (exerciseAddDto.TestCaseAddDtos != null && exerciseAddDto.TestCaseAddDtos.Any())
                {
                    foreach (TestCaseExerciseAddDto testCaseAddDto in exerciseAddDto.TestCaseAddDtos)
                    {
                        testCases.Add(new TestCaseAddDto()
                        {
                            ExerciseId = exercise.Id,
                            ExpectedOutput = await HandleFile.Upload("Outputs", testCaseAddDto.ExpectedOutput),
                            InputData = await HandleFile.Upload("Inputs", testCaseAddDto.InputData),
                            IsLock = testCaseAddDto.IsLock
                        });
                    }
                    await _testCaseRepository.AddManyAsync(_mapper.Map<List<TestCase>>(testCases));
                }
                await _testCaseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"An error occurred while saving the entity changes: {innerException}");
            }
        }


        public async Task<ApiResponse<TopicExercise>> GetTopicExercise(int id)
        {
            try
            {
                string topicExercise = await _exerciseRepository.GetFieldByIdAsync(id, e => e.Content);
                var contentExercise = JsonConvert.DeserializeObject<TopicExercise>(topicExercise);
                return new ApiResponse<TopicExercise> { IsSuccess = true, Metadata = contentExercise };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<ContentExercise>> GetExerciseInfo(int id)
        {
            try
            {
                var result = await _exerciseRepository.GetAllQueryAble().Where(e => e.Id == id).FirstOrDefaultAsync();
                var contentExercise = JsonConvert.DeserializeObject<TopicExercise>(result.Content);
                return new ApiResponse<ContentExercise> { IsSuccess = true, Metadata = new ContentExercise { Name = result.Name, Difficult = result.DifficultLevel, topicExercise = contentExercise } };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

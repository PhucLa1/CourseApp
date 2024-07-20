using AutoMapper;
using Data.Entities;
using Dtos.Models.ExerciseModels;
using Dtos.Results;
using Dtos.Results.ExerciseResults;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.Base;

namespace Services.Exercises
{
    public interface IExerciseCommentService
    {
        Task<ApiResponse<List<CommentExerciseDto>>> GetExerciseComment(int exerciseId);
        Task<ApiResponse<bool>> AddComment(CommentExerciseAddDto commentAddDto);
    }
    public class ExerciseCommentService : IExerciseCommentService
    {
        private IBaseRepository<ExerciseComment> _exerciseCommentRepository;
        private IBaseRepository<User> _userRepository;
        private readonly IMapper _mapper;
        public ExerciseCommentService(IBaseRepository<ExerciseComment> exerciseCommentRepository, IBaseRepository<User> userRepository, IMapper mapper)
        {
            _exerciseCommentRepository = exerciseCommentRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<ApiResponse<List<CommentExerciseDto>>> GetExerciseComment(int exerciseId)
        {
            try
            {
                var query = from ec in _exerciseCommentRepository.GetAllQueryAble().Where(e => e.ExerciseId == exerciseId)
                            join u in _userRepository.GetAllQueryAble()
                            on ec.CreatedBy equals u.Id
                            orderby ec.CreatedAt descending
                            select new CommentExerciseDto
                            {
                                Content = ec.Content,
                                UserAvatar = u.Avatar,
                                UserName = u.FirstName + " " + u.LastName
                            };
                List<CommentExerciseDto> comment = await query.ToListAsync();
                return new ApiResponse<List<CommentExerciseDto>> { IsSuccess = true, Metadata = comment };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> AddComment(CommentExerciseAddDto commentAddDto)
        {
            try
            {
                await _exerciseCommentRepository.AddAsync(_mapper.Map<ExerciseComment>(commentAddDto));
                await _exerciseCommentRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

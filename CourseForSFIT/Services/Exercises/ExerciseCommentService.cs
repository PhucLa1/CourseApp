using AutoMapper;
using Data.Entities;
using Dtos.Models;
using Dtos.Results;
using Microsoft.EntityFrameworkCore;
using Repositories.unitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Exercises
{
    public interface IExerciseCommentService
    {
        Task<ApiResponse<List<CommentExerciseDto>>> GetExerciseComment(int exerciseId);
        Task<ApiResponse<bool>> AddComment(CommentExerciseAddDto commentAddDto);
    }
    public class ExerciseCommentService : IExerciseCommentService
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ExerciseCommentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse<List<CommentExerciseDto>>> GetExerciseComment(int exerciseId)
        {
            try
            {
                var query = from ec in _unitOfWork.ExerciseCommentRepository.GetByExerciseId(exerciseId)
                            join u in _unitOfWork.UserRepository.GetAllQueryAble()
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
                await _unitOfWork.ExerciseCommentRepository.AddAsync(_mapper.Map<ExerciseComment>(commentAddDto));
                await _unitOfWork.SaveAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

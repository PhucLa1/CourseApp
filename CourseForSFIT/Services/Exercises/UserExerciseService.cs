using AutoMapper;
using Data.Entities;
using Dtos.Results;
using Dtos.Results.ExerciseResults;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.Base;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Exercises
{
    public interface IUserExerciseService
    {
        Task<ApiResponse<ContentCodes>> GetContentCodeLastest(int exerciseId);
        Task<ApiResponse<PagedResult<UserExerciseSubmit>>> GetUserSubmission(int exerciseId, int isMine = 1, int pageNumber = 1, int pageSize = 10);
    }
    public class UserExerciseService : IUserExerciseService
    {
        private readonly IBaseRepository<UserExercise> _userExerciseRepository;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public UserExerciseService(IBaseRepository<UserExercise> userExerciseRepository, IBaseRepository<User> userRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _userExerciseRepository = userExerciseRepository;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<ApiResponse<ContentCodes>> GetContentCodeLastest(int exerciseId)
        {
            int userId = _httpContextAccessor.HttpContext.Items["UserId"] == null ? 0 : int.Parse(_httpContextAccessor.HttpContext.Items["UserId"] as string);
            return new ApiResponse<ContentCodes> { IsSuccess = true, Metadata = _mapper.Map<ContentCodes>(await _userExerciseRepository.GetAllQueryAble().Where(e => e.UserId == userId && e.IsSuccess == true && e.ExerciseId == exerciseId).OrderByDescending(e => e.CreatedAt).FirstOrDefaultAsync()) };
        }
        public async Task<ApiResponse<PagedResult<UserExerciseSubmit>>> GetUserSubmission(int exerciseId, int isMine = 1, int pageNumber = 1, int pageSize = 10)
        {
            int userId = _httpContextAccessor.HttpContext.Items["UserId"] == null ? 0 : int.Parse(_httpContextAccessor.HttpContext.Items["UserId"] as string);
            var query = from ue in _userExerciseRepository.GetAllQueryAble()
                        join u in _userRepository.GetAllQueryAble()
                        on ue.UserId equals u.Id
                        select new
                        {
                            Id = u.Id,
                            FullName = u.FirstName + " " + u.LastName,
                            Avatar = u.Avatar,
                            SuccessRate = ue.SuccessRate,
                            IsSuccess = ue.IsSuccess,
                            CreatedAt = ue.CreatedAt
                        };
            if (isMine == 1)
            {
                query = query.Where(e => e.Id == userId);
            }
            return new ApiResponse<PagedResult<UserExerciseSubmit>> { IsSuccess = true, Metadata = HandlePagination<UserExerciseSubmit>.PageList(pageNumber, query.Count(), pageSize, query.OrderByDescending(e => e.CreatedAt).Select(e => new UserExerciseSubmit { Avatar = e.Avatar, FullName = e.FullName, SuccessRate = e.SuccessRate, CreatedAt = e.CreatedAt, IsSuccess = e.IsSuccess }).Skip((pageNumber - 1) * pageSize).Take(pageSize)) };
        }
    }
}

using AutoMapper;
using Data.Entities;
using Dtos.Results;
using Dtos.Results.UserResults;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services.Users
{
    public interface IUserService
    {
        Task<ApiResponse<User>> GetUserInfo();
    }
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public UserService(IBaseRepository<User> userRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper) 
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<ApiResponse<User>> GetUserInfo()
        {
            int currentUserId = _httpContextAccessor.HttpContext.Items["UserId"] == null ? 0 : int.Parse(_httpContextAccessor.HttpContext.Items["UserId"] as string);
            User user = await _userRepository.GetAllQueryAble().Where(e => e.Id == currentUserId).FirstAsync();
            UserInfoDto userInfoDto = _mapper.Map<UserInfoDto>(user);
            /*
            userInfoDto.AchivementsDeserialize = user.Achivements == null ? new List<string>() : JsonSerializer.Deserialize<List<string>>(user.Achivements);*/
            return new ApiResponse<User> { IsSuccess = true , Metadata = user };
        }
    }
}

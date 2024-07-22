using AutoMapper;
using Data.Entities;
using Dtos.Models.UserModels;
using Dtos.Results;
using Dtos.Results.UserResults;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Repositories.Repositories.Base;
using Shared;
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
        Task<ApiResponse<UserInfoDto>> GetUserInfo();
        Task<ApiResponse<bool>> UpdateUserInfo(UserUpdateDto userUpdateDto);
        Task<ApiResponse<bool>> UpdateAchievement(List<string>? achievements);
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
        public async Task<ApiResponse<UserInfoDto>> GetUserInfo()
        {
            int currentUserId = _httpContextAccessor.HttpContext.Items["UserId"] == null ? 0 : int.Parse(_httpContextAccessor.HttpContext.Items["UserId"] as string);
            User user = await _userRepository.GetAllQueryAble().Where(e => e.Id == currentUserId).FirstAsync();
            UserInfoDto userInfoDto = _mapper.Map<UserInfoDto>(user);
            userInfoDto.AchivementsDeserialize = user.Achivements == null ? new List<string>() : System.Text.Json.JsonSerializer.Deserialize<List<string>>(user.Achivements);
            return new ApiResponse<UserInfoDto> { IsSuccess = true , Metadata = userInfoDto };
        }
        public async Task<ApiResponse<bool>> UpdateUserInfo(UserUpdateDto userUpdateDto)
        {
            try
            {
                int currentUserId = _httpContextAccessor.HttpContext.Items["UserId"] == null ? 0 : int.Parse(_httpContextAccessor.HttpContext.Items["UserId"] as string);
                User user = await _userRepository.GetAllQueryAble().Where(e => e.Id == currentUserId).FirstAsync();
                if(userUpdateDto.Avatar != null)
                {
                    if(user.Avatar != null)
                    {
                        await HandleFile.DeleteFile("Uploads", user.Avatar);
                    }
                    user.Avatar = await HandleImage.Upload(userUpdateDto.Avatar);
                }
                user.Class = userUpdateDto.Class;
                user.NickName = userUpdateDto.NickName;
                user.SchoolYear = userUpdateDto.SchoolYear;
                _userRepository.Update(user);
                await _userRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> UpdateAchievement(List<string>? achievements)
        {
            try
            {
                int currentUserId = _httpContextAccessor.HttpContext.Items["UserId"] == null ? 0 : int.Parse(_httpContextAccessor.HttpContext.Items["UserId"] as string);
                User user = await _userRepository.GetAllQueryAble().Where(e => e.Id == currentUserId).FirstAsync();
                user.Achivements = JsonConvert.SerializeObject(achievements);
                _userRepository.Update(user);
                await _userRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

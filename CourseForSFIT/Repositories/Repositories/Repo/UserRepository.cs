using Data.Data;
using Data.Entities;
using Dtos.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.Base;
using Repositories.Repositories.IRepo;

namespace Repositories.Repositories.Repo
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(CourseForSFITContext context) : base(context)
        {

        }
        public async Task<User?> FindUserByEmail(string email)
        {
            try
            {
                return await _context.user.Where(user => user.Email == email).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> UpdateCodeAndTimeSend(VerifyVerificationCodeRequest request)
        {
            try
            {
                User? userEmail = await _context.user.Where(user => user.Email == request.Email).FirstOrDefaultAsync();
                if (userEmail == null)
                {
                    return false;
                }
                userEmail.Code = request.Code;
                userEmail.ExpiredTime = DateTime.UtcNow.AddMinutes(15);
                _context.user.Update(userEmail);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> ChangePassword(ResetPassword resetPassword)
        {
            try
            {
                User? user = await _context.user.Where(user => user.Email == resetPassword.Email).FirstOrDefaultAsync();
                user.Password = BCrypt.Net.BCrypt.HashPassword(resetPassword.Password);
                _context.user.Update(user);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

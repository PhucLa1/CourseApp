using Data.Entities;
using Dtos.Models.AuthModels;
using Repositories.Repositories.Base;


namespace Repositories.Repositories.IRepo
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> FindUserByEmail(string email);
        Task<bool> UpdateCodeAndTimeSend(VerifyVerificationCodeRequest request);
        Task<bool> ChangePassword(ResetPassword resetPassword);
    }
}

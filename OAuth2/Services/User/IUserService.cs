using OAuth2.Models.User.Requests;
using OAuth2.Models.User.Responses;

namespace OAuth2.Services.User
{
    public interface IUserService
    {
        Task<UserReadResponse> CreateUserAsync(UserCreateRequest request);
        string GenerateRandomPassword();
        void CreatePasswordHash(string password,
          out byte[] passwordHash,
          out byte[] passwordSalt);
        bool IsExist(string email, int accountType);
    }
}

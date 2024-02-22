using OAuth2.Models.User.Requests;

namespace OAuth2.Services.User
{
    public interface IUserService
    {
        void CreateUserAsync(UserCreateRequest request);
        string GenerateRandomPassword();
        void CreatePasswordHash(string password,
          out byte[] passwordHash,
          out byte[] passwordSalt);
        bool IsExist(string email, int accountType);
    }
}

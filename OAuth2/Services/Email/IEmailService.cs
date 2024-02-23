
using OAuth2.Models.User;

namespace OAuth2.Services.Email
{
    public interface IEmailService
    {
        string CreateToken(UserModel user);
        Task SendEmailAsync(string to, string subject, string body);
        Task ConfirmPasswordReset(UserModel user, string password);
    }
}

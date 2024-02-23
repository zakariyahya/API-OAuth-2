
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using OAuth2.Models.User;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace OAuth2.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        public async Task SendEmailAsync(string to, string subject, string body)
        {

            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("yahya.z@kiranatama.com");
            mailMessage.Body = body;
            mailMessage.Subject = subject;
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.SubjectEncoding = Encoding.UTF8;
            mailMessage.IsBodyHtml = true;
            mailMessage.To.Add(to);

            var client = new SmtpClient("smtp.gmail.com")
            {
                UseDefaultCredentials = false,
                EnableSsl = true,
                Port = 587,
                Credentials = new System.Net.NetworkCredential("yahya.z@kiranatama.com", "Password#1234")
            };

             client.SendMailAsync(mailMessage);
        }

        public async Task ConfirmPasswordReset(UserModel user, string password)
        {
             SendEmailAsync(user.Email, "New password", $"<p>Your new password is: {password}</p><p>Please change it after login.</p>");
        }

/*        public async Task<MimeMessage> BodyEmail(MimeMessage message, UserModel user, string password)
        {
            string token = CreateToken(user);
            message.From.Add(new MailboxAddress("Your Name", "your-email@example.com")); // Change this

            message.To.Add(new MailboxAddress("", user.Email));
            message.Subject = "API token";
            message.Body = new TextPart(TextFormat.Html) { Text = $@"Your password is: <code>{password}</code><br /> Copy the Token: <code>{token}</code>" };

            return message;
        }
*/
        public string CreateToken(UserModel user)
        {
            List<Claim> claims =
            [
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("Id", user.Id.ToString())
            ];
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddMinutes(10), signingCredentials: cred);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}

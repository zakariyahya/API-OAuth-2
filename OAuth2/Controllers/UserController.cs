using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using OAuth2.Models.User.Requests;
using OAuth2.Models.User.Responses;
using OAuth2.Services.User;
using OAuth2.Data;
using Microsoft.AspNetCore.Authentication.Google;
using static OAuth2.Models.AccountTypeEnum;

namespace OAuth2.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("google-login")]
        [HttpGet]
        public IActionResult GoogleSignup()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [Route("google-response")]
        [HttpGet]
        public async Task<IActionResult> GoogleResponse()
        {
            try
            {
                var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                var username = result.Principal.FindFirst(ClaimTypes.Name).Value;
                var name = result.Principal.FindFirst(ClaimTypes.Surname).Value;
                var email = result.Principal.FindFirst(ClaimTypes.Email).Value;
                var generatePassword = _userService.GenerateRandomPassword();

                byte[] passwordHash, passwordSalt;
                _userService.CreatePasswordHash(generatePassword, out passwordHash, out passwordSalt);

                var accountId = result.Principal.FindFirst(ClaimTypes.NameIdentifier).Value;
                var AccountType = AccountPermissions.Google_Type;

                DateTime utcDateTime = DateTime.Now.ToUniversalTime();

                var created = new UserCreateRequest
                {
                    Name = name,
                    UserName = username,
                    Email = email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    AccountId = accountId,
                    Type = (int)AccountType,
                    Creation_date = utcDateTime,
                };

                var exist = _userService.IsExist(email, (int)AccountType);

                if (!exist)
                {
                    var data = await _userService.CreateUserAsync(created);
                    return Ok(data);
                }
                else
                {
                    return StatusCode(409, "Google Email Already Exists!");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}

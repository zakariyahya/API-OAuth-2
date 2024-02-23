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
using OAuth2.Services.Email;
using OAuth2.Models.User;
using AutoMapper;

namespace OAuth2.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IEmailService emailService, IMapper mapper)
        {
            _userService = userService;
            _emailService = emailService;
            _mapper = mapper;
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
                 var tasks = new List<Task>();
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
                    var user = _mapper.Map<UserModel>(data);
/*
                    var sendEmail = _emailService.ConfirmPasswordReset(user, generatePassword);

                    // Check if sending email is faulted
                    if (sendEmail.IsFaulted)
                    {
                        // Log the error or handle it accordingly
                        Console.WriteLine($"Error sending confirmation email: {sendEmail.Exception?.Message}");
                    }*/

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

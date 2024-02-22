using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OAuth2.Data;
using OAuth2.Models.User;
using OAuth2.Models.User.Requests;
using OAuth2.Models.User.Responses;
using static OAuth2.Models.AccountTypeEnum;
using System.Security.Claims;

namespace OAuth2.Services.User
{
    public class UserService : IUserService
    {
        private readonly OAuthContextClass _context;
        private readonly IMapper _mapper;

        public UserService(OAuthContextClass context, IMapper mapper )
        {
            _context = context;
            _mapper = mapper;
        }

       

        public async Task<UserReadResponse> CreateUserAsync(UserCreateRequest request)
        {
            var model = _mapper.Map<UserModel>(request);
            _context.Add(model);
             SaveChanges();

            var response = _mapper.Map<UserReadResponse>(model);
            return response;
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null)
                throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException(
                    "Value cannot be empty or whitespace only string.",
                    "password"
                );

            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        public string GenerateRandomPassword()
        {
            var options = new PasswordOptions
            {
                RequiredLength = 8,
                RequiredUniqueChars = 4,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = true,
                RequireUppercase = true
            };

            var randomChars = new[] {
                "ABCDEFGHJKLMNOPQRSTUVWXYZ",
                "abcdefghijkmnopqrstuvwxyz",
                "0123456789",
                "!@$?_-"
            };

            var rand = new Random(Environment.TickCount);
            var chars = new List<char>();

            if (options.RequireUppercase)
            {
                chars.Insert(rand.Next(0, chars.Count), randomChars[0][rand.Next(0, randomChars[0].Length)]);
            }

            if (options.RequireLowercase)
            {
                chars.Insert(rand.Next(0, chars.Count), randomChars[1][rand.Next(0, randomChars[1].Length)]);
            }

            if (options.RequireDigit)
            {
                chars.Insert(rand.Next(0, chars.Count), randomChars[2][rand.Next(0, randomChars[2].Length)]);
            }

            if (options.RequireNonAlphanumeric)
            {
                chars.Insert(rand.Next(0, chars.Count), randomChars[3][rand.Next(0, randomChars[3].Length)]);
            }

            for (int i = chars.Count; i < options.RequiredLength || chars.Distinct().Count() < options.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count), rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }

        public bool IsExist(string email, int accountType)
        {
           var exist =  _context.Users.Where(x=> x.Email == email && x.Type == accountType).FirstOrDefault();
            if (exist == null)
            {
                return false;
            }

            return true;
        }
    }
}

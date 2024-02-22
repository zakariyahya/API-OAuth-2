using Newtonsoft.Json;

namespace OAuth2.Models.User.Requests
{
    public class UserCreateRequest
    {
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public int GroupId { get; set; }
        public int Type { get; set; }
        public string? AccountId { get; set; }
        public DateTime Creation_date { get; set; } = DateTime.Now;
    }
}

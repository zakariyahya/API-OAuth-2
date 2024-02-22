using Newtonsoft.Json;
using OAuth2.Models.Base;

namespace OAuth2.Models.User
{
    public class UserModel : BaseModel
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("userName")]
        public string? UserName { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("passwordHash")]
        public byte[]? PasswordHash { get; set; }

        [JsonProperty("passwordSalt")]
        public byte[]? PasswordSalt { get; set; }

        [JsonProperty("groupId")]
        public int GroupId { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("accountId")]
        public string? AccountId { get; set; }

        [JsonProperty("creationDate")]
        public DateTime Creation_date { get; set; } = DateTime.Now;
    }
}

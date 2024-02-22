namespace OAuth2.Models.User.Responses
{
    public class UserReadResponse
    {
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public int GroupId { get; set; }
        public int Type { get; set; }
        public string? AccountId { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}

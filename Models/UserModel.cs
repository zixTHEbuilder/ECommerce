namespace ECommerce.Models
{
    public class UserModel
    {
        public int id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string HashedPassword { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public int Balance { get; set; }
        public string Role { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiry {get;set;}
    }
}

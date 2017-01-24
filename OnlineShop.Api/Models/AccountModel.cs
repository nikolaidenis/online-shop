namespace OnlineShop.Api.Models
{
    public class AccountModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool Blocked { get; set; }
        public string Role { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
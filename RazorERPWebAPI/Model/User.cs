namespace RazorERPWebAPI.Model
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public required string Role { get; set; }  // Admin or User
        public int CompanyId { get; set; }
        public bool IsAdmin { get; set; }
    }
}

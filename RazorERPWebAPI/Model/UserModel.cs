namespace RazorERPWebAPI.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public int CompanyId { get; set; }
        public string Roles { get; set; }
    }

    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

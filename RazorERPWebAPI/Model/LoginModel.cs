namespace RazorERPWebAPI.Model
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        // Add this property to pass the company ID
        public int CompanyId { get; set; } 

    }
}

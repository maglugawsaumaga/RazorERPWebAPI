using BCrypt.Net;

namespace RazorERPWebAPI.Services
{
    public class PasswordHasherService
    {
        public string HashPassword(string password)
        {
            // Generate a salted hash for the password
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string providedPassword, string hashedPassword)
        {
            // Compare provided password to stored hash
            return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
        }
    }
}

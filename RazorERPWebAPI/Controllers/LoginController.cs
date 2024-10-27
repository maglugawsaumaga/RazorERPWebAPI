using Microsoft.AspNetCore.Mvc;
using RazorERPWebAPI.Model;
using RazorERPWebAPI.Repository;
using RazorERPWebAPI.Services;
using System.Security.Cryptography;
using System.Text;

namespace RazorERPWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly AuthService _authService;
        private readonly PasswordHasherService _passwordHasherService;

        public LoginController(UserRepository userRepository, AuthService authService, PasswordHasherService passwordHasherService)
        {
            _userRepository = userRepository;
            _authService = authService;
            _passwordHasherService = passwordHasherService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            // Check if the user exists for the provided username and company ID
            var user = await _userRepository.GetUserByUsername(model.Username, model.CompanyId);
            if (user == null || !_passwordHasherService.VerifyPassword(model.Password, user.PasswordHash))
                return Unauthorized("Invalid Credentials");

            var token = _authService.GenerateToken(user);
            return Ok(new { Token = token });
        }
    }

}

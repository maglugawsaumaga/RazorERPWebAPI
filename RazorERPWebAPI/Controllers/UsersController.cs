using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RazorERPWebAPI.Model;
using RazorERPWebAPI.Repository;
using RazorERPWebAPI.Services;

namespace RazorERPWebAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly AuthService _authService;
        private readonly PasswordHasherService _passwordHasherService;

        public UsersController(UserRepository userRepository, AuthService authService, PasswordHasherService passwordHasherService)
        {
            _userRepository = userRepository;
            _authService = authService;
            _passwordHasherService = passwordHasherService;
        }
        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var companyId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "CompanyId")?.Value);
            var isAdmin = User.IsInRole("Admin");
            //var companyId = 1;
            //var isAdmin = false;
            var users = await _userRepository.GetUsersByCompany(companyId, isAdmin);
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            user.PasswordHash = _passwordHasherService.HashPassword(user.PasswordHash);
            await _userRepository.CreateUser(user);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            user.Id = id;
            await _userRepository.UpdateUser(user);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var companyId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "CompanyId")?.Value);
            await _userRepository.DeleteUser(id, companyId);
            return Ok();
        }
    }

}

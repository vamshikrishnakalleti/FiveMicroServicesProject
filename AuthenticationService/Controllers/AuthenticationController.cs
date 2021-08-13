using AuthenticationService.Database;
using AuthenticationService.Models;
using AuthenticationService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        IAuthenticationService _authService;
        public AuthenticationController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public async Task<IEnumerable<UserModel>> GetUsers()
        {
            return await _authService.GetUsers();
        }

        [HttpPost]
        public async Task<IActionResult> AuthenticateUser(LoginModel model)
        {
            var data = await _authService.AuthenticateUser(model);
            if (data != null)
                return Ok(data);
            else
                return NotFound("user not found!");
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserSignUpModel model)
        {
            // To DO:
            User user = new User
            {
                Name = model.Name,
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };
            bool result = await _authService.CreateUser(user, model.Password);
            if (result)
            {
                return StatusCode(StatusCodes.Status201Created);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }
    }
}

using AuthenticationService.Database;
using AuthenticationService.Models;
using AuthenticationService.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        protected SignInManager<User> _signInManager;
        protected UserManager<User> _userManager;
        //protected RoleManager<Role> _roleManager;
        protected IConfiguration _config;
        public AuthenticationService(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<Role> roleManager, IConfiguration config)
        {
            // _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }
        private string GenerateJsonToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                             new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                             new Claim(JwtRegisteredClaimNames.Email, user.Email),
                             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                             new Claim("Roles", string.Join(",",user.Roles))
                             };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], claims, expires: DateTime.Now.AddMinutes(20), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<UserModel> AuthenticateUser(LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                var roles = await _userManager.GetRolesAsync(user);

                UserModel userModel = new UserModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Roles = roles.ToArray()
                };
                userModel.Token = GenerateJsonToken(userModel);
                return userModel;
            }
            return null;
        }

        public async Task<bool> CreateUser(User model, string Password)
        {
            var result = await _userManager.CreateAsync(model, Password);
            if (result.Succeeded)
            {
                //Admin, User
                string role = "User";
                var res = await _userManager.AddToRoleAsync(model, role);
                return res.Succeeded;
            }
            return false;
        }

        public async Task<bool> SignOut()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<IEnumerable<UserModel>> GetUsers()
        {
            //var users= await _userManager.Users.ToListAsync();
            //IList<UserModel> model = new List<UserModel>();
            //foreach (var user in users)
            //{
            //    UserModel userModel = new UserModel
            //    {
            //        Id = user.Id,
            //        Name = user.Name,
            //        Email = user.Email,
            //        PhoneNumber = user.PhoneNumber
            //    };
            //    model.Add(userModel);
            //}
            //return model;

            var users = await _userManager.Users.Select(user => new UserModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            }).ToListAsync();
            return users;
        }
    }
}

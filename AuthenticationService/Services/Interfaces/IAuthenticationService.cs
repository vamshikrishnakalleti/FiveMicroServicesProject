using AuthenticationService.Database;
using AuthenticationService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthenticationService.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<IEnumerable<UserModel>> GetUsers();
        Task<UserModel> AuthenticateUser(LoginModel model);
        Task<bool> CreateUser(User model, string Password);
        Task<bool> SignOut();
    }
}

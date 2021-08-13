using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class AccountController : Controller
    {
        HttpClient _client;
        IConfiguration _config;
        Uri _baseAddress;
        public AccountController(IConfiguration config)
        {
            _config = config;
            _client = new HttpClient();

            _baseAddress = new Uri(_config["ApiAddress"]);
            _client.BaseAddress = _baseAddress;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            string strData = JsonSerializer.Serialize(model);
            StringContent content = new StringContent(strData, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(_client.BaseAddress + "authentication/AuthenticateUser", content);
            if (response.IsSuccessStatusCode)
            {
                var strUser = await response.Content.ReadAsStringAsync();
                UserModel user = JsonSerializer.Deserialize<UserModel>(strUser);
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Now.AddMinutes(20);
                HttpContext.Response.Cookies.Append("token", user.Token);

                return RedirectToAction("Index", "Dashboard", new { area = "User" });
            }
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserSignUpModel model)
        {
            string strData = JsonSerializer.Serialize(model);
            StringContent content = new StringContent(strData, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(_client.BaseAddress + "/authentication/createuser", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
    }
}

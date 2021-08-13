using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        IConfiguration _config;
        HttpClient _client;
        Uri _uri;
        public HomeController(IConfiguration config)
        {
            _config = config;
            _uri = new Uri(_config["ApiAddress"]);
            _client = new HttpClient();
            _client.BaseAddress = _uri;
        }

        public IActionResult Index()
        {
            try
            {
                int x = 0, y = 4;
                int z = y / x;
            }
            catch (Exception ex)
            {
                var data = JsonConvert.SerializeObject(ex);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                _ = _client.PostAsync(_client.BaseAddress + "log", content).Result;
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

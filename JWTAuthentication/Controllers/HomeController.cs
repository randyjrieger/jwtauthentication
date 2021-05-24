using JWTAuthentication.Models;
using JWTAuthentication.Repository;
using JWTAuthentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace JWTAuthentication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;

        private readonly ILogger<HomeController> _logger;

        public HomeController(IConfiguration config,
            ITokenService tokenService,
            IUserRepository userRepository,
            ILogger<HomeController> logger)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _config = config;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public IActionResult Login(UserModel userModel)
        {
            if (string.IsNullOrEmpty(userModel.UserName) ||
                string.IsNullOrEmpty(userModel.Password))
            {
                return (RedirectToAction("Error"));
            }

            IActionResult response = Unauthorized();
            var validUser = GetUser(userModel);

            if (validUser != null)
            {
                var generatedToken = _tokenService.BuildToken(_config["Jwt:Key"].ToString(),
                    _config["Jwt:Issuer"].ToString(),
                    validUser);
                if (generatedToken != null)
                {
                    HttpContext.Session.SetString("Token", generatedToken);
                    return RedirectToAction("MainWindow");
                }
                else
                {
                    return RedirectToAction("Error");
                }
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        private UserDTO GetUser(UserModel userModel)
        {
            return _userRepository.GetUser(userModel);
        }

        [Route("mainwindow")]
        [HttpGet]
        public IActionResult MainWindow()
        {
            string token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return RedirectToAction("Index");
            }

            if (!_tokenService.IsTokenValid(
                _config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), token))
            {
                return RedirectToAction("Index");
            }

            ViewBag.Message = BuildMessage(token, 50);
            return View();

        }

        public IActionResult Error()
        {
            ViewBag.Message = "AUthentication Error...";
            return View();
        }

        private string BuildMessage(string stringToSplit, int chuckSize)
        {
            var data = Enumerable.Range(0, stringToSplit.Length / chuckSize)
                .Select(i => stringToSplit.Substring(i * chuckSize, chuckSize));
            string result = "The generated token is: ";

            foreach(string str in data)
            {
                result += Environment.NewLine + str;
            }

            return result;
        }

    }
}

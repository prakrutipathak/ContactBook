using ContactBook.Services.Contract;
using ContactBook.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ContactBook.Controllers
{
    public class AuthController : Controller
    {
       
        private readonly IAuthService _authservice;
        public AuthController(IAuthService authService)
        {
            _authservice = authService;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel register)
        {
            if (ModelState.IsValid)
            {
                //Password Strength
                var message = _authservice.registerUserService(register);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    TempData["ErrorMessage"] = message;
                    return View(register);
                }

                return RedirectToAction("RegisterSuccess");

            }
            return View(register);
        }

        public IActionResult RegisterSuccess()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                var message = _authservice.LoginUserService(login);
                if (message == "Invalid username or password")
                {
                    TempData["ErrorMessage"] = message;
                }
                else if (message == "Something went wrong,please try after sometime")
                {
                    TempData["ErrorMessage"] = message;
                    return View(login);
                }
                else
                {
                    string token = message;
                    Response.Cookies.Append("jwtToken", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                    });
                    return RedirectToAction("Index", "Contact");
                }

            }
            return View(login);
        }
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwtToken");
            return RedirectToAction("Index", "Home");
        }

    }
}

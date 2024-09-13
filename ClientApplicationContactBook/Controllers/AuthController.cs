using ClientApplicationContactBook.Implementation;
using ClientApplicationContactBook.Infrastructure;
using ClientApplicationContactBook.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;

namespace ClientApplicationContactBook.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IConfiguration _configuration;
        private readonly IJwtTokenHandler _tokenHandler;
        private string endPoint;
        public AuthController(IHttpClientService httpClientService, IConfiguration configuration, IJwtTokenHandler tokenHandler)
        {
            _httpClientService = httpClientService;
            _configuration = configuration;
            _tokenHandler = tokenHandler;
            endPoint = _configuration["EndPoint:CivicaApi"];
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
                var apiUrl = $"{endPoint}Auth/Register";
                var response = _httpClientService.PostHttpResponseMessage(apiUrl, register, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(data);
                    TempData["SuccessMessage"] = serviceResponse.Message;
                    return RedirectToAction("RegisterSuccess");

                }
                else
                {
                    string errorData = response.Content.ReadAsStringAsync().Result;
                    var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorData);
                    if (errorResponse != null)
                    {
                        TempData["ErrorMessage"] = errorResponse.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Something went wrong.Please try after sometime.";
                    }
                    return RedirectToAction("Register");
                }

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
                var apiUrl = $"{endPoint}Auth/Login";
                var response = _httpClientService.PostHttpResponseMessage(apiUrl, login, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);

                    string token = serviceResponse.Data;

                    Response.Cookies.Append("jwtToken", token, new CookieOptions
                    {
                        HttpOnly = false,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = DateTime.UtcNow.AddDays(1),
                    });

                    //var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = _tokenHandler.ReadJwtToken(token);
                    var userId = jwtToken.Claims.First(claim => claim.Type == "UserId").Value;

                    
                    Response.Cookies.Append("userid", userId, new CookieOptions
                    {
                        HttpOnly = false,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = DateTime.UtcNow.AddDays(1),
                    });
                    var id = Convert.ToInt32(userId);
                    //Get user details
                    var userDetails = UserDetailById(id);

                    //// Store user image in cookie
                    if (userDetails != null && userDetails.ImageByte != null)
                    {
                        var image = Convert.ToBase64String(userDetails.ImageByte);

                        // Split image into smaller chunks if necessary to fit cookie size limit
                        int chunkSize = 3800; // safe size under 4KB considering other cookie data
                        int totalChunks = (image.Length + chunkSize - 1) / chunkSize;

                        for (int i = 0; i < totalChunks; i++)
                        {
                            string chunk = image.Substring(i * chunkSize, Math.Min(chunkSize, image.Length - i * chunkSize));
                            Response.Cookies.Append($"image_chunk_{i}", chunk, new CookieOptions
                            {
                                HttpOnly = false,
                                Secure = true,
                                SameSite = SameSiteMode.None,
                                Expires = DateTime.UtcNow.AddDays(1),
                            });
                        }
                    }
                    TempData["SuccessMessage"] = serviceResponse.Message;
                    return RedirectToAction("Index1", "Contact");
                }
                else
                {
                    string errorData = response.Content.ReadAsStringAsync().Result;
                    var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorData);
                    if (errorResponse != null)
                    {
                        TempData["ErrorMessage"] = errorResponse.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Something went wrong.Please try after sometime.";
                    }
                    return RedirectToAction("Login");
                }


            }
            return View(login);
        }
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwtToken");
            Response.Cookies.Delete("userid");
            int i = 0;
            while (Request.Cookies.ContainsKey($"image_chunk_{i}"))
            {
                Response.Cookies.Delete($"image_chunk_{i}");
                i++;
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgotPassword(ForgetViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var apiUrl = $"{endPoint}Auth/ForgetPassword";
                var response = _httpClientService.PostHttpResponseMessage(apiUrl, viewModel, HttpContext.Request); // Blocking call

                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result; // Blocking call
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);
                    TempData["SuccessMessage"] = serviceResponse.Message;
                    return RedirectToAction("ForgotPasswordConfirmation");
                }
                else
                {
                    string errorResponse = response.Content.ReadAsStringAsync().Result; // Blocking call
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorResponse);

                    if (serviceResponse != null)
                    {
                        TempData["ErrorMessage"] = serviceResponse.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Something went wrong. Please try again later.";
                    }
                    return RedirectToAction("ForgotPassword");
                }
            }
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        [HttpGet]
        [ExcludeFromCodeCoverage]
        [Authorize]
        public virtual UpdateUserViewModel UserDetailById(int id)
        {

            var apiUrl = $"{endPoint}Auth/GetUserById/" + id;
            var response = _httpClientService.GetHttpResponseMessage<UpdateUserViewModel>(apiUrl, HttpContext.Request);
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<UpdateUserViewModel>>(data);
                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    return serviceResponse.Data;
                }
                else
                {
                    throw new Exception(serviceResponse.Message);

                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<UpdateUserViewModel>>(errorData);
                if (errorResponse != null)
                {
                    throw new Exception(errorResponse.Message);
                }
                else
                {
                    throw new Exception("Something went wrong. Please try after some time.");
                }
            }

        }

        [Authorize]
        [HttpGet]
        public IActionResult EditUser(int id)
        {
            var apiUrl = $"{endPoint}Auth/GetUserById/" + id;
            var response = _httpClientService.GetHttpResponseMessage<UpdateUserViewModel>(apiUrl, HttpContext.Request);
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<UpdateUserViewModel>>(data);
                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    UpdateUserViewModel viewModel = serviceResponse.Data;
                    return View(viewModel);
                }
                else
                {
                    TempData["ErrorMessage"] = serviceResponse.Message;
                    return RedirectToAction("Register");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<UpdateUserViewModel>>(errorData);
                if (errorResponse != null)
                {
                    TempData["ErrorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong.Please try after sometime.";
                }
                return RedirectToAction("Register");
            }
        }
        [Authorize]
        [HttpPost]
        public IActionResult EditUser(UpdateUserViewModel updateContact)
        {
            if (ModelState.IsValid)
            {
                  if (updateContact.File != null && updateContact.File.Length > 0)
                    
                {
                    if (updateContact.File.Length > 10240) // 10KB in bytes
                    {
                        TempData["ErrorMessage"] = "Image size should not be greater than 10KB.";
                        return View(updateContact);
                    }
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" }; // Add other allowed extensions as needed
                    var fileExtension = Path.GetExtension(updateContact.File.FileName).ToLower();
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        TempData["ErrorMessage"] = "Only JPG, JPEG, and PNG file extensions are allowed.";
                        return View(updateContact);
                    }
                    using (var memoryStream = new MemoryStream())
                    {
                        updateContact.File.CopyTo(memoryStream);
                        updateContact.ImageByte = memoryStream.ToArray();
                    }
                    updateContact.Image = updateContact.File.FileName;
                }
                else if (updateContact.RemoveImageHidden == "true")
                {
                    // User wants to remove the current image
                    updateContact.Image = null;
                    updateContact.ImageByte = null;
                }
                else
                {
                    // Use the previous file name if no new file is provided
                    updateContact.Image = updateContact.Image;
                }

                var apiUrl = $"{endPoint}Auth/Edit";
                HttpResponseMessage response = _httpClientService.PutHttpResponseMessage(apiUrl, updateContact, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);
                    TempData["SuccessMessage"] = serviceResponse.Message;
                    return RedirectToAction("Logout");
                }
                else
                {
                    string errorData = response.Content.ReadAsStringAsync().Result;
                    var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorData);
                    if (errorResponse != null)
                    {
                        TempData["ErrorMessage"] = errorResponse.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Something went wrong try after some time";
                        return RedirectToAction("Index1","Contact");
                    }
                }
            }
            return View(updateContact);
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            ForgetViewModel viewModel=new ForgetViewModel();
            viewModel.UserName = @User.Identity.Name;
            return View(viewModel);
        }
        [HttpPost]
        [Authorize]
        public IActionResult ChangePassword(ForgetViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var apiUrl = $"{endPoint}Auth/ForgetPassword";
                var response = _httpClientService.PostHttpResponseMessage(apiUrl, viewModel, HttpContext.Request); // Blocking call

                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result; // Blocking call
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);
                    TempData["SuccessMessage"] = serviceResponse.Message;
                    return RedirectToAction("Logout");
                }
                else
                {
                    string errorResponse = response.Content.ReadAsStringAsync().Result; // Blocking call
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorResponse);

                    if (serviceResponse != null)
                    {
                        TempData["ErrorMessage"] = serviceResponse.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Something went wrong. Please try again later.";
                    }
                    return RedirectToAction("ChangePassword");
                }
            }
            return View(viewModel);
        }

    }
}

using APIContactBook.Dtos;
using APIContactBook.Models;
using APIContactBook.Services.Contract;
using APIContactBook.Services.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIContactBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authservice;
        public AuthController(IAuthService authService)
        {

            _authservice = authService;
        }
        [HttpPost("Register")]
        public IActionResult Register(RegisterDto register)
        {
            var response = _authservice.registerUserService(register);
            return !response.Success ? BadRequest(response) : Ok(response);
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginDto login)
        {
            var response = _authservice.LoginUserService(login);
            return !response.Success ? BadRequest(response) : Ok(response);
        }
        [HttpPost("ForgetPassword")]
        public IActionResult ForgetPassword(ForgetDto forgetDto)
        {
            var response = _authservice.ForgetPasswordService(forgetDto);
            return !response.Success ? BadRequest(response) : Ok(response);
        }
        [HttpGet("GetUserById/{id}")]
        public IActionResult GetUserById(int id)
        {
            var response = _authservice.GetUser(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpPut("Edit")]
        public IActionResult Edit(GetUserDto userDto)
        {
            if (ModelState.IsValid)
            {
                var user = new User()
                {
                   UserId= userDto.UserId,
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Email = userDto.Email,
                    Image = userDto.Image,
                    ImageByte = userDto.ImageByte,
                    LoginId = userDto.LoginId,
                    ContactNumber = userDto.ContactNumber,
                };
                var result = _authservice.ModifyUser(user);
                return !result.Success ? BadRequest(result) : Ok(result);
            }
            else
            {
                return BadRequest();
            }

        }
    }
}


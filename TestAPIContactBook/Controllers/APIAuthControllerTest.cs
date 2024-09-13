using APIContactBook.Controllers;
using APIContactBook.Dtos;
using APIContactBook.Models;
using APIContactBook.Services.Contract;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestAPIContactBook.Controllers
{
    public class APIAuthControllerTest
    {
        [Theory]
        [InlineData("User already exists.")]
        [InlineData("Something went wrong, please try after sometime.")]
        [InlineData("Mininum password length should be 8")]
        [InlineData("Password should be apphanumeric")]
        [InlineData("Password should contain special characters")]
        public void Register_ReturnsBadRequest_WhenRegistrationFails(string message)
        {
            // Arrange
            var registerDto = new RegisterDto();
            var mockAuthService = new Mock<IAuthService>();
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = message

            };
            mockAuthService.Setup(service => service.registerUserService(registerDto))
                           .Returns(expectedServiceResponse);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.Register(registerDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(message, ((ServiceResponse<string>)actual.Value).Message);
            Assert.False(((ServiceResponse<string>)actual.Value).Success);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(badRequestResult.Value);
            Assert.False(((ServiceResponse<string>)badRequestResult.Value).Success);
            mockAuthService.Verify(service => service.registerUserService(registerDto), Times.Once);
        }
        [Fact]
        public void Register_ReturnsOk_WhenRegistrationSuccess()
        {
            // Arrange
            var registerDto = new RegisterDto();
            var mockAuthService = new Mock<IAuthService>();
            var message = "User Added Successfully";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = message

            };
            mockAuthService.Setup(service => service.registerUserService(registerDto))
                           .Returns(expectedServiceResponse);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.Register(registerDto) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(message, ((ServiceResponse<string>)actual.Value).Message);
            Assert.True(((ServiceResponse<string>)actual.Value).Success);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            var okResult = Assert.IsType<OkObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(okResult.Value);
            Assert.True(((ServiceResponse<string>)okResult.Value).Success);
            mockAuthService.Verify(service => service.registerUserService(registerDto), Times.Once);
        }
        [Theory]
        [InlineData("Invalid username or password!")]
        [InlineData("Something went wrong, please try after sometime.")]
        public void Login_ReturnsBadRequest_WhenLoginFails(string message)
        {
            // Arrange
            var loginDto = new LoginDto();
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = message

            };
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.LoginUserService(loginDto))
                           .Returns(expectedServiceResponse);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.Login(loginDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(message, ((ServiceResponse<string>)actual.Value).Message);
            Assert.False(((ServiceResponse<string>)actual.Value).Success);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(badRequestResult.Value);
            Assert.False(((ServiceResponse<string>)badRequestResult.Value).Success);
            mockAuthService.Verify(service => service.LoginUserService(loginDto), Times.Once);
        }
        [Fact]
        public void Login_ReturnsOk_WhenLoginSucceeds()
        {
            // Arrange
            var loginDto = new LoginDto { Username = "username", Password = "password" };
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = string.Empty

            };
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.LoginUserService(loginDto))
                           .Returns(expectedServiceResponse);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.Login(loginDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(string.Empty, ((ServiceResponse<string>)actual.Value).Message);
            Assert.True(((ServiceResponse<string>)actual.Value).Success);
            var okResult = Assert.IsType<OkObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(okResult.Value);
            Assert.True(((ServiceResponse<string>)okResult.Value).Success);
            mockAuthService.Verify(service => service.LoginUserService(loginDto), Times.Once);
        }

        [Theory]
        [InlineData("Mininum password length should be 8")]
        [InlineData("Password should be apphanumeric")]
        [InlineData("Password should contain special characters")]
        [InlineData("Invalid username!")]
        [InlineData("Password and confirmation password do not match!")]
        [InlineData("Something went wrong, please try again later.")]
        public void ForgetPassword_ReturnsBadRequest_WhenForgetPasswordFails(string message)
        {
            // Arrange
            var forgetDto = new ForgetDto();
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = message

            };
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.ForgetPasswordService(forgetDto))
                           .Returns(expectedServiceResponse);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.ForgetPassword(forgetDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(message, ((ServiceResponse<string>)actual.Value).Message);
            Assert.False(((ServiceResponse<string>)actual.Value).Success);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(badRequestResult.Value);
            Assert.False(((ServiceResponse<string>)badRequestResult.Value).Success);
            mockAuthService.Verify(service => service.ForgetPasswordService(forgetDto), Times.Once);
        }

        [Fact]
        public void ForgetPassword_ReturnsOk_WhenForgetPasswordSucceeds()
        {
            // Arrange
            var fixture = new Fixture();
            var forgetDto = new ForgetDto()
            {
                UserName = "username",
                Password = "Password@1234",
                ConfirmPassword = "Password@1234"
            };
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = ""

            };
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.ForgetPasswordService(forgetDto))
                           .Returns(expectedServiceResponse);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.ForgetPassword(forgetDto) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(string.Empty, ((ServiceResponse<string>)actual.Value).Message);
            Assert.True(((ServiceResponse<string>)actual.Value).Success);
            var okResult = Assert.IsType<OkObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(okResult.Value);
            Assert.True(((ServiceResponse<string>)okResult.Value).Success);
            mockAuthService.Verify(service => service.ForgetPasswordService(forgetDto), Times.Once);
        }
        [Fact]
        public void GetUserById_ReturnsOk()
        {

            var userId = 1;
            var user = new User
            {
                UserId = userId,
                FirstName = "User 1"
            };

            var response = new ServiceResponse<GetUserDto>
            {
                Success = true,
                Data = new GetUserDto
                {
                    UserId = userId,
                    FirstName = user.FirstName
                }
            };

            var mockUserService = new Mock<IAuthService>();
            var target = new AuthController(mockUserService.Object);
            mockUserService.Setup(c => c.GetUser(userId)).Returns(response);

            //Act
            var actual = target.GetUserById(userId) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockUserService.Verify(c => c.GetUser(userId), Times.Once);
        }

        [Fact]
        public void GetUserById_ReturnsNotFound()
        {

            var userId = 1;
            var user = new User
            {
                UserId = userId,
                FirstName = "User 1"
            };

            var response = new ServiceResponse<GetUserDto>
            {
                Success = false,
                Data = new GetUserDto
                {
                    UserId = userId,
                    FirstName = user.FirstName
                }
            };

            var mockUserService = new Mock<IAuthService>();
            var target = new AuthController(mockUserService.Object);
            mockUserService.Setup(c => c.GetUser(userId)).Returns(response);

            //Act
            var actual = target.GetUserById(userId) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockUserService.Verify(c => c.GetUser(userId), Times.Once);
        }
        [Fact]
        public void Edit_ReturnsBadRequest_WhenModelIsInValid()
        {
            var fixture = new Fixture();
            var updateUserDto = fixture.Create<GetUserDto>();
            var mockUserService = new Mock<IAuthService>();
            var target = new AuthController(mockUserService.Object);
            target.ModelState.AddModelError("ContactNumber", "ContactNumber is required");
            //Act

            var actual = target.Edit(updateUserDto) as BadRequestResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.False(target.ModelState.IsValid);
        }


        [Fact]
        public void Edit_ReturnsOk_WhenUserIsUpdatesSuccessfully()
        {
            var fixture = new Fixture();
            var updateUserDto = fixture.Create<GetUserDto>();
            var response = new ServiceResponse<string>
            {
                Success = true,
            };
            var mockUserService = new Mock<IAuthService>();
            var target = new AuthController(mockUserService.Object);
            mockUserService.Setup(c => c.ModifyUser(It.IsAny<User>())).Returns(response);

            //Act

            var actual = target.Edit(updateUserDto) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockUserService.Verify(c => c.ModifyUser(It.IsAny<User>()), Times.Once);

        }

        [Fact]
        public void Edit_ReturnsBadRequest_WhenUserIsNotUpdated()
        {
            var fixture = new Fixture();
            var updateUserDto = fixture.Create<GetUserDto>();
            var response = new ServiceResponse<string>
            {
                Success = false,
            };
            var mockUserService = new Mock<IAuthService>();
            var target = new AuthController(mockUserService.Object);
            mockUserService.Setup(c => c.ModifyUser(It.IsAny<User>())).Returns(response);

            //Act

            var actual = target.Edit(updateUserDto) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockUserService.Verify(c => c.ModifyUser(It.IsAny<User>()), Times.Once);

        }
    }
}

using APIContactBook.Data.Contract;
using APIContactBook.Dtos;
using APIContactBook.Models;
using APIContactBook.Services.Contract;
using APIContactBook.Services.Implementation;
using AutoFixture;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAPIContactBook.Services
{
    public class APIAuthServiceTests
    {
        [Fact]
        public void RegisterUserService_ReturnsSuccess_WhenValidRegistration()
        {
            // Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockVerifyPasswordHash = new Mock<IVerifyPasswordHash>();
            mockAuthRepository.Setup(repo => repo.UserExist(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            mockAuthRepository.Setup(repo => repo.RegisterUser(It.IsAny<User>())).Returns(true);


            var target = new AuthService(mockAuthRepository.Object, mockVerifyPasswordHash.Object);

            var registerDto = new RegisterDto
            {
                FirstName = "firstname",
                LastName = "lastname",
                Email = "email@example.com",
                LoginId = "loginid",
                ContactNumber = "1234567890",
                Password = "Password@123"
            };

            // Act
            var result = target.registerUserService(registerDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(string.Empty, result.Message);
            mockAuthRepository.Verify(c => c.UserExist(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockAuthRepository.Verify(c => c.RegisterUser(It.IsAny<User>()), Times.Once);
        }
        [Fact]
        public void RegisterUserService_ReturnsFailure_WhenPasswordIsWeak()
        {
            // Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockVerifyPasswordHash = new Mock<IVerifyPasswordHash>();
            var target = new AuthService(mockAuthRepository.Object, mockVerifyPasswordHash.Object);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Mininum password length should be 8" + Environment.NewLine);
            stringBuilder.Append("Password should be alphanumeric" + Environment.NewLine);
            stringBuilder.Append("Password should contain special characters" + Environment.NewLine);
            var registerDto = new RegisterDto
            {
                FirstName = "firstname",
                LastName = "lastname",
                Email = "email@example.com",
                LoginId = "loginid",
                ContactNumber = "1234567890",
                Password = "pass"
            };

            // Act
            var result = target.registerUserService(registerDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(stringBuilder.ToString(), result.Message);
        }
        [Fact]
        public void RegisterUserService_ReturnsUserExists()
        {
            // Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockVerifyPasswordHash = new Mock<IVerifyPasswordHash>();
            mockAuthRepository.Setup(repo => repo.UserExist(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            var target = new AuthService(mockAuthRepository.Object, mockVerifyPasswordHash.Object);

            var registerDto = new RegisterDto
            {
                FirstName = "firstname",
                LastName = "lastname",
                Email = "email@example.com",
                LoginId = "loginid",
                ContactNumber = "1234567890",
                Password = "Password@123"
            };

            // Act
            var result = target.registerUserService(registerDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("User already exists.", result.Message);
            mockAuthRepository.Verify(c => c.UserExist(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
        [Fact]
        public void RegisterUserService_ReturnsSomeThingWentWrong_WhenInValidRegistration()
        {
            // Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockVerifyPasswordHash = new Mock<IVerifyPasswordHash>();
            mockAuthRepository.Setup(repo => repo.UserExist(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            mockAuthRepository.Setup(repo => repo.RegisterUser(It.IsAny<User>())).Returns(false);


            var target = new AuthService(mockAuthRepository.Object, mockVerifyPasswordHash.Object);

            var registerDto = new RegisterDto
            {
                FirstName = "firstname",
                LastName = "lastname",
                Email = "email@example.com",
                LoginId = "loginid",
                ContactNumber = "1234567890",
                Password = "Password@123"
            };

            // Act
            var result = target.registerUserService(registerDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Something went wrong, please try after sometime.", result.Message);
            mockAuthRepository.Verify(c => c.UserExist(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockAuthRepository.Verify(c => c.RegisterUser(It.IsAny<User>()), Times.Once);
        }
        [Fact]
        public void LoginUserService_ReturnsSomethingWentWrong_WhenLoginDtoIsNull()
        {
            //Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IVerifyPasswordHash>();

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);


            // Act
            var result = target.LoginUserService(null);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Something went wrong,please try after sometime", result.Message);

        }
        [Fact]
        public void LoginUserService_ReturnsInvalidUsernameOrPassword_WhenUserIsNull()
        {
            //Arrange
            var loginDto = new LoginDto
            {
                Username = "username"
            };
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IVerifyPasswordHash>();
            mockAuthRepository.Setup(repo => repo.ValidateUser(loginDto.Username)).Returns<User>(null);

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);


            // Act
            var result = target.LoginUserService(loginDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid username or password", result.Message);
            mockAuthRepository.Verify(repo => repo.ValidateUser(loginDto.Username), Times.Once);


        }
        [Fact]
        public void LoginUserService_ReturnsInvalidUsernameOrPassword_WhenPasswordIsWrong()
        {
            //Arrange
            var loginDto = new LoginDto
            {
                Username = "username",
                Password = "password"
            };
            var fixture = new Fixture();
            var user = fixture.Create<User>();
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IVerifyPasswordHash>();
            mockAuthRepository.Setup(repo => repo.ValidateUser(loginDto.Username)).Returns(user);
            mockConfiguration.Setup(repo => repo.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt)).Returns(false);

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);


            // Act
            var result = target.LoginUserService(loginDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid username or password", result.Message);
            mockAuthRepository.Verify(repo => repo.ValidateUser(loginDto.Username), Times.Once);
            mockConfiguration.Verify(repo => repo.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt), Times.Once);


        }

        [Fact]
        public void LoginUserService_ReturnsResponse_WhenLoginIsSuccessful()
        {
            //Arrange
            var loginDto = new LoginDto
            {
                Username = "username",
                Password = "password"
            };
            var fixture = new Fixture();
            var user = fixture.Create<User>();
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IVerifyPasswordHash>();
            mockAuthRepository.Setup(repo => repo.ValidateUser(loginDto.Username)).Returns(user);
            mockConfiguration.Setup(repo => repo.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt)).Returns(true);
            mockConfiguration.Setup(repo => repo.CreateToken(user)).Returns("");

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);


            // Act
            var result = target.LoginUserService(loginDto);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            mockAuthRepository.Verify(repo => repo.ValidateUser(loginDto.Username), Times.Once);
            mockConfiguration.Verify(repo => repo.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt), Times.Once);
            mockConfiguration.Verify(repo => repo.CreateToken(user), Times.Once);


        }

        [Fact]
        public void ForgetPasswordService_ReturnsSomethingWentWrong_WhenForgetDtoIsNull()
        {
            //Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IVerifyPasswordHash>();

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);


            // Act
            var result = target.ForgetPasswordService(null);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("Something went wrong, please try again later.", result.Message);

        }

        [Fact]
        public void ForgetPasswordService_ReturnsInvalidUsername_WhenUserIsNull()
        {
            //Arrange
            var forgetDto = new ForgetDto
            {
                UserName = "username"
            };
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IVerifyPasswordHash>();
            mockAuthRepository.Setup(repo => repo.ValidateUser(forgetDto.UserName)).Returns<User>(null);

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);


            // Act
            var result = target.ForgetPasswordService(forgetDto);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("Invalid username!", result.Message);
            mockAuthRepository.Verify(repo => repo.ValidateUser(forgetDto.UserName), Times.Once);

        }

        [Fact]
        public void ForgetPasswordService_ReturnsFailure_WhenPasswordIsWeak()
        {
            // Arrange
            var fixture = new Fixture();
            var user = fixture.Create<User>();
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IVerifyPasswordHash>();
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Mininum password length should be 8" + Environment.NewLine);
            stringBuilder.Append("Password should be alphanumeric" + Environment.NewLine);
            stringBuilder.Append("Password should contain special characters" + Environment.NewLine);
            var forgetDto = new ForgetDto
            {
                UserName = "username",
                Password = "pass"
            };
            mockAuthRepository.Setup(repo => repo.ValidateUser(forgetDto.UserName)).Returns(user);


            // Act
            var result = target.ForgetPasswordService(forgetDto);

            // Assert
            Assert.False(result.Success);
           Assert.Equal(stringBuilder.ToString(), result.Message);
            mockAuthRepository.Verify(repo => repo.ValidateUser(forgetDto.UserName), Times.Once);

        }
        [Fact]
        public void ForgetPasswordService_ReturnsFailure_WhenPasswordsDontMatch()
        {
            // Arrange
            var fixture = new Fixture();
            var user = fixture.Create<User>();
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IVerifyPasswordHash>();
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);
            var forgetDto = new ForgetDto
            {
                UserName = "username",
                Password = "Password@1234",
                ConfirmPassword = "Password234"
            };
            mockAuthRepository.Setup(repo => repo.ValidateUser(forgetDto.UserName)).Returns(user);


            // Act
            var result = target.ForgetPasswordService(forgetDto);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("Password and confirmation password do not match!", result.Message);
            mockAuthRepository.Verify(repo => repo.ValidateUser(forgetDto.UserName), Times.Once);

        }
        [Fact]
        public void ForgetPasswordService_ReturnsSuccess_WhenPasswordResetSuccessfully()
        {
            // Arrange
            var fixture = new Fixture();
            var user = fixture.Create<User>();
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IVerifyPasswordHash>();
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);
            var forgetDto = new ForgetDto
            {
                UserName = "username",
                Password = "Password@1234",
                ConfirmPassword = "Password@1234"
            };
            mockAuthRepository.Setup(repo => repo.ValidateUser(forgetDto.UserName)).Returns(user);
            mockAuthRepository.Setup(repo => repo.UpdateUser(user));

            // Act
            var result = target.ForgetPasswordService(forgetDto);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("Password reset successfully!", result.Message);
            mockAuthRepository.Verify(repo => repo.ValidateUser(forgetDto.UserName), Times.Once);
            mockAuthRepository.Verify(repo => repo.UpdateUser(user), Times.Once);

        }
        [Fact]
        public void GetUser_ReturnsUser_WhenUserExist()
        {
            // Arrange
            var userId = 1;
            var user = new User
            {
                UserId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Image = "file1.txt",
                LoginId = "abc",
            };
            var mockRepository = new Mock<IAuthRepository>();
            var mockVerifyHash = new Mock<IVerifyPasswordHash>();
            mockRepository.Setup(r => r.GetUser(userId)).Returns(user);
            var userService = new AuthService(mockRepository.Object, mockVerifyHash.Object);

            // Act
            var actual = userService.GetUser(userId);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            mockRepository.Verify(r => r.GetUser(userId), Times.Once);
        }

        [Fact]
        public void GetUser_ReturnsNoRecord_WhenNoUserExist()
        {
            // Arrange
            var userId = 1;
            var mockRepository = new Mock<IAuthRepository>();
            var mockVerifyHash = new Mock<IVerifyPasswordHash>();
            mockRepository.Setup(r => r.GetUser(userId)).Returns<IEnumerable<User>>(null);
            var userService = new AuthService(mockRepository.Object, mockVerifyHash.Object);
            // Act
            var actual = userService.GetUser(userId);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found!", actual.Message);
            mockRepository.Verify(r => r.GetUser(userId), Times.Once);
        }

        [Fact]
        public void ModifyUser_ReturnsAlreadyExists_WhenUserAlreadyExists()
        {
            var userId = 1;
            var user = new User()
            {
                UserId = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Image = "file1.txt",
                LoginId = "abc",
            };
            var mockRepository = new Mock<IAuthRepository>();
            var mockVerifyHash = new Mock<IVerifyPasswordHash>();
            var userService = new AuthService(mockRepository.Object, mockVerifyHash.Object);
            mockRepository.Setup(r => r.UserExist(userId, user.LoginId,user.Email)).Returns(true);

            // Act
            var actual = userService.ModifyUser(user);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("User Exists!", actual.Message);
            mockRepository.Verify(r => r.UserExist(userId, user.LoginId, user.Email), Times.Once);
        }
        [Fact]
        public void ModifyUser_ReturnsSomethingWentWrong_WhenUserNotFound()
        {
            var userId = 1;
            var user = new User()
            {
                UserId = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Image = "file1.txt",
                LoginId = "abc",
            };

            var updatedUser = new User()
            {
                UserId = userId,
                FirstName = "John1"
            };

            var mockRepository = new Mock<IAuthRepository>();
            var mockVerifyHash = new Mock<IVerifyPasswordHash>();
            var userService = new AuthService(mockRepository.Object, mockVerifyHash.Object);
           // mockRepository.Setup(r => r.UserExist(userId, user.LoginId, user.Email)).Returns(true);
           
            mockRepository.Setup(r => r.GetUser(updatedUser.UserId)).Returns<IEnumerable<User>>(null);

            // Act
            var actual = userService.ModifyUser(user);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Something went wrong after sometime", actual.Message);
            mockRepository.Verify(r => r.GetUser(updatedUser.UserId), Times.Once);
        }

        [Fact]
        public void ModifyUser_ReturnsUpdatedSuccessfully_WhenUserModifiedSuccessfully()
        {

            //Arrange
            var userId = 1;
            var user = new User()
            {
                UserId = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Image = "file1.txt",
                LoginId = "abc",
            };

            var updatedUser = new User()
            {
                UserId = userId,
                FirstName = "John1"
            };

            var mockRepository = new Mock<IAuthRepository>();
            var mockVerifyHash = new Mock<IVerifyPasswordHash>();
            var userService = new AuthService(mockRepository.Object, mockVerifyHash.Object);
            mockRepository.Setup(r => r.GetUser(updatedUser.UserId)).Returns(user);
            mockRepository.Setup(c => c.UpdateUser(user)).Returns(true);

            //Act

            var actual = userService.ModifyUser(updatedUser);


            //Assert
            Assert.NotNull(actual);
            Assert.Equal("User Updated successfully", actual.Message);
            mockRepository.Verify(r => r.GetUser(updatedUser.UserId), Times.Once);
            mockRepository.Verify(c => c.UpdateUser(user),Times.Once);

        }
        [Fact]
        public void ModifyUser_ReturnsError_WhenUserModifiedFails()
        {

            //Arrange
            var userId = 1;
            var user = new User()
            {
                UserId = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Image = "file1.txt",
                LoginId = "abc",
            };

            var updatedUser = new User()
            {
                UserId = userId,
                FirstName = "John1"
            };

            var mockRepository = new Mock<IAuthRepository>();
            var mockVerifyHash = new Mock<IVerifyPasswordHash>();
            var userService = new AuthService(mockRepository.Object, mockVerifyHash.Object);
            mockRepository.Setup(r => r.GetUser(updatedUser.UserId)).Returns(user);
            mockRepository.Setup(c => c.UpdateUser(user)).Returns(false);

            //Act

            var actual = userService.ModifyUser(updatedUser);


            //Assert
            Assert.NotNull(actual);
            Assert.Equal("Something went wrong after sometime", actual.Message);
            mockRepository.Verify(r => r.GetUser(updatedUser.UserId), Times.Once);
            mockRepository.Verify(c => c.UpdateUser(user), Times.Once);

        }

    }
}

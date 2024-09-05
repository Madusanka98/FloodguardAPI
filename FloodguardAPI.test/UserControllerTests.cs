using LearnAPI.Controllers;
using LearnAPI.Helper;
using LearnAPI.Modal;
using LearnAPI.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FloodguardAPI.test
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UserController _userController;

        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _userController = new UserController(_mockUserService.Object);
        }

        [Fact]
        public async Task UserRegisteration_ReturnsOkResult_WithValidData()
        {
            // Arrange
            var userRegister = new UserRegister { UserName = "testuser", Email = "testuser@example.com", Name = "Test User", Phone = "1234567890", userType = "user" };
            _mockUserService.Setup(service => service.UserRegisteration(userRegister)).ReturnsAsync(new APIResponse { Result = "pass" });

            // Act
            var result = await _userController.UserRegisteration(userRegister);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<APIResponse>(okResult.Value);
            Assert.Equal("pass", apiResponse.Result);
        }

        [Fact]
        public async Task ResetPassword_ReturnsOkResult_WithValidData()
        {
            // Arrange
            var resetPassword = new Resetpassword { username = "testuser", oldpassword = "oldpassword", newpassword = "newpassword" };
            _mockUserService.Setup(service => service.ResetPassword(resetPassword.username, resetPassword.oldpassword, resetPassword.newpassword))
                            .ReturnsAsync(new APIResponse { Result = "pass" });

            // Act
            var result = await _userController.resetpassword(resetPassword);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<APIResponse>(okResult.Value);
            Assert.Equal("pass", apiResponse.Result);
        }

        [Fact]
        public async Task ForgetPassword_ReturnsOkResult_WithValidUsername()
        {
            // Arrange
            string username = "testuser";
            _mockUserService.Setup(service => service.ForgetPassword(username))
                            .ReturnsAsync(new APIResponse { Result = "pass" });

            // Act
            var result = await _userController.forgetpassword(username);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<APIResponse>(okResult.Value);
            Assert.Equal("pass", apiResponse.Result);
        }

        [Fact]
        public async Task UpdateStatus_ReturnsOkResult_WithValidData()
        {
            // Arrange
            var updateStatus = new Updatestatus { username = "testuser", status = true };
            _mockUserService.Setup(service => service.UpdateStatus(updateStatus.username, updateStatus.status))
                            .ReturnsAsync(new APIResponse { Result = "pass" });

            // Act
            var result = await _userController.updatestatus(updateStatus);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<APIResponse>(okResult.Value);
            Assert.Equal("pass", apiResponse.Result);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithUserList()
        {
            // Arrange
            var userList = new List<UserModel> { new UserModel { Username = "testuser1" }, new UserModel { Username = "testuser2" } };
            _mockUserService.Setup(service => service.Getall()).ReturnsAsync(userList);

            // Act
            var result = await _userController.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var users = Assert.IsType<List<UserModel>>(okResult.Value);
            Assert.Equal(2, users.Count);
        }

        [Fact]
        public async Task GetByCode_ReturnsOkResult_WithUser()
        {
            // Arrange
            string code = "testcode";
            var user = new UserModel { Username = "testuser" };
            _mockUserService.Setup(service => service.Getbycode(code)).ReturnsAsync(user);

            // Act
            var result = await _userController.Getbycode(code);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUser = Assert.IsType<UserModel>(okResult.Value);
            Assert.Equal("testuser", returnedUser.Username);
        }
    }
}

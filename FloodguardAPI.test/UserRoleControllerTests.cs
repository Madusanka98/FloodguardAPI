using Xunit;
using Moq;
using LearnAPI.Controllers;
using LearnAPI.Service;
using LearnAPI.Modal;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using LearnAPI.Helper;
using LearnAPI.Repos.Models;

namespace FloodguardAPI.test
{
    public class UserRoleControllerTests
    {
        private readonly Mock<IUserRoleServicecs> _mockUserRoleService;
        private readonly UserRoleController _controller;

        public UserRoleControllerTests()
        {
            _mockUserRoleService = new Mock<IUserRoleServicecs>();
            _controller = new UserRoleController(_mockUserRoleService.Object);
        }

        [Fact]
        public async Task AssignRolePermission_ReturnsOkResult_WhenDataIsValid()
        {
            // Arrange
            var rolePermissions = new List<TblRolepermission> { new TblRolepermission { Userrole = "Admin", Menucode = "Dashboard" } };
            var apiResponse = new APIResponse { Result = "pass", Message = "Saved successfully." };
            _mockUserRoleService.Setup(service => service.AssignRolePermission(rolePermissions))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await _controller.assignrolepermission(rolePermissions);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(apiResponse, okResult.Value);
        }

        [Fact]
        public async Task GetAllRoles_ReturnsOkResult_WithData()
        {
            // Arrange
            var roles = new List<TblRole> { new TblRole { Name = "Admin" } };
            _mockUserRoleService.Setup(service => service.GetAllRoles())
                .ReturnsAsync(roles);

            // Act
            var result = await _controller.GetAllRoles();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(roles, okResult.Value);
        }

        [Fact]
        public async Task GetAllRoles_ReturnsNotFound_WhenDataIsNull()
        {
            // Arrange
            _mockUserRoleService.Setup(service => service.GetAllRoles())
                .ReturnsAsync((List<TblRole>)null);

            // Act
            var result = await _controller.GetAllRoles();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetAllMenus_ReturnsOkResult_WithData()
        {
            // Arrange
            var menus = new List<TblMenu> { new TblMenu { Name = "Home" } };
            _mockUserRoleService.Setup(service => service.GetAllMenus())
                .ReturnsAsync(menus);

            // Act
            var result = await _controller.GetAllMenus();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(menus, okResult.Value);
        }

        [Fact]
        public async Task GetAllMenusbyrole_ReturnsOkResult_WithData()
        {
            // Arrange
            var appMenus = new List<Appmenu> { new Appmenu { code = "001", Name = "Dashboard" } };
            _mockUserRoleService.Setup(service => service.GetAllMenubyrole("Admin"))
                .ReturnsAsync(appMenus);

            // Act
            var result = await _controller.GetAllMenusbyrole("Admin");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(appMenus, okResult.Value);
        }

        [Fact]
        public async Task GetMenupermissionbyrole_ReturnsOkResult_WithData()
        {
            // Arrange
            var menuPermission = new Menupermission { code = "Dashboard", Haveview = true };
            _mockUserRoleService.Setup(service => service.GetMenupermissionbyrole("Admin", "Dashboard"))
                .ReturnsAsync(menuPermission);

            // Act
            var result = await _controller.GetMenupermissionbyrole("Admin", "Dashboard");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(menuPermission, okResult.Value);
        }
    }
}

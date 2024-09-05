using LearnAPI.Controllers;
using LearnAPI.Modal;
using LearnAPI.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using LearnAPI.Helper;


namespace FloodguardAPI.test
{
    public class RiverControllerTests
    {
        private readonly Mock<IRiverService> _mockService;
        private readonly RiverController _controller;

        public RiverControllerTests()
        {
            _mockService = new Mock<IRiverService>();
            _controller = new RiverController(_mockService.Object);
        }

        #region GetAll 

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfRivers()
        {
            // Arrange
            var mockRivers = new List<River>
                {
                    new River { Id = 1, Name = "River 1", Isactive = true },
                    new River { Id = 2, Name = "River 2", Isactive = true }
                };
            _mockService.Setup(service => service.Getall()).ReturnsAsync(mockRivers);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<River>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        #endregion

        #region Getbycode 

        [Fact]
        public async Task Getbycode_ReturnsOkResult_WithRiver()
        {
            // Arrange
            var mockRiver = new River { Id = 1, Name = "River 1", Isactive = true };
            _mockService.Setup(service => service.Getbycode(It.IsAny<int>())).ReturnsAsync(mockRiver);

            // Act
            var result = await _controller.Getbycode(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<River>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task Getbycode_ReturnsNotFound_WhenRiverNotFound()
        {
            // Arrange
            _mockService.Setup(service => service.Getbycode(It.IsAny<int>())).ReturnsAsync((River)null);

            // Act
            var result = await _controller.Getbycode(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        #endregion

        #region Create

        [Fact]
        public async Task Create_ReturnsOkResult_WithAPIResponse()
        {
            // Arrange
            var mockResponse = new APIResponse { ResponseCode = 201, Result = "pass" };
            _mockService.Setup(service => service.Create(It.IsAny<River>())).ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.Create(new River { Name = "New River" });

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<APIResponse>(okResult.Value);
            Assert.Equal(201, returnValue.ResponseCode);
        }

        #endregion

        #region Update

        [Fact]
        public async Task Update_ReturnsOkResult_WithAPIResponse()
        {
            // Arrange
            var mockResponse = new APIResponse { ResponseCode = 200, Result = "pass" };
            _mockService.Setup(service => service.Update(It.IsAny<River>(), It.IsAny<int>())).ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.Update(new River { Name = "Updated River" }, 1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<APIResponse>(okResult.Value);
            Assert.Equal(200, returnValue.ResponseCode);
        }

        #endregion

        #region Remove

        [Fact]
        public async Task Remove_ReturnsOkResult_WithAPIResponse()
        {
            // Arrange
            var mockResponse = new APIResponse { ResponseCode = 200, Result = "pass" };
            _mockService.Setup(service => service.Remove(It.IsAny<int>())).ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.Remove(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<APIResponse>(okResult.Value);
            Assert.Equal(200, returnValue.ResponseCode);
        }


        #endregion
    }
}
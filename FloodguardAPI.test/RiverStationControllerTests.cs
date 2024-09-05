using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using LearnAPI.Controllers;
using LearnAPI.Service;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using System.Collections.Generic;
using LearnAPI.Modal;
using LearnAPI.Helper;

namespace FloodguardAPI.test
{
    public class RiverStationControllerTests
    {
        private readonly Mock<IRiverStationService> _mockService;
        private readonly Mock<IWebHostEnvironment> _mockEnvironment;
        private readonly RiverStationController _controller;

        public RiverStationControllerTests()
        {
            _mockService = new Mock<IRiverStationService>();
            _mockEnvironment = new Mock<IWebHostEnvironment>();
            _controller = new RiverStationController(_mockService.Object, _mockEnvironment.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WhenDataIsPresent()
        {
            // Arrange
            var mockData = new List<RiverStation> { new RiverStation { Id = 1, Name = "Test Station" } };
            _mockService.Setup(service => service.Getall()).ReturnsAsync(mockData);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<RiverStation>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetAll_ReturnsNotFound_WhenDataIsNull()
        {
            // Arrange
            _mockService.Setup(service => service.Getall()).ReturnsAsync((List<RiverStation>)null);

            // Act
            var result = await _controller.GetAll();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Getbycode_ReturnsOkResult_WhenStationIsFound()
        {
            // Arrange
            var stationId = 1;
            var mockStation = new RiverStation { Id = stationId, Name = "Test Station" };
            _mockService.Setup(service => service.Getbycode(stationId)).ReturnsAsync(mockStation);

            // Act
            var result = await _controller.Getbycode(stationId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<RiverStation>(okResult.Value);
            Assert.Equal(stationId, returnValue.Id);
        }

        [Fact]
        public async Task Getbycode_ReturnsNotFound_WhenStationIsNotFound()
        {
            // Arrange
            var stationId = 1;
            _mockService.Setup(service => service.Getbycode(stationId)).ReturnsAsync((RiverStation)null);

            // Act
            var result = await _controller.Getbycode(stationId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsOkResult_WhenDataIsValid()
        {
            // Arrange
            var newStation = new RiverStation { Name = "New Station" };
            var apiResponse = new APIResponse { ResponseCode = 201, Result = "pass" };
            _mockService.Setup(service => service.Create(newStation)).ReturnsAsync(apiResponse);

            // Act
            var result = await _controller.Create(newStation);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<APIResponse>(okResult.Value);
            Assert.Equal("pass", returnValue.Result);
        }

        [Fact]
        public async Task Update_ReturnsOkResult_WhenUpdateIsSuccessful()
        {
            // Arrange
            var stationId = 1;
            var updatedStation = new RiverStation { Id = stationId, Name = "Updated Station" };
            var apiResponse = new APIResponse { ResponseCode = 200, Result = "pass" };
            _mockService.Setup(service => service.Update(updatedStation, stationId)).ReturnsAsync(apiResponse);

            // Act
            var result = await _controller.Update(updatedStation, stationId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<APIResponse>(okResult.Value);
            Assert.Equal("pass", returnValue.Result);
        }

        [Fact]
        public async Task Remove_ReturnsOkResult_WhenRemovalIsSuccessful()
        {
            // Arrange
            var stationId = 1;
            var apiResponse = new APIResponse { ResponseCode = 200, Result = "pass" };
            _mockService.Setup(service => service.Remove(stationId)).ReturnsAsync(apiResponse);

            // Act
            var result = await _controller.Remove(stationId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<APIResponse>(okResult.Value);
            Assert.Equal("pass", returnValue.Result);
        }
    }
}

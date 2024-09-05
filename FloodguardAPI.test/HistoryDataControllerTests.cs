using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using LearnAPI.Controllers;
using LearnAPI.Service;
using LearnAPI.Modal;
using System.Threading.Tasks;
using System.Collections.Generic;
using LearnAPI.Helper;

namespace FloodguardAPI.test
{
    public class HistoryDataControllerTests
    {
        private readonly Mock<IHistoryDataService> _mockHistoryDataService;
        private readonly Mock<IRiverStationService> _mockRiverStationService;
        private readonly HistoryDataController _controller;

        public HistoryDataControllerTests()
        {
            _mockHistoryDataService = new Mock<IHistoryDataService>();
            _mockRiverStationService = new Mock<IRiverStationService>();
            _controller = new HistoryDataController(_mockHistoryDataService.Object, _mockRiverStationService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithData()
        {
            // Arrange
            var historyData = new List<CurrentPredict>
            {
                new CurrentPredict { DateRange = "2023-09-01", RiverHight = "5.6", Rainfall = "10", River = "Kalu Ganga", StationName = "Putupaula", stationId = 1 },
            };
            _mockHistoryDataService.Setup(service => service.Getall()).ReturnsAsync(historyData);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<CurrentPredict>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetAll_ReturnsNotFoundResult_WhenDataIsNull()
        {
            // Arrange
            _mockHistoryDataService.Setup(service => service.Getall()).ReturnsAsync((List<CurrentPredict>)null);

            // Act
            var result = await _controller.GetAll();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Getbycode_ReturnsOkResult_WithData()
        {
            // Arrange
            var historyData = new HistoryData { Id = 1, RainfallData = 10, Date = System.DateTime.Now, RiverHeight = 5.6, RiverStationId = 1 };
            _mockHistoryDataService.Setup(service => service.Getbycode(It.IsAny<int>())).ReturnsAsync(historyData);

            // Act
            var result = await _controller.Getbycode(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<HistoryData>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task Getbycode_ReturnsNotFoundResult_WhenDataIsNull()
        {
            // Arrange
            _mockHistoryDataService.Setup(service => service.Getbycode(It.IsAny<int>())).ReturnsAsync((HistoryData)null);

            // Act
            var result = await _controller.Getbycode(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsOkResult_WithCreatedData()
        {
            // Arrange
            var historyData = new HistoryData { RainfallData = 10, Date = System.DateTime.Now, RiverHeight = 5.6, RiverStationId = 1 };
            var apiResponse = new APIResponse { ResponseCode = 201, Result = "pass" };
            _mockHistoryDataService.Setup(service => service.Create(It.IsAny<HistoryData>())).ReturnsAsync(apiResponse);

            // Act
            var result = await _controller.Create(historyData);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<APIResponse>(okResult.Value);
            Assert.Equal(201, returnValue.ResponseCode);
        }

        [Fact]
        public async Task TriggerManualyPredict_ReturnsOkResult_WithData()
        {
            // Arrange
            var stations = new List<RiverStation>
            {
                new RiverStation { Id = 1, Name = "Putupaula", Latitude = "6.6828", Longitude = "80.3992" },
                new RiverStation { Id = 2, Name = "Ellagawa", Latitude = "6.9344", Longitude = "80.6093" }
            };

                    var predictData = new List<CurrentPredict>
            {
                new CurrentPredict { DateRange = "2023-09-01", RiverHight = "5.6", Rainfall = "10", River = "Kalu Ganga", StationName = "Putupaula", stationId = 1 },
                new CurrentPredict { DateRange = "2023-09-02", RiverHight = "6.0", Rainfall = "15", River = "Kalu Ganga", StationName = "Ellagawa", stationId = 2 }
            };

            _mockRiverStationService.Setup(service => service.GetallActive()).ReturnsAsync(stations);
            _mockHistoryDataService.Setup(service => service.GetDataMain1(stations, "7")).ReturnsAsync(predictData);

            // Act
            var result = await _controller.triggerManualyPredict("7");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<CurrentPredict>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task TriggerManualyPredict_ReturnsBadRequest_WhenStationsAreNull()
        {
            // Arrange
            _mockRiverStationService.Setup(service => service.GetallActive()).ReturnsAsync((List<RiverStation>)null);

            // Act
            var result = await _controller.triggerManualyPredict("7");

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}

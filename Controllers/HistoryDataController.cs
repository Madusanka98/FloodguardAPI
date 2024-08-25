using LearnAPI.Container;
using LearnAPI.Modal;
using LearnAPI.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace LearnAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryDataController : ControllerBase
    {
        private readonly IHistoryDataService service;
        private readonly IRiverStationService riverStationService;
        //private readonly IWebHostEnvironment environment;
        public HistoryDataController(IHistoryDataService service, IRiverStationService riverStationService//, IWebHostEnvironment environment
            )
        {
            this.service = service;
            this.riverStationService = riverStationService;
            //this.environment = environment;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var data = await this.service.Getall();
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [DisableRateLimiting]

        [HttpGet("Getbycode")]
        public async Task<IActionResult> Getbycode(int id)
        {
            var data = await this.service.Getbycode(id);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(HistoryData _data)
        {
            var data = await this.service.Create(_data);
            return Ok(data);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update(HistoryData _data, int id)
        {
            var data = await this.service.Update(_data, id);
            return Ok(data);
        }

        [HttpDelete("Remove")]
        public async Task<IActionResult> Remove(int id)
        {
            var data = await this.service.Remove(id);
            return Ok(data);
        }

        [HttpGet("currentPredict")]
        public async Task<IActionResult> currentPredict(string configHours)
        {
            List<RiverStation> stations = await riverStationService.GetallActive();
            if (stations != null)
            {
                var data = await this.service.GetDataMain1(stations, configHours);
                return Ok(data);
            }
            else
            {
                return BadRequest();
            }
            
        }

        [HttpGet("currentRiverHeight")]
        public async Task<IActionResult> currentRiverHeight()
        {
            // Define the URL
            /*string url = "https://floodms.navy.lk/api/riverdata";

            // Call the function to get water levels
            var waterLevels = await this.service.GetWaterLevels(url);*/
            return Ok();
            

        }
    }
}

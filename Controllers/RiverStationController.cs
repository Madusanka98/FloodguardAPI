using LearnAPI.Modal;
using LearnAPI.Repos;
using LearnAPI.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace LearnAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RiverStationController : ControllerBase
    {
        private readonly IRiverStationService service;
        private readonly IWebHostEnvironment environment;
        public RiverStationController( IRiverStationService service , IWebHostEnvironment environment )
        {
            this.service = service;
            this.environment = environment;
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
        public async Task<IActionResult> Create(RiverStation _data)
        {
            var data = await this.service.Create(_data);
            return Ok(data);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update(RiverStation _data, int id)
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
    }
}

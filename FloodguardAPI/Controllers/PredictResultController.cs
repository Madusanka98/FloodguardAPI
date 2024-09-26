using FloodguardAPI.Modal;
using FloodguardAPI.Service;
using LearnAPI.Modal;
using LearnAPI.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FloodguardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PredictResultController : ControllerBase
    {
        private readonly IPredictResultService service;
        public PredictResultController(IPredictResultService service)
        {
            this.service = service;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(string configTime)
        {
            var data = await this.service.Getall(Int32.Parse(configTime));
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        /*[HttpPost("Create")]
        public async Task<IActionResult> Create(PredictResult _data)
        {
            var data = await this.service.Create(_data);
            return Ok(data);
        }*/
    }
}

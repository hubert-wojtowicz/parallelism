using System.Threading.Tasks;
using CounterApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ConterAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CounterController : ControllerBase
    {
        private readonly ILogger<CounterController> _logger;
        private readonly CounterService _counterService;

        public CounterController(
              ILogger<CounterController> logger,
              CounterService counterService)
        {
            _logger = logger;
            _counterService = counterService;
        }

        [HttpPost("increase")]
        public async Task<IActionResult> IncreaseCounter()
        {
            _logger.LogInformation("Increase counter");
            // if i would simply await _counterService.Increase/Decrease I wouldn't have race condition
            // most probably

            await Task.Factory.StartNew(async () => await _counterService.Increase());
            return Ok();
        }

        [HttpPost("decrease")]
        public async Task<IActionResult> DecreaseCounter()
        {
            _logger.LogInformation("Decrease counter");

            await Task.Factory.StartNew(async ()=> await _counterService.Decrease());
            return Ok();
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Return counter");
            return Ok(_counterService.Counter);
        }
    }
}

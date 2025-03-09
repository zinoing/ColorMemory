using ColorMemory.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ColorMemory.Controllers
{
    [Route("health")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        private readonly ILogger<ScoreController> _logger;

        public HealthCheckController(ILogger<ScoreController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetHealth()
        {
            _logger.LogInformation("Health check request received");
            return StatusCode(200);
        }
    }
}

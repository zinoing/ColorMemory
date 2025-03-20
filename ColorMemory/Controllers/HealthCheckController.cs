﻿using ColorMemory.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ColorMemory.Controllers
{
    [Route("health")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        private readonly ILogger<HealthCheckController> _logger;

        public HealthCheckController(ILogger<HealthCheckController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetHealth()
        {
            _logger.LogInformation("Health check endpoint was hit.");
            return StatusCode(200);
        }
    }
}

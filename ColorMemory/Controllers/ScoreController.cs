using ColorMemory.DTO;
using ColorMemory.Repository.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ColorMemory.Controllers;
using ColorMemory.Services;

namespace ColorMemory.Controllers
{
    [Route("api/score")]
    [ApiController]
    public class ScoreController : ControllerBase
    {
        private readonly ILogger<ScoreController> _logger;
        private readonly ScoreService _scoreService;

        public ScoreController(ILogger<ScoreController> logger, ScoreService scoreService)
        {
            _logger = logger;
            _scoreService = scoreService;
        }

        [HttpPost("update_weekly")]
        public async Task<IActionResult> SaveWeeklyScore([FromBody] ScoreDTO scoreInfo)
        {
            try
            {
                await _scoreService.SaveWeeklyScoreAsync(scoreInfo);
                _logger.LogInformation($"updated {scoreInfo.PlayerId}'s weekly score");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating score");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("update_national")]
        public async Task<IActionResult> SaveNationalScore([FromBody] ScoreDTO scoreInfo)
        {
            try
            {
                await _scoreService.SaveNationalScoreAsync(scoreInfo);
                _logger.LogInformation($"updated {scoreInfo.PlayerId}'s national score");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating score");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("get_weekly/{playerId}")]
        public async Task<IActionResult> GetWeeklyScore(string playerId)
        {
            var score = await _scoreService.GetWeeklyScoreAsync(playerId);
            return Ok(score);
        }

        [HttpGet("get_national/{playerId}")]
        public async Task<IActionResult> GetNationalScore(string playerId)
        {
            var score = await _scoreService.GetNationalScoreAsync(playerId);
            return Ok(score);
        }

        /// <summary>
        /// Get top 10 scores from ranking data
        /// </summary>
        /// <returns></returns>
        [HttpGet("get_weekly/top_ten")]
        public async Task<IActionResult> GetTopTenWeeklyScores()
        {
            var scores = await _scoreService.GetTopTenWeeklyScoresAsync();
            return Ok(scores);
        }

        [HttpGet("get_national/top_ten")]
        public async Task<IActionResult> GetTopTenNationalScores()
        {
            var scores = await _scoreService.GetTopTenNationalScoresAsync();
            return Ok(scores);
        }
    }
}

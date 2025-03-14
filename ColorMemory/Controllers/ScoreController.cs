using ColorMemory.DTO;
using ColorMemory.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ColorMemory.Controllers
{
    [Route("api/score")]
    [ApiController]
    public class ScoreController : ControllerBase
    {
        private readonly ILogger<ScoreController> _logger;
        private readonly WeeklyRankingDb _weeklyRankingDb;
        private readonly NationalRankingDb _nationalRankingDb;

        public ScoreController(ILogger<ScoreController> logger, WeeklyRankingDb weeklyRankingDb, NationalRankingDb nationalRankingDb)
        {
            _logger = logger;
            _weeklyRankingDb = weeklyRankingDb;
            _nationalRankingDb = nationalRankingDb;
        }

        [HttpPost("update_weekly")]
        public async Task<IActionResult> SaveWeeklyScore([FromBody] ScoreDTO scoreInfo)
        {
            try
            {
                await _weeklyRankingDb.SaveScoreAsync(scoreInfo);
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
                await _nationalRankingDb.SaveScoreAsync(scoreInfo);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating score");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("get_weekly/{userId}")]
        public async Task<IActionResult> GetWeeklyScore(string userId)
        {
            var score = await _weeklyRankingDb.GetScoreAsyncById(userId);
            return Ok(score);
        }

        [HttpGet("get_national/{userId}")]
        public async Task<IActionResult> GetNationalScore(string userId)
        {
            var score = await _nationalRankingDb.GetScoreAsyncById(userId);
            return Ok(score);
        }

        /// <summary>
        /// Get top 10 scores from ranking data
        /// </summary>
        /// <returns></returns>
        [HttpGet("get_weekly/top_ten")]
        public async Task<IActionResult> GetTopTenWeeklyScores()
        {
            var scores = await _weeklyRankingDb.GetTopTenRanksAsync();
            return Ok(scores);
        }

        [HttpGet("get_national/top_ten")]
        public async Task<IActionResult> GetTopTenNationalScores()
        {
            var scores = await _weeklyRankingDb.GetTopTenRanksAsync();
            return Ok(scores);
        }
    }
}

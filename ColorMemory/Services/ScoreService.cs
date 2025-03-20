using ColorMemory.DTO;
using ColorMemory.Repository.Implementations;

namespace ColorMemory.Services
{
    public class ScoreService
    {
        private readonly WeeklyRankingDb _weeklyRankingDb;
        private readonly NationalRankingDb _nationalRankingDb;
        private readonly PlayerService _playerService;

        public ScoreService(WeeklyRankingDb weeklyRankingDb, NationalRankingDb nationalRankingDb, PlayerService playerService)
        {
            _weeklyRankingDb = weeklyRankingDb;
            _nationalRankingDb = nationalRankingDb;
            _playerService = playerService;
        }

        public async Task SaveWeeklyScoreAsync(ScoreDTO scoreInfo)
        {
            //await _weeklyRankingDb.SaveScoreAsync(scoreInfo);
            await _playerService.SetHighScore(scoreInfo.PlayerId, scoreInfo.Score);
        }

        public async Task SaveNationalScoreAsync(ScoreDTO scoreInfo)
        {
            await _nationalRankingDb.SaveScoreAsync(scoreInfo);
        }

        public async Task<double?> GetWeeklyScoreAsync(string playerId)
        {
            return await _weeklyRankingDb.GetScoreAsyncById(playerId);
        }

        public async Task<double?> GetNationalScoreAsync(string playerId)
        {
            return await _nationalRankingDb.GetScoreAsyncById(playerId);
        }

        public async Task<List<ScoreDTO>> GetTopTenWeeklyScoresAsync()
        {
            return await _weeklyRankingDb.GetTopTenRanksAsync();
        }

        public async Task<List<ScoreDTO>> GetTopTenNationalScoresAsync()
        {
            return await _nationalRankingDb.GetTopTenRanksAsync();
        }
    }

}

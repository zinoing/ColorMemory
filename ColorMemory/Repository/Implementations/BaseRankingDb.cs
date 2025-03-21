using ColorMemory.DTO;
using StackExchange.Redis;

namespace ColorMemory.Repository.Implementations
{
    public abstract class BaseRankingDb : IRedisDb
    {
        protected readonly ILogger<BaseRankingDb> _logger;
        protected readonly IDatabase _database;
        protected readonly string _key;

        protected BaseRankingDb(ILogger<BaseRankingDb> logger, IConfiguration configuration, string key)
        {
            _logger = logger;
            var redisHost = configuration["Redis:Host"];
            var redisPort = configuration["Redis:Port"];
            var connectionString = $"{redisHost}:{redisPort}";
            _database = ConnectionMultiplexer.Connect(connectionString).GetDatabase();
            _key = key;
        }

        public async Task SaveScoreAsync(ScoreDTO scoreInfo)
        {
            var id = scoreInfo.PlayerId.ToString();
            var score = scoreInfo.Score;
            await _database.SortedSetAddAsync(_key, id, score);
        }

        public async Task<double?> GetScoreAsyncById(string userId)
        {
            var score = await _database.SortedSetScoreAsync(_key, userId);
            return score.HasValue ? score.Value : null;
        }

        public async Task<long?> GetRankAsyncById(string userId)
        {
            var rank = await _database.SortedSetRankAsync(_key, userId, Order.Descending);
            return rank.HasValue ? rank.Value + 1 : null;
        }

        public async Task<List<ScoreDTO>> GetTopTenRanksAsync()
        {
            var topScores = await _database.SortedSetRangeByRankWithScoresAsync(_key, 0, 9, Order.Descending);

            List<ScoreDTO> scores = new List<ScoreDTO>();

            foreach (var score in topScores)
            {
                scores.Add(new ScoreDTO(score.Element, (int)score.Score));
            }

            return scores;
        }

        public async Task<bool> DeleteScoreAsyncById(string userId)
        {
            return await _database.SortedSetRemoveAsync(_key, userId);
        }
    }

}

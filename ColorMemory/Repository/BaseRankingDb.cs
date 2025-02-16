using ColorMemory.DTO;
using StackExchange.Redis;

namespace ColorMemory.Repository
{
    public abstract class BaseRankingDb : IRedisDb
    {
        protected readonly ILogger<BaseRankingDb> _logger;
        protected readonly IDatabase _database;
        protected readonly string _key;

        protected BaseRankingDb(ILogger<BaseRankingDb> logger, string key)
        {
            _logger = logger;
            _database = ConnectionMultiplexer.Connect("localhost:6379").GetDatabase();
            _key = key;
        }

        public async Task SaveScoreAsync(ScoreDTO scoreInfo)
        {
            var id = scoreInfo.UserId.ToString();
            var score = scoreInfo.Score;
            await _database.SortedSetAddAsync(_key, id, score);
        }

        public async Task<double?> GetScoreAsyncById(string userId)
        {
            var score = await _database.SortedSetScoreAsync(_key, userId);
            return score.HasValue ? score.Value : (double?)null;
        }

        public async Task<long?> GetRankAsyncById(string userId)
        {
            var rank = await _database.SortedSetRankAsync(_key, userId, Order.Descending);
            return rank.HasValue ? rank.Value + 1 : (long?)null; // 0-based 인덱스를 1-based로 변환
        }

        public async Task<bool> DeleteScoreAsyncById(string userId)
        {
            return await _database.SortedSetRemoveAsync(_key, userId);
        }
    }

}

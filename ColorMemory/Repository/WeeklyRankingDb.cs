using ColorMemory.Controllers;
using ColorMemory.DTO;
using StackExchange.Redis;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace ColorMemory.Repository
{
    public class WeeklyRankingDb : BaseRankingDb
    {
        public WeeklyRankingDb(ILogger<WeeklyRankingDb> logger)
            : base(logger, "weekly_rankings") { }
    }
}

using Microsoft.Extensions.Configuration;

namespace ColorMemory.Repository
{
    public class NationalRankingDb : BaseRankingDb
    {
        public NationalRankingDb(ILogger<NationalRankingDb> logger, IConfiguration configuration)
            : base(logger, configuration, "national_rankings") { }
    }
}

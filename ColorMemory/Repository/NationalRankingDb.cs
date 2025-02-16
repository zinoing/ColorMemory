namespace ColorMemory.Repository
{
    public class NationalRankingDb : BaseRankingDb
    {
        public NationalRankingDb(ILogger<NationalRankingDb> logger)
            : base(logger, "national_rankings") { }
    }
}

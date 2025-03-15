using System.ComponentModel.DataAnnotations;

namespace ColorMemory.DTO
{
    public class ScoreDTO : InGameDTO
    {
        [Required]
        public int Score { get; set; }

        public ScoreDTO(string playerId, int score) : base(playerId)
        {
            Score = score;
        }
    }
}

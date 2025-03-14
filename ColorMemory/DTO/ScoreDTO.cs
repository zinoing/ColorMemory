using System.ComponentModel.DataAnnotations;

namespace ColorMemory.DTO
{
    public class ScoreDTO : InGameDTO
    {
        [Required]
        public int Score { get; set; }

        public ScoreDTO(string userId, int score) : base(userId)
        {
            Score = score;
        }
    }
}

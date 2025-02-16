using System.ComponentModel.DataAnnotations;

namespace ColorMemory.DTO
{
    public class InGameDTO
    {
        [Required]
        public string UserId { get; set; }

        public InGameDTO(string userId)
        {
            UserId = userId;
        }
    }
}

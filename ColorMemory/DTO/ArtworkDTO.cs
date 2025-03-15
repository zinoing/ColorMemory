using System.ComponentModel.DataAnnotations;

namespace ColorMemory.DTO
{
    public class ArtworkDTO : InGameDTO
    {
        [Required]
        public int ArtworkId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Artist { get; set; }

        public ArtworkDTO(string playerId, int artworkId, string title, string artist) : base(playerId)
        {
            ArtworkId = artworkId;
            Title = title;
            Artist = artist;
        }
    }
}

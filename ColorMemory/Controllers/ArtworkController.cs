using ColorMemory.Repository.Implementations;
using ColorMemory.DTO;
using ColorMemory.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ColorMemory.Controllers
{
    [Route("api/artwork")]
    [ApiController]
    public class ArtworkController : ControllerBase
    {
        private readonly ILogger<ArtworkController> _logger;
        private readonly ArtworkService _artworkService;
        private readonly PlayerService _playerService;

        public ArtworkController(ILogger<ArtworkController> logger, ArtworkService artworkService, PlayerService playerService)
        {
            _logger = logger;
            _artworkService = artworkService;
            _playerService = playerService;
        }

        [HttpGet("add_artwork/{fileName}")]
        public async Task<IActionResult> AddArtwork(string fileName)
        {
            var artwork = await _artworkService.AddArtworkAsync(fileName);

            if (artwork == null)
                _logger.LogInformation($"{fileName} already exists");

            _logger.LogInformation($"added {fileName} to db");
            return Ok(artwork);
        }

        [HttpGet("add_player_artwork")]
        public async Task<IActionResult> AddPlayerArtwork([FromBody] ArtworkDTO artworkInfo)
        {
            var result = await _playerService.AddArtworkToPlayerAsync(artworkInfo);

            if (result == false)
                return BadRequest(new
                {
                    Message = "Artwork could not be added.",
                    Reason = "The artwork might already be owned by the player or does not exist."
                });

            _logger.LogInformation($"added {artworkInfo.Title} to {artworkInfo.PlayerId}");
            return Ok();
        }

        [HttpGet("get_artworks/{playerId}")]
        public async Task<IActionResult> GetPlayerArtworks(string playerId)
        {
            var artworks = await _playerService.GetArtworksAsync(playerId);

            if (artworks == null || artworks.Count == 0)
                return NotFound("No artworks found for this player.");

            return Ok(artworks);
        }

        [HttpGet("get_unowned_artworks/{playerId}")]
        public async Task<IActionResult> GetPlayerUnownedArtworks(string playerId)
        {
            var artworks = await _playerService.GetUnownedArtworksAsync(playerId);

            if (artworks == null || artworks.Count == 0)
                return NotFound("No artworks found for this player.");

            return Ok(artworks);
        }
    }
}

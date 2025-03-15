using ColorMemory.Data;
using ColorMemory.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ColorMemory.Services
{
    public class PlayerService
    {
        private readonly GameDbContext _context;

        public PlayerService(GameDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SetHighScore(string playerId, int score)
        {
            if (string.IsNullOrEmpty(playerId))
                return false;

            var player = await _context.Players.FirstOrDefaultAsync(p => p.PlayerId == playerId);

            if (player == null)
            {
                player = new Player
                {
                    PlayerId = playerId,
                    Score = score
                };
                _context.Players.Add(player);
            }
            else if (score > player.Score) 
            {
                player.Score = score;
            }

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> AddArtworkToPlayerAsync(ArtworkDTO artworkInfo)
        {
            int artworkId = artworkInfo.ArtworkId;
            string playerId = artworkInfo.PlayerId;

            var player = await _context.Players
                .Include(p => p.PlayerArtworks)
                .FirstOrDefaultAsync(p => p.PlayerId == playerId);

            if (player == null)
                return false;

            var artwork = await _context.Artworks.FindAsync(artworkId);
            if (artwork == null)
                return false;

            var existingEntry = player.PlayerArtworks
                .FirstOrDefault(pa => pa.ArtworkId == artworkId);

            if (existingEntry == null)
            {
                player.PlayerArtworks.Add(new PlayerArtwork
                {
                    PlayerId = playerId,
                    ArtworkId = artworkId,
                    HasIt = true
                });

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<ArtworkDTO>> GetArtworksAsync(string playerId)
        {
            var player = await _context.Players
                .Include(p => p.PlayerArtworks)
                .ThenInclude(pa => pa.Artwork)
                .FirstOrDefaultAsync(p => p.PlayerId == playerId);

            if (player == null)
                return new List<ArtworkDTO>();

            List<Artwork> artworksFromDB = player.PlayerArtworks
                .Where(pa => pa.HasIt)
                .Select(pa => pa.Artwork)
                .ToList();

            List<ArtworkDTO> artworks = new List<ArtworkDTO>();
            foreach (var artwork in artworksFromDB)
            {
                var dto = new ArtworkDTO
                (
                    playerId,
                    artwork.ArtworkId,
                    artwork.Title,
                    artwork.Artist
                );

                artworks.Add(dto);
            }

            return artworks;
        }

        public async Task<List<ArtworkDTO>> GetUnownedArtworksAsync(string playerId)
        {
            var player = await _context.Players
                .Include(p => p.PlayerArtworks)
                .ThenInclude(pa => pa.Artwork)
                .FirstOrDefaultAsync(p => p.PlayerId == playerId);

            if (player == null)
                return new List<ArtworkDTO>();

            List<Artwork> artworksFromDB = player.PlayerArtworks
                .Where(pa => !pa.HasIt)
                .Select(pa => pa.Artwork)
                .ToList();

            List<ArtworkDTO> unownedArtworks = new List<ArtworkDTO>();
            foreach (var artwork in artworksFromDB)
            {
                var dto = new ArtworkDTO
                (
                    playerId,
                    artwork.ArtworkId,
                    artwork.Title,
                    artwork.Artist
                );

                unownedArtworks.Add(dto);
            }

            return unownedArtworks;
        }
    }
}

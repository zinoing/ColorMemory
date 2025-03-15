using ColorMemory.Data;
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

        public async Task<bool> AddArtworkToPlayer(int playerId, int artworkId)
        {
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
    }
}

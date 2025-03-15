using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ColorMemory.Data
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }
        public int Score { get; set; }

        public ICollection<PlayerArtwork> PlayerArtworks { get; set; }
    }

    public class Artwork
    {
        public int ArtworkId { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string S3Url { get; set; }

        public string FileName { get; set; }
        public string ContentType { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class PlayerArtwork
    {
        public int PlayerId { get; set; }
        public Player Player { get; set; }

        public int ArtworkId { get; set; }
        public Artwork Artwork { get; set; }

        public bool HasIt { get; set; } = false;
    }


    public class GameDbContext : DbContext
    {
        public GameDbContext(DbContextOptions<GameDbContext> options)
            : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Artwork> Artworks { get; set; }
        public DbSet<PlayerArtwork> PlayerArtworks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PlayerArtwork>()
                .HasKey(pa => new { pa.PlayerId, pa.ArtworkId });

            modelBuilder.Entity<PlayerArtwork>()
                .HasOne(pa => pa.Player)
                .WithMany(p => p.PlayerArtworks)
                .HasForeignKey(pa => pa.PlayerId);

            modelBuilder.Entity<PlayerArtwork>()
                .HasOne(pa => pa.Artwork)
                .WithMany()
                .HasForeignKey(pa => pa.ArtworkId);
        }

    }
}

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColorMemory.Data
{
    public class Player
    {
        [Key]
        [Required]
        public string PlayerId { get; set; }
        public int Score { get; set; }

        public ICollection<PlayerArtwork> PlayerArtworks { get; set; }
    }

    public class Artwork
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ArtworkId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Artist { get; set; }

        [Required]
        public string S3JsonUrl { get; set; }
        [Required]
        public string S3JpgUrl { get; set; }
        [Required]
        public string FileName { get; set; }
    }

    public class PlayerArtwork
    {
        [Required]
        public string PlayerId { get; set; }
        [Required]
        public Player Player { get; set; }

        public int ArtworkId { get; set; }
        [Required]
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
                .HasForeignKey(pa => pa.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlayerArtwork>()
                .HasOne(pa => pa.Artwork)
                .WithMany()
                .HasForeignKey(pa => pa.ArtworkId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Artwork>(entity =>
            {
                entity.HasKey(a => a.ArtworkId);

                entity.Property(a => a.ArtworkId)
                    .ValueGeneratedOnAdd();

                entity.Property(a => a.Title)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(a => a.Artist)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(a => a.S3JsonUrl)
                    .HasMaxLength(500)
                    .IsRequired();

                entity.Property(a => a.S3JpgUrl)
                    .HasMaxLength(500)
                    .IsRequired();

                entity.Property(a => a.FileName)
                    .HasMaxLength(255)
                    .IsRequired();
            });
        }
    }
}

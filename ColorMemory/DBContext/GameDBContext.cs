using Microsoft.EntityFrameworkCore;

namespace ColorMemory.DBContext
{
    public class Player
    {
        public int PlayerId { get; set; }
        public int Score{ get; set; }
        public ICollection<Artwork> Artworks { get; set; }
    }

    public class Artwork
    {
        public int ArtworkId { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public ICollection<Player> Players { get; set; }
    }

    public class GameDbContext : DbContext
    {
        public GameDbContext(DbContextOptions<GameDbContext> options)
            : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Artwork> Artworks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>()
                .HasMany(p => p.Artworks)
                .WithMany(a => a.Players)
                .UsingEntity(j => j.ToTable("PlayerArtCollections"));
        }
    }

}

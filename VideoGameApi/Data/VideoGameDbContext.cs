using Microsoft.EntityFrameworkCore;
using VideoGameApi.Models;

namespace VideoGameApi.Data
{
    public class VideoGameDbContext : DbContext
    {
        public VideoGameDbContext(DbContextOptions<VideoGameDbContext> options) : base(options)
        {
        }

        public DbSet<VideoGame> VideoGames => Set<VideoGame>();
        public DbSet<VideoGameDetails> VideoGameDetails => Set<VideoGameDetails>();
        public DbSet<Developer> Developers => Set<Developer>();
        public DbSet<Publisher> Publishers => Set<Publisher>();
        public DbSet<Genre> Genres => Set<Genre>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensure required name properties at the database level
            modelBuilder.Entity<Developer>().Property(d => d.Name).IsRequired();
            modelBuilder.Entity<Publisher>().Property(p => p.Name).IsRequired();
            modelBuilder.Entity<Genre>().Property(g => g.Name).IsRequired();

            // Configure one-to-one relationship between VideoGame and VideoGameDetails
            modelBuilder.Entity<VideoGame>()
                .HasOne(v => v.VideoGameDetails)
                .WithOne()
                .HasForeignKey<VideoGameDetails>(d => d.VideoGameId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure many-to-many between VideoGame and Genre (EF creates join table)
            modelBuilder.Entity<VideoGame>()
                .HasMany(v => v.Genres)
                .WithMany(g => g.VideoGames);

            // Seed minimal data for VideoGames (safe because Dev/Publisher are nullable in model)
            modelBuilder.Entity<VideoGame>().HasData(
                new VideoGame
                {
                    Id = 1,
                    Title = "Spider-Man 2",
                    Platform = "PS5"
                },
                new VideoGame
                {
                    Id = 2,
                    Title = "The Legend of Zelda: Breath of the Wild",
                    Platform = "Nintendo Switch"
                },
                new VideoGame
                {
                    Id = 3,
                    Title = "Cyberpunk 2077",
                    Platform = "PC"
                }
            );
        }
    }
}

using Microsoft.EntityFrameworkCore;
using MovieTicketMVC.Models;

namespace MovieTicketMVC.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(
            DbContextOptions<AppDbContext> options
        ) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình giá tiền
            modelBuilder.Entity<Movie>()
                .Property(m => m.BasePrice)
                .HasPrecision(18, 2);

            // Quan hệ một phim - nhiều vé
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Movie)
                .WithMany(m => m.Tickets)
                .HasForeignKey(t => t.MovieId)
                .OnDelete(DeleteBehavior.Restrict);

            // Dữ liệu phim mẫu
            modelBuilder.Entity<Movie>().HasData(
                new Movie
                {
                    Id = 1,
                    Title = "Doraemon",
                    BasePrice = 70000,
                    DurationMinutes = 110,
                    Genre = "Hoạt hình"
                },
                new Movie
                {
                    Id = 2,
                    Title = "Avengers: Endgame",
                    BasePrice = 90000,
                    DurationMinutes = 181,
                    Genre = "Hành động"
                },
                new Movie
                {
                    Id = 3,
                    Title = "Interstellar",
                    BasePrice = 80000,
                    DurationMinutes = 169,
                    Genre = "Khoa học viễn tưởng"
                }
            );
        }
    }
}
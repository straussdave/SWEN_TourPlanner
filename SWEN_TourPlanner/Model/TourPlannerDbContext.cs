using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace SWEN_TourPlanner.Model
{
    public partial class TourPlannerDbContext : DbContext
    {
        public TourPlannerDbContext()
        {
        }

        public TourPlannerDbContext(DbContextOptions<TourPlannerDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Log> Logs { get; set; }

        public virtual DbSet<Tour> Tours { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var a = Assembly.GetExecutingAssembly();
            using var stream = a.GetManifestResourceStream("SWEN_TourPlanner.appsettings.json");

            var config = new ConfigurationBuilder()
                        .AddJsonStream(stream)
                        .Build();
            optionsBuilder.UseNpgsql(config.GetConnectionString("cs"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Log>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("log_pkey");

                entity.ToTable("log");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Comment)
                    .HasMaxLength(500)
                    .HasColumnName("comment");
                entity.Property(e => e.Difficulty).HasColumnName("difficulty");
                entity.Property(e => e.Rating).HasColumnName("rating");
                entity.Property(e => e.TotalTime).HasColumnName("total_time");
                entity.Property(e => e.TourDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("tour_date");
                entity.Property(e => e.TourId).HasColumnName("tour_id");

                entity.HasOne(d => d.Tour).WithMany(p => p.Logs)
                    .HasForeignKey(d => d.TourId)
                    .HasConstraintName("log_tour_id_fkey");
            });

            modelBuilder.Entity<Tour>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("tour_pkey");

                entity.ToTable("tour");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .HasColumnName("description");
                entity.Property(e => e.EstimatedTime).HasColumnName("estimated_time");
                entity.Property(e => e.FromLocation)
                    .HasMaxLength(50)
                    .HasColumnName("from_location");
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
                entity.Property(e => e.RouteImage)
                    .HasMaxLength(100)
                    .HasColumnName("route_image");
                entity.Property(e => e.ToLocation)
                    .HasMaxLength(50)
                    .HasColumnName("to_location");
                entity.Property(e => e.TourDistance).HasColumnName("tour_distance");
                entity.Property(e => e.TransportType)
                    .HasMaxLength(50)
                    .HasColumnName("transport_type");
            });

            OnModelCreatingPartial(modelBuilder);
            SeedData(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Dummy data for Tours
            var tours = new List<Tour>
            {
                new Tour
                {
                    Id = 1,
                    Name = "Tour 1",
                    Description = "Description of Tour 1",
                    FromLocation = "Location A",
                    ToLocation = "Location B",
                    TransportType = "Car",
                    TourDistance = 100,
                    EstimatedTime = 120,
                    RouteImage = "route_1.jpg"
                },
                new Tour
                {
                    Id = 2,
                    Name = "Tour 2",
                    Description = "Description of Tour 2",
                    FromLocation = "Location X",
                    ToLocation = "Location Y",
                    TransportType = "Bike",
                    TourDistance = 80,
                    EstimatedTime = 90,
                    RouteImage = "route_2.jpg"
                },
                // Add more Tour objects as needed
            };

            modelBuilder.Entity<Tour>().HasData(tours);

            // Dummy data for Logs
            var logs = new List<Log>
            {
                new Log
                {
                    Id = 1,
                    TourId = 1,
                    TourDate = new DateTime(2023, 7, 1),
                    Comment = "Nice tour!",
                    Difficulty = 3,
                    TotalTime = 90,
                    Rating = 4
                },
                new Log
                {
                    Id = 2,
                    TourId = 1,
                    TourDate = new DateTime(2023, 7, 5),
                    Comment = "Enjoyed the scenery!",
                    Difficulty = 2,
                    TotalTime = 80,
                    Rating = 5
                },
                // Add more Log objects as needed
            };

            modelBuilder.Entity<Log>().HasData(logs);
        }
    }
}
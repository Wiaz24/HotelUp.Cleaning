using HotelUp.Cleaning.Persistence.EF.Config;
using Microsoft.EntityFrameworkCore;

namespace HotelUp.Cleaning.Persistence.EF;

public class AppDbContext : DbContext
{
    // public DbSet<Entity> Entities { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("cleaning");

        var configuration = new DbContextConfiguration();
        // modelBuilder.ApplyConfiguration<Entity>(configuration);

        base.OnModelCreating(modelBuilder);
    }
}
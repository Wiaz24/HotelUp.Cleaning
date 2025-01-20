using HotelUp.Cleaning.Persistence.Const;
using HotelUp.Cleaning.Persistence.EF.Config;
using HotelUp.Cleaning.Persistence.EF.Postgres;
using HotelUp.Cleaning.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TaskStatus = HotelUp.Cleaning.Persistence.Const.TaskStatus;

namespace HotelUp.Cleaning.Persistence.EF;

public class AppDbContext : DbContext
{
    private readonly PostgresOptions _postgresOptions;
    
    public DbSet<CleaningTask> CleaningTasks { get; set; }
    public DbSet<Cleaner> Cleaners { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options, IOptions<PostgresOptions> postgresOptions)
        : base(options)
    {
        _postgresOptions = postgresOptions.Value;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(_postgresOptions.SchemaName);
        modelBuilder.HasPostgresEnum<CleaningType>();
        modelBuilder.HasPostgresEnum<TaskStatus>();

        var configuration = new DbContextConfiguration();
        modelBuilder.ApplyConfiguration<Cleaner>(configuration);
        modelBuilder.ApplyConfiguration<CleaningTask>(configuration);
        modelBuilder.ApplyConfiguration<Reservation>(configuration);

        base.OnModelCreating(modelBuilder);
    }
}
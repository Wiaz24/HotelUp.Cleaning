using HotelUp.Cleaning.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelUp.Cleaning.Persistence.EF.Config;

internal sealed class DbContextConfiguration
    : IEntityTypeConfiguration<Cleaner>,
        IEntityTypeConfiguration<CleaningTask>,
        IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Cleaner> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasMany(x => x.CleaningTasks)
            .WithOne()
            .HasForeignKey(x => x.CleanerId);
        
        builder.ToTable($"{nameof(Cleaner)}s");
    }

    public void Configure(EntityTypeBuilder<CleaningTask> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Reservation)
            .WithMany()
            .HasForeignKey(x => x.ReservationId);
        
        builder.Property(x => x.RealisationDate)
            .IsRequired()
            .HasConversion(
                x => DateTime.SpecifyKind(x, DateTimeKind.Utc),
                x => DateTime.SpecifyKind(x, DateTimeKind.Utc));
        
        builder.Property(x => x.RoomNumber)
            .IsRequired();
        
        builder.Property(x => x.Status)
            .IsRequired();
        
        builder.Property(x => x.CleaningType)
            .IsRequired();
        
        builder.ToTable($"{nameof(CleaningTask)}s");
    }

    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.PrimitiveCollection(x => x.RoomNumbers)
            .IsRequired();
        
        builder.Property(x => x.StartDate)
            .IsRequired()
            .HasConversion(
                x => DateTime.SpecifyKind(x, DateTimeKind.Utc),
                x => DateTime.SpecifyKind(x, DateTimeKind.Utc));
        
        builder.Property(x => x.EndDate)
            .IsRequired()
            .HasConversion(
                x => DateTime.SpecifyKind(x, DateTimeKind.Utc),
                x => DateTime.SpecifyKind(x, DateTimeKind.Utc));
        
        builder.ToTable($"{nameof(Reservation)}s");
    }
}
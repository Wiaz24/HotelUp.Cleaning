using HotelUp.Cleaning.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelUp.Cleaning.Persistence.EF.Config;

internal sealed class DbContextConfiguration
    : IEntityTypeConfiguration<Cleaner>,
        IEntityTypeConfiguration<CleaningTask>
{
    public void Configure(EntityTypeBuilder<Cleaner> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.ToTable($"{nameof(Cleaner)}s");
    }

    public void Configure(EntityTypeBuilder<CleaningTask> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.ReservationId)
            .IsRequired();
        
        builder.Property(x => x.RealisationDate)
            .IsRequired();
        
        builder.Property(x => x.RoomNumber)
            .IsRequired();
        
        builder.Property(x => x.Status)
            .IsRequired();
        
        builder.Property(x => x.CleaningType)
            .IsRequired();

        builder.HasOne(x => x.Cleaner)
            .WithMany();
        
        builder.ToTable($"{nameof(CleaningTask)}s");
    }
}
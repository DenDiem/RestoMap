using RestoMap.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RestoMap.Infrastructure.Data.Configurations;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.Property(c => c.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Country)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Latitude)
            .HasColumnType("decimal(10,8)")
            .IsRequired();

        builder.Property(c => c.Longitude)
            .HasColumnType("decimal(11,8)")
            .IsRequired();

        builder.HasMany(c => c.Restaurants)
            .WithOne(r => r.City)
            .HasForeignKey(r => r.CityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => new { c.Name, c.Country })
            .HasDatabaseName("IX_Cities_Name_Country");
    }
} 
using RestoMap.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RestoMap.Infrastructure.Data.Configurations;

public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
{
    public void Configure(EntityTypeBuilder<Restaurant> builder)
    {
        builder.Property(r => r.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(r => r.BuildingId)
            .HasMaxLength(50);

        builder.Property(r => r.Latitude)
            .HasColumnType("decimal(10,8)")
            .IsRequired();

        builder.Property(r => r.Longitude)
            .HasColumnType("decimal(11,8)")
            .IsRequired();

        builder.Property(r => r.Address)
            .HasMaxLength(500)
            .IsRequired();

        builder.HasOne(r => r.City)
            .WithMany(c => c.Restaurants)
            .HasForeignKey(r => r.CityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(r => r.BuildingId)
            .HasDatabaseName("IX_Restaurants_BuildingId");

        builder.HasIndex(r => new { r.Latitude, r.Longitude })
            .HasDatabaseName("IX_Restaurants_Location");

        builder.HasIndex(r => r.CityId)
            .HasDatabaseName("IX_Restaurants_CityId");
    }
} 
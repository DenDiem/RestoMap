using RestoMap.Domain.Entities;

namespace RestoMap.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<City> Cities { get; }

    DbSet<Restaurant> Restaurants { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

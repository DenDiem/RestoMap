using RestoMap.Domain.Entities;

namespace RestoMap.Application.Restaurants.Queries.GetRestaurants;

public class CityDto
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Country { get; init; } = string.Empty;

    public double Latitude { get; init; }

    public double Longitude { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<City, CityDto>();
        }
    }
} 
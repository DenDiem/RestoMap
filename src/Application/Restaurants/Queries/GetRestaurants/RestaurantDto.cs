using RestoMap.Domain.Entities;

namespace RestoMap.Application.Restaurants.Queries.GetRestaurants;

public class RestaurantDto
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? BuildingId { get; init; }

    public double Latitude { get; init; }

    public double Longitude { get; init; }

    public string Address { get; init; } = string.Empty;

    public int CityId { get; init; }

    public string CityName { get; init; } = string.Empty;

    public string Country { get; init; } = string.Empty;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Restaurant, RestaurantDto>()
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.City.Country));
        }
    }
} 
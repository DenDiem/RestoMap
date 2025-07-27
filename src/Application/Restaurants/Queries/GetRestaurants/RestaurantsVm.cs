namespace RestoMap.Application.Restaurants.Queries.GetRestaurants;

public class RestaurantsVm
{
    public IReadOnlyCollection<CityDto> Cities { get; init; } = Array.Empty<CityDto>();

    public IReadOnlyCollection<RestaurantDto> Restaurants { get; init; } = Array.Empty<RestaurantDto>();

    public int? SelectedCityId { get; init; }
} 
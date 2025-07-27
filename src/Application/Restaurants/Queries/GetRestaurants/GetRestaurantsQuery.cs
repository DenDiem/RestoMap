using RestoMap.Application.Common.Interfaces;
using RestoMap.Domain.Entities;

namespace RestoMap.Application.Restaurants.Queries.GetRestaurants;

public record GetRestaurantsQuery : IRequest<RestaurantsVm>
{
    public int? CityId { get; init; }
}

public class GetRestaurantsQueryValidator : AbstractValidator<GetRestaurantsQuery>
{
    private readonly IApplicationDbContext _context;

    public GetRestaurantsQueryValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.CityId)
            .Must(BeValidCityId)
            .WithMessage("City ID must be greater than 0 when provided.");
            
        RuleFor(x => x.CityId)
            .MustAsync(CityExists)
            .When(x => x.CityId.HasValue && x.CityId > 0)
            .WithMessage("City with the specified ID does not exist.");
    }

    private bool BeValidCityId(int? cityId)
    {
        return !cityId.HasValue || cityId > 0;
    }

    private async Task<bool> CityExists(int? cityId, CancellationToken cancellationToken)
    {
        if (!cityId.HasValue) return true;
        
        return await _context.Cities
            .AnyAsync(c => c.Id == cityId.Value, cancellationToken);
    }
}

public class GetRestaurantsQueryHandler : IRequestHandler<GetRestaurantsQuery, RestaurantsVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetRestaurantsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RestaurantsVm> Handle(GetRestaurantsQuery request, CancellationToken cancellationToken)
    {
        // Get all cities for the dropdown
        var cities = await _context.Cities
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ProjectTo<CityDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        // Get restaurants, optionally filtered by city
        IQueryable<Restaurant> restaurantsQuery = _context.Restaurants
            .AsNoTracking();

        if (request.CityId.HasValue)
        {
            restaurantsQuery = restaurantsQuery.Where(r => r.CityId == request.CityId.Value);
        }

        restaurantsQuery = restaurantsQuery.Include(r => r.City);

        var restaurants = await restaurantsQuery
            .OrderBy(r => r.Name)
            .ProjectTo<RestaurantDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new RestaurantsVm
        {
            Cities = cities,
            Restaurants = restaurants,
            SelectedCityId = request.CityId
        };
    }
} 
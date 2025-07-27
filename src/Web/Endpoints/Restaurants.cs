using RestoMap.Application.Restaurants.Queries.GetRestaurants;
using Microsoft.AspNetCore.Http.HttpResults;

namespace RestoMap.Web.Endpoints;

public class Restaurants : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetRestaurants);
    }

    public async Task<Ok<RestaurantsVm>> GetRestaurants(ISender sender, [AsParameters] GetRestaurantsQuery query)
    {
        var vm = await sender.Send(query);

        return TypedResults.Ok(vm);
    }
} 
using AutoMapper;
using RestoMap.Application.Common.Interfaces;
using RestoMap.Application.Restaurants.Queries.GetRestaurants;
using RestoMap.Domain.Entities;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Reflection;

namespace RestoMap.Application.UnitTests.Restaurants.Queries;

public class GetRestaurantsQueryHandlerTests
{
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        var configuration = new MapperConfiguration(config => 
            config.AddMaps(Assembly.GetAssembly(typeof(IApplicationDbContext))));
        _mapper = configuration.CreateMapper();
    }

    [Test]
    public void Query_ShouldHaveCorrectProperties()
    {
        // Arrange & Act
        var query = new GetRestaurantsQuery { CityId = 123 };

        // Assert
        query.CityId.Should().Be(123);
    }

    [Test] 
    public void Query_ShouldAllowNullCityId()
    {
        // Arrange & Act
        var query = new GetRestaurantsQuery { CityId = null };

        // Assert
        query.CityId.Should().BeNull();
    }

    [Test]
    public void RestaurantDto_ShouldMapFromRestaurant()
    {
        // Arrange
        var city = new City 
        { 
            Id = 1, 
            Name = "Kyiv", 
            Country = "Ukraine",
            Latitude = 50.4501,
            Longitude = 30.5234
        };

        var restaurant = new Restaurant
        {
            Id = 1,
            Name = "Kanapa",
            BuildingId = "OSM_123",
            Latitude = 50.4547,
            Longitude = 30.5238,
            Address = "Andriyivskyy Descent, 19/8, Kyiv, 04070",
            CityId = 1,
            City = city
        };

        // Act
        var dto = _mapper.Map<RestaurantDto>(restaurant);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(1);
        dto.Name.Should().Be("Kanapa");
        dto.BuildingId.Should().Be("OSM_123");
        dto.Latitude.Should().Be(50.4547);
        dto.Longitude.Should().Be(30.5238);
        dto.Address.Should().Be("Andriyivskyy Descent, 19/8, Kyiv, 04070");
        dto.CityId.Should().Be(1);
        dto.CityName.Should().Be("Kyiv");
        dto.Country.Should().Be("Ukraine");
    }

    [Test]
    public void CityDto_ShouldMapFromCity()
    {
        // Arrange
        var city = new City
        {
            Id = 1,
            Name = "Kyiv",
            Country = "Ukraine",
            Latitude = 50.4501,
            Longitude = 30.5234
        };

        // Act
        var dto = _mapper.Map<CityDto>(city);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(1);
        dto.Name.Should().Be("Kyiv");
        dto.Country.Should().Be("Ukraine");
        dto.Latitude.Should().Be(50.4501);
        dto.Longitude.Should().Be(30.5234);
    }

    [Test]
    public void RestaurantsVm_ShouldHaveCorrectProperties()
    {
        // Arrange
        var cities = new List<CityDto>
        {
            new CityDto { Id = 1, Name = "Kyiv", Country = "Ukraine" }
        };
        
        var restaurants = new List<RestaurantDto>
        {
            new RestaurantDto { Id = 1, Name = "Kanapa", CityId = 1 }
        };

        // Act
        var vm = new RestaurantsVm
        {
            Cities = cities,
            Restaurants = restaurants,
            SelectedCityId = 1
        };

        // Assert
        vm.Should().NotBeNull();
        vm.Cities.Should().HaveCount(1);
        vm.Cities.First().Name.Should().Be("Kyiv");
        vm.Restaurants.Should().HaveCount(1);
        vm.Restaurants.First().Name.Should().Be("Kanapa");
        vm.SelectedCityId.Should().Be(1);
    }
} 
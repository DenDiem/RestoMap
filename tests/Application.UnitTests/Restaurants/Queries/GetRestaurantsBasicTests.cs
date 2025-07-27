using RestoMap.Application.Restaurants.Queries.GetRestaurants;
using RestoMap.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace RestoMap.Application.UnitTests.Restaurants.Queries;

public class GetRestaurantsBasicTests
{
    [Test]
    public void GetRestaurantsQuery_ShouldAllowNullCityId()
    {
        // Arrange & Act
        var query = new GetRestaurantsQuery { CityId = null };

        // Assert
        query.CityId.Should().BeNull();
    }

    [Test]
    public void GetRestaurantsQuery_ShouldAllowValidCityId()
    {
        // Arrange & Act
        var query = new GetRestaurantsQuery { CityId = 1 };

        // Assert
        query.CityId.Should().Be(1);
    }

    [Test]
    public void RestaurantDto_ShouldHaveCorrectProperties()
    {
        // Arrange & Act
        var dto = new RestaurantDto
        {
            Id = 1,
            Name = "Test Restaurant",
            BuildingId = "OSM_123",
            Latitude = 50.4501,
            Longitude = 30.5234,
            Address = "Test Address",
            CityId = 1,
            CityName = "Kyiv",
            Country = "Ukraine"
        };

        // Assert
        dto.Id.Should().Be(1);
        dto.Name.Should().Be("Test Restaurant");
        dto.BuildingId.Should().Be("OSM_123");
        dto.Latitude.Should().Be(50.4501);
        dto.Longitude.Should().Be(30.5234);
        dto.Address.Should().Be("Test Address");
        dto.CityId.Should().Be(1);
        dto.CityName.Should().Be("Kyiv");
        dto.Country.Should().Be("Ukraine");
    }

    [Test]
    public void CityDto_ShouldHaveCorrectProperties()
    {
        // Arrange & Act
        var dto = new CityDto
        {
            Id = 1,
            Name = "Kyiv",
            Country = "Ukraine",
            Latitude = 50.4501,
            Longitude = 30.5234
        };

        // Assert
        dto.Id.Should().Be(1);
        dto.Name.Should().Be("Kyiv");
        dto.Country.Should().Be("Ukraine");
        dto.Latitude.Should().Be(50.4501);
        dto.Longitude.Should().Be(30.5234);
    }

    [Test]
    public void RestaurantsVm_ShouldInitializeCorrectly()
    {
        // Arrange & Act
        var vm = new RestaurantsVm
        {
            Cities = new List<CityDto> { new CityDto { Id = 1, Name = "Kyiv" } },
            Restaurants = new List<RestaurantDto> { new RestaurantDto { Id = 1, Name = "Test" } },
            SelectedCityId = 1
        };

        // Assert
        vm.Cities.Should().HaveCount(1);
        vm.Restaurants.Should().HaveCount(1);
        vm.SelectedCityId.Should().Be(1);
    }
} 
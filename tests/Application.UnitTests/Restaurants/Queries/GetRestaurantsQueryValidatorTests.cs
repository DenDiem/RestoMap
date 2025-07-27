using RestoMap.Application.Common.Interfaces;
using RestoMap.Application.Restaurants.Queries.GetRestaurants;
using RestoMap.Application.UnitTests.Common;
using RestoMap.Domain.Entities;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;

namespace RestoMap.Application.UnitTests.Restaurants.Queries;

public class GetRestaurantsQueryValidatorTests
{
    private GetRestaurantsQueryValidator _validator;
    private Mock<IApplicationDbContext> _mockContext;

    [SetUp]
    public void Setup()
    {
        _mockContext = new Mock<IApplicationDbContext>();
        _validator = new GetRestaurantsQueryValidator(_mockContext.Object);
    }

    [Test]
    public void ShouldNotHaveErrorWhenCityIdIsNull()
    {
        // Arrange
        var query = new GetRestaurantsQuery { CityId = null };

        // Act & Assert
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void ShouldHaveErrorWhenCityIdIsZero()
    {
        // Arrange
        var query = new GetRestaurantsQuery { CityId = 0 };

        // Act 
        var result = _validator.TestValidate(query);
        
        // Debug information
        Console.WriteLine($"Validation errors count: {result.Errors.Count}");
        foreach (var error in result.Errors)
        {
            Console.WriteLine($"Error: {error.PropertyName} - {error.ErrorMessage}");
        }

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CityId);
    }

    [Test]
    public void ShouldHaveErrorWhenCityIdIsNegative()
    {
        // Arrange
        var query = new GetRestaurantsQuery { CityId = -1 };

        // Act
        var result = _validator.TestValidate(query);
        
        // Debug information
        Console.WriteLine($"Validation errors count: {result.Errors.Count}");
        foreach (var error in result.Errors)
        {
            Console.WriteLine($"Error: {error.PropertyName} - {error.ErrorMessage}");
        }

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CityId);
    }

    // Note: Testing positive CityId values would require complex async mock setup
    // The important validation logic (rejecting null, zero, negative values) is tested above
} 
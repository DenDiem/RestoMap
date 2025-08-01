﻿using RestoMap.Application.Common.Behaviours;
using RestoMap.Application.Common.Interfaces;
using RestoMap.Application.Restaurants.Queries.GetRestaurants;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace RestoMap.Application.UnitTests.Common.Behaviours;

public class RequestLoggerTests
{
    private Mock<ILogger<GetRestaurantsQuery>> _logger = null!;
    private Mock<IUser> _user = null!;
    private Mock<IIdentityService> _identityService = null!;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<GetRestaurantsQuery>>();
        _user = new Mock<IUser>();
        _identityService = new Mock<IIdentityService>();
    }

    [Test]
    public async Task ShouldCallGetUserNameAsyncOnceIfAuthenticated()
    {
        _user.Setup(x => x.Id).Returns(Guid.NewGuid().ToString());

        var requestLogger = new LoggingBehaviour<GetRestaurantsQuery>(_logger.Object, _user.Object, _identityService.Object);

        await requestLogger.Process(new GetRestaurantsQuery { CityId = 1 }, new CancellationToken());

        _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task ShouldNotCallGetUserNameAsyncOnceIfUnauthenticated()
    {
        var requestLogger = new LoggingBehaviour<GetRestaurantsQuery>(_logger.Object, _user.Object, _identityService.Object);

        await requestLogger.Process(new GetRestaurantsQuery { CityId = 1 }, new CancellationToken());

        _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Never);
    }
}

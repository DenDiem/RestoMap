using RestoMap.Domain.Constants;
using RestoMap.Domain.Entities;
using RestoMap.Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace RestoMap.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            Console.WriteLine("🔧 Connection: " + _context.Database.GetConnectionString());

            // See https://jasontaylor.dev/ef-core-database-initialisation-strategies
            await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var administratorRole = new IdentityRole(Roles.Administrator);

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        // Default users
        var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Administrator1!");
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new [] { administratorRole.Name });
            }
        }

        // Seed Cities and Restaurants
        if (!_context.Cities.Any())
        {
            var cities = new List<City>
            {
                // Ukraine cities
                new City
                {
                    Name = "Kyiv",
                    Country = "Ukraine",
                    Latitude = 50.4501,
                    Longitude = 30.5234
                },
                new City
                {
                    Name = "Lviv",
                    Country = "Ukraine", 
                    Latitude = 49.8397,
                    Longitude = 24.0297
                },
                new City
                {
                    Name = "Odesa",
                    Country = "Ukraine",
                    Latitude = 46.4825,
                    Longitude = 30.7233
                },
                new City
                {
                    Name = "Kharkiv",
                    Country = "Ukraine",
                    Latitude = 49.9935,
                    Longitude = 36.2304
                },
                new City
                {
                    Name = "Dnipro",
                    Country = "Ukraine",
                    Latitude = 48.4647,
                    Longitude = 35.0462
                },
                // International cities
                new City
                {
                    Name = "Warsaw",
                    Country = "Poland",
                    Latitude = 52.2297,
                    Longitude = 21.0122
                },
                new City
                {
                    Name = "Berlin",
                    Country = "Germany",
                    Latitude = 52.5200,
                    Longitude = 13.4050
                },
                new City
                {
                    Name = "Prague",
                    Country = "Czech Republic",
                    Latitude = 50.0755,
                    Longitude = 14.4378
                }
            };

            _context.Cities.AddRange(cities);
            await _context.SaveChangesAsync();

            // Now seed restaurants for these cities
            var kyiv = cities.First(c => c.Name == "Kyiv");
            var lviv = cities.First(c => c.Name == "Lviv");
            var odesa = cities.First(c => c.Name == "Odesa");
            var kharkiv = cities.First(c => c.Name == "Kharkiv");
            var dnipro = cities.First(c => c.Name == "Dnipro");
            var warsaw = cities.First(c => c.Name == "Warsaw");
            var berlin = cities.First(c => c.Name == "Berlin");
            var prague = cities.First(c => c.Name == "Prague");

            var restaurants = new List<Restaurant>
            {
                // Kyiv restaurants (most detailed since it's required)
                new Restaurant
                {
                    Name = "Kanapa",
                    BuildingId = "OSM_1234567890",
                    Latitude = 50.4547,
                    Longitude = 30.5238,
                    Address = "Andriyivskyy Descent, 19/8, Kyiv, 04070",
                    CityId = kyiv.Id
                },
                new Restaurant
                {
                    Name = "BEEF meat & wine",
                    BuildingId = "OSM_1234567891",
                    Latitude = 50.4501,
                    Longitude = 30.5169,
                    Address = "Shota Rustaveli St, 4, Kyiv, 01001",
                    CityId = kyiv.Id
                },
                new Restaurant
                {
                    Name = "Ostannya Barykada",
                    BuildingId = "OSM_1234567892",
                    Latitude = 50.4492,
                    Longitude = 30.5236,
                    Address = "Maidan Nezalezhnosti, 1, Kyiv, 02000",
                    CityId = kyiv.Id
                },
                new Restaurant
                {
                    Name = "Tsarske Selo",
                    BuildingId = "OSM_1234567893",
                    Latitude = 50.4516,
                    Longitude = 30.5245,
                    Address = "Volodymyrska St, 10, Kyiv, 01001",
                    CityId = kyiv.Id
                },
                new Restaurant
                {
                    Name = "O'Panas",
                    BuildingId = "OSM_1234567894",
                    Latitude = 50.4463,
                    Longitude = 30.5143,
                    Address = "Velyka Vasylkivska St, 55A, Kyiv, 03150",
                    CityId = kyiv.Id
                },
                new Restaurant
                {
                    Name = "Spotykach",
                    BuildingId = "OSM_1234567895",
                    Latitude = 50.4521,
                    Longitude = 30.5186,
                    Address = "Podil district, Kyiv, 04070",
                    CityId = kyiv.Id
                },

                // Lviv restaurants
                new Restaurant
                {
                    Name = "Baczewski Restaurant",
                    BuildingId = "OSM_2234567890",
                    Latitude = 49.8419,
                    Longitude = 24.0315,
                    Address = "Shevska St, 8, Lviv, 79000",
                    CityId = lviv.Id
                },
                new Restaurant
                {
                    Name = "Amadeus",
                    BuildingId = "OSM_2234567891",
                    Latitude = 49.8397,
                    Longitude = 24.0297,
                    Address = "Rynok Square, 4, Lviv, 79008",
                    CityId = lviv.Id
                },
                new Restaurant
                {
                    Name = "Kryivka",
                    BuildingId = "OSM_2234567892",
                    Latitude = 49.8414,
                    Longitude = 24.0319,
                    Address = "Rynok Square, 14, Lviv, 79008",
                    CityId = lviv.Id
                },
                new Restaurant
                {
                    Name = "Centolire",
                    BuildingId = "OSM_2234567893",
                    Latitude = 49.8405,
                    Longitude = 24.0301,
                    Address = "Valova St, 4, Lviv, 79008",
                    CityId = lviv.Id
                },

                // Odesa restaurants  
                new Restaurant
                {
                    Name = "Bernardazzi",
                    BuildingId = "OSM_3234567890",
                    Latitude = 46.4825,
                    Longitude = 30.7394,
                    Address = "Derybasivska St, 20, Odesa, 65026",
                    CityId = odesa.Id
                },
                new Restaurant
                {
                    Name = "Tavernetta",
                    BuildingId = "OSM_3234567891",
                    Latitude = 46.4836,
                    Longitude = 30.7416,
                    Address = "Katerynynska St, 45, Odesa, 65026",
                    CityId = odesa.Id
                },
                new Restaurant
                {
                    Name = "Dacha",
                    BuildingId = "OSM_3234567892",
                    Latitude = 46.4854,
                    Longitude = 30.7394,
                    Address = "Derybasivska St, 15, Odesa, 65026",
                    CityId = odesa.Id
                },

                // Kharkiv restaurants
                new Restaurant
                {
                    Name = "Stargorod",
                    BuildingId = "OSM_4234567890",
                    Latitude = 49.9881,
                    Longitude = 36.2310,
                    Address = "Sumska St, 18, Kharkiv, 61000",
                    CityId = kharkiv.Id
                },
                new Restaurant
                {
                    Name = "Rozmarin",
                    BuildingId = "OSM_4234567891",
                    Latitude = 49.9925,
                    Longitude = 36.2411,
                    Address = "Pushkinska St, 32, Kharkiv, 61057",
                    CityId = kharkiv.Id
                },

                // Dnipro restaurants
                new Restaurant
                {
                    Name = "BM Grill",
                    BuildingId = "OSM_5234567890",
                    Latitude = 48.4647,
                    Longitude = 35.0380,
                    Address = "Yavornytskoho Ave, 91, Dnipro, 49000",
                    CityId = dnipro.Id
                },

                // Warsaw restaurants
                new Restaurant
                {
                    Name = "Atelier Amaro",
                    BuildingId = "OSM_6234567890",
                    Latitude = 52.2319,
                    Longitude = 21.0067,
                    Address = "Agrykoli 1, Warsaw, 00-184",
                    CityId = warsaw.Id
                },
                new Restaurant
                {
                    Name = "Opasły Tom",
                    BuildingId = "OSM_6234567891",
                    Latitude = 52.2297,
                    Longitude = 21.0122,
                    Address = "Foksal 17, Warsaw, 00-372",
                    CityId = warsaw.Id
                },

                // Berlin restaurants
                new Restaurant
                {
                    Name = "Restaurant Tim Raue",
                    BuildingId = "OSM_7234567890",
                    Latitude = 52.5069,
                    Longitude = 13.4047,
                    Address = "Rudi-Dutschke-Straße 26, Berlin, 10969",
                    CityId = berlin.Id
                },
                new Restaurant
                {
                    Name = "Zur Letzten Instanz",
                    BuildingId = "OSM_7234567891",
                    Latitude = 52.5170,
                    Longitude = 13.4122,
                    Address = "Waisenstraße 14-16, Berlin, 10179",
                    CityId = berlin.Id
                },

                // Prague restaurants
                new Restaurant
                {
                    Name = "La Degustation Bohême Bourgeoise",
                    BuildingId = "OSM_8234567890",
                    Latitude = 50.0874,
                    Longitude = 14.4212,
                    Address = "Haštalská 18, Prague, 110 00",
                    CityId = prague.Id
                },
                new Restaurant
                {
                    Name = "U Fleku",
                    BuildingId = "OSM_8234567891",
                    Latitude = 50.0820,
                    Longitude = 14.4162,
                    Address = "Křemencova 11, Prague, 110 00",
                    CityId = prague.Id
                }
            };

            _context.Restaurants.AddRange(restaurants);
            await _context.SaveChangesAsync();
        }
    }
}

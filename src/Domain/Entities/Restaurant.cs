namespace RestoMap.Domain.Entities;

/// <summary>
/// Represents a restaurant or dining establishment
/// </summary>
public class Restaurant : BaseAuditableEntity
{
    /// <summary>
    /// The name of the restaurant
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// OpenStreetMap building identifier
    /// </summary>
    public string? BuildingId { get; set; }
    
    /// <summary>
    /// Restaurant's latitude coordinate
    /// </summary>
    public double Latitude { get; set; }
    
    /// <summary>
    /// Restaurant's longitude coordinate
    /// </summary>
    public double Longitude { get; set; }
    
    /// <summary>
    /// Full address of the restaurant
    /// </summary>
    public string Address { get; set; } = string.Empty;
    
    /// <summary>
    /// Foreign key to the city where this restaurant is located
    /// </summary>
    public int CityId { get; set; }
    
    /// <summary>
    /// Navigation property to the city
    /// </summary>
    public City City { get; set; } = null!;
} 
namespace RestoMap.Domain.Entities;

/// <summary>
/// Represents a city where restaurants can be located
/// </summary>
public class City : BaseAuditableEntity
{
    /// <summary>
    /// The name of the city
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// The country where the city is located
    /// </summary>
    public string Country { get; set; } = string.Empty;
    
    /// <summary>
    /// City's latitude coordinate
    /// </summary>
    public double Latitude { get; set; }
    
    /// <summary>
    /// City's longitude coordinate
    /// </summary>
    public double Longitude { get; set; }
    
    /// <summary>
    /// Collection of restaurants located in this city
    /// </summary>
    public IList<Restaurant> Restaurants { get; private set; } = new List<Restaurant>();
} 
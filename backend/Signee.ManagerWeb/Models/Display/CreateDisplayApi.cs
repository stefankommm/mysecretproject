namespace Signee.ManagerWeb.Models.Display;
using Display = Domain.Entities.Display.Display;

public class CreateDisplayApi
{ 
    public string Name { get; set; }
    
    public string? GroupId { get; set; }

    /// <summary>
    /// Maps API model CreateDisplayApi to domain model Display 
    /// </summary>
    /// <param name="displayApi">API model</param>
    /// <returns>Domain model</returns>
    public static Display ToDomainModel(CreateDisplayApi displayApi)
    {
        return new Display
        {
            Name = displayApi.Name,
            GroupId = displayApi.GroupId
        };
    }
}
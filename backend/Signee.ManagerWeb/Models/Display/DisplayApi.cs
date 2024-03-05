namespace Signee.ManagerWeb.Models.Display;
using Display = Domain.Entities.Display.Display;

public class DisplayResponseApi
{
    public string? Id { get; set; } // "www.nasastranka.com/display/Id=123"
    
    public string? Name { get; set; } = string.Empty;
    
    public int? PairingCode { get; set; }
    
    public string? GroupId { get; set; }
    
    /// <summary>
    /// Maps domain model Display to DisplayResponseApi model 
    /// </summary>
    /// <param name="display">Domain model</param>
    /// <returns>API model</returns>
    public static DisplayResponseApi FromDomainModel(Display? display)
    {
        return new DisplayResponseApi()
        {
            Id = display?.Id ?? string.Empty,
            Name = display?.Name ?? string.Empty,
            PairingCode = display?.PairingCode ?? new(),
            GroupId = display?.GroupId ?? string.Empty
        };
    }
}
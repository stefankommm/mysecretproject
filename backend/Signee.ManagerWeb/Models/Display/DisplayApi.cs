using Signee.Domain.Entities.Display;

namespace Signee.ManagerWeb.Models.Display;

public class DisplayApi
{
    public string? Id { get; set; } // "www.nasastranka.com/display/Id=123"
    public string? Name { get; set; } = string.Empty;
    
    public string? Url { get; set; } = string.Empty;
    
    public int? PairingCode { get; set; }
    
    public string? GroupId { get; set; }
}
using Signee.Domain.Entities.Display;

namespace Signee.ManagerWeb.Models.Display;

public class DisplayApi
{
    public string Id { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string? Url { get; set; }
    public int? PairingCode { get; set; }
    public Content? Content { get; set; }

    public string? CreatedByName { get; set; }
}
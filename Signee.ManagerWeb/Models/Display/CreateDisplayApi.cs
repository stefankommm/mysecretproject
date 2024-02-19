namespace Signee.ManagerWeb.Models.Display;

public class CreateDisplayApi
{
    public required int Id { get; set; }
    public string? Name { get; set; }
    public string? creatorEmail { get; set; }
}
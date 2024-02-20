namespace Signee.ManagerWeb.Models.Display;

public class CreateDisplayApi
{
    public required int Id { get; set; }
    public string? Name { get; set; }
    public string? CreatorEmail { get; set; }

    public string? ImgUrl { get; set; }
}
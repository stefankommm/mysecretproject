using Signee.ManagerWeb.Models.Display;

namespace Signee.ManagerWeb.Models.Group;

public class GroupDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsGroup { get; set; }
    public List<DisplayApi> Displays { get; set; }
    public List<string> Views { get; set; }
}
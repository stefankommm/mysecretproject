using Signee.ManagerWeb.Models.Display;

namespace Signee.ManagerWeb.Models.Group;
using Group = Domain.Entities.Group.Group;

public class GroupResponseApi
{
    public string Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<DisplayResponseApi> Displays { get; set; }
    public IEnumerable<string> ViewIds { get; set; }

    /// <summary>
    /// Maps domain model Group to GroupResponseApi model 
    /// </summary>
    /// <param name="group">Domain model</param>
    /// <returns>API model</returns>
    public static GroupResponseApi FromDomainModel(Group? group)
    {
        return new GroupResponseApi
        {
            Id = group?.Id ?? string.Empty,
            Name = group?.Name ?? string.Empty,
            Displays = group?.Displays.Select(d => DisplayResponseApi.FromDomainModel(d)) ?? new List<DisplayResponseApi>(),
            ViewIds = group?.Views.Select(v => v.Id) ?? new List<string>()
        };
    }
}
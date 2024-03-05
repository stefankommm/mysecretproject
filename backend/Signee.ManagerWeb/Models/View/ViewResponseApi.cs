using Signee.ManagerWeb.Models.Group;

namespace Signee.ManagerWeb.Models.View;
using View = Signee.Domain.Entities.View.View;

public class ViewResponseApi
{
    public string Id { get; set; } = string.Empty;    
    public DateTime From { get; set; }  
    public DateTime To { get; set; }
    public string? GroupId { get; set; }
    public GroupResponseApi? Group { get; set; }
    
    // TODO Update this model...

    /// <summary>
    /// Maps domain model View to ViewResponseApi model 
    /// </summary>
    /// <param name="view">Domain model</param>
    /// <returns>API model</returns>
    public static ViewResponseApi FromDomainModel(View? view)
    {
        return new ViewResponseApi
        {
            Id = view?.Id ?? string.Empty,
            From = view?.From ?? new DateTime(),
            To = view?.To ?? new DateTime(),
            GroupId = view?.GroupId ?? string.Empty,
            Group = GroupResponseApi.FromDomainModel(view?.Group)
        };
    }
}

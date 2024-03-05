namespace Signee.ManagerWeb.Models.View;
using View = Signee.Domain.Entities.View.View;

public class CreateViewApi
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }

    /// <summary>
    /// Maps API model CreateViewApi to domain model View 
    /// </summary>
    /// <param name="viewApi">API model</param>
    /// <returns>Domain model</returns>
    public static View ToDomainModel(CreateViewApi viewApi)
    {
        return new View
        {
            From = viewApi.From,
            To = viewApi.To
        };
    }
}
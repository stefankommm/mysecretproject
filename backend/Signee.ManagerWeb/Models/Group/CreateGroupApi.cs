namespace Signee.ManagerWeb.Models.Group;
using Group =  Domain.Entities.Group.Group;

public class CreateGroupApi
{
    public string Name { get; set; }
    
    /// <summary>
    /// Maps API model CreateGroupApi to domain model Group 
    /// </summary>
    /// <param name="groupApi">API model</param>
    /// <returns>Domain model</returns>
    public static Group ToDomainModel(CreateGroupApi groupApi)
    {
        return new Group
        {
            Name = groupApi.Name
        };
    }
}
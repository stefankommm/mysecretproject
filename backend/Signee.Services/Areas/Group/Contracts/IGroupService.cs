namespace Signee.Services.Areas.Group.Contracts;

using Group = Domain.Entities.Group.Group;
using View = Domain.Entities.View.View;

public interface IGroupService
{
    Task<IEnumerable<Group>> GetAll();

    Task<Group?> GetById(string id);

    Task<IEnumerable<Group>> GetAllWithSingleDisplayAsync();

    Task<IEnumerable<Group>> GetAllWithMultipleDisplaysAsync();

    Task Add(Group g, string userId);

    Task Update(Group g);

    Task DeleteById(string id);

    // User methods
    Task<IEnumerable<Group>> GetByUserId(string userId);

    // Display methods
    Task<IEnumerable<Group>> GetByDisplayId(string displayId);

    Task AddDisplayToGroup(string groupId, string displayId);

    Task DeleteDisplayFromGroup(string groupId, string displayId);

    // View methods
    Task AddViewToGroup(string groupId, string viewId);

    Task DeleteViewFromGroup(string groupId, string viewId);

    Task<View> GetCurrentView(string groupId);
}
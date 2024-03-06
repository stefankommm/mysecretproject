using Signee.Domain.Identity;

namespace Signee.Services.Auth.Contracts

{
    public interface IUserContextProvider
    {
        // Gets the current user's identifier
        string GetCurrentUserId();

        // Gets a list of roles associated with the current user
        IEnumerable<Role> GetCurrentRoles();

        // You can add more methods related to the user context as needed
        // For example, getting permissions or other user-specific information
    }
}
using System.Net;
using Signee.Domain.Identity;

namespace Signee.Services.Areas.Auth.Contracts

{
    public interface IUserContextProvider
    {
         /// <summary>
         /// Gets the current user's identifier
         /// </summary>
        string GetCurrentUserId();

        // Gets a list of roles associated with the current user
        IEnumerable<Role> GetCurrentRoles();
        
        bool IsAdmin();

        IPAddress? GetIpAddress();

        string? GetDeviceViewport();

        // You can add more methods related to the user context as needed
        // For example, getting permissions or other user-specific information
    }
}
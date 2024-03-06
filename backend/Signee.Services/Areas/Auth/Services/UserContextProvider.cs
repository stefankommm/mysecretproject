using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Signee.Domain.Identity;
using Signee.Services.Auth.Contracts;

namespace Signee.Services.Auth.Services;

public class UserContextProvider : IUserContextProvider
{
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        
        public string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        }

        public IEnumerable<Role> GetCurrentRoles()
        {
            var rolesClaims = _httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role);
            if (rolesClaims == null) return new List<Role>();

            var roles = new List<Role>();
            foreach (var roleClaim in rolesClaims)
            {
                if (Enum.TryParse<Role>(roleClaim.Value, out var role))
                {
                    roles.Add(role);
                }
                else
                {
                    // handle the case where parsing failed
                    // e.g., log a warning or throw a custom exception
                }
            }
            return roles;
        }
}
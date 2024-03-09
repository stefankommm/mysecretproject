    using System.Net;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Http;
    using Signee.Domain.Identity;
    using Signee.Services.Areas.Auth.Contracts;

    namespace Signee.Services.Areas.Auth.Providers;

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

            public bool IsAdmin() => GetCurrentRoles().Contains(Role.Admin);

            public IPAddress? GetIpAddress()
            {
                return _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress;
            }

            public string? GetDeviceViewport()
            {
                return _httpContextAccessor.HttpContext?.Request.Headers["Viewport"].ToString();
            }
    }
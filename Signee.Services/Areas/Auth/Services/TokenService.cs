using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Signee.Domain.Identity;
using Signee.Services.Areas.Auth.Contracts;

namespace Signee.Services.Areas.Auth.Services;

public class TokenService : ITokenService
{
    private const int ExpirationMinutes = 60;
    private readonly ILogger<TokenService> _logger;

    public TokenService(ILogger<TokenService> logger)
    {
        _logger = logger;
    }
    
    public string GenerateToken(ApplicationUser user)
    {
        var expiration = DateTime.UtcNow.AddMinutes(ExpirationMinutes);
        var token = GenerateJwtToken(
            GenerateClaims(user),
            CreateSigningCredentials(),
            expiration
        );
        var tokenHandler = new JwtSecurityTokenHandler();
        
        _logger.LogInformation("JWT Token created");
        
        return tokenHandler.WriteToken(token);
    }
    
    /// <summary>
    /// Generates a JWT token using the provided claims, signing credentials, and expiration time.
    /// </summary>
    /// <param name="claims">The claims to include in the token.</param>
    /// <param name="credentials">The signing credentials used to sign the token.</param>
    /// <param name="expiration">The expiration time of the token.</param>
    /// <returns>The JWT token.</returns>
    private JwtSecurityToken GenerateJwtToken(List<Claim> claims, SigningCredentials credentials,
        DateTime expiration) =>
        new(
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["ValidIssuer"],
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["ValidAudience"],
            claims,
            expires: expiration,
            signingCredentials: credentials
        );

    /// <summary>
    /// Creates claims for the specified user, encoding UID, UserName, Email etc...
    /// </summary>
    /// <param name="user">The user for whom the claims are created.</param>
    /// <returns>A list of claims.</returns>
    private List<Claim> GenerateClaims(ApplicationUser user)
    {
        var jwtSub = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["JwtRegisteredClaimNamesSub"];
        
        try
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwtSub ?? throw new ArgumentNullException(nameof(jwtSub))),                                               // Subject (ID of our app)
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),                            // Unique JWT identifier (so that it is not reused)
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()), // Issued at time
                new Claim(ClaimTypes.NameIdentifier, user.Id),                                                // User info stored in the token ...
                new Claim(ClaimTypes.Name, user.UserName ?? throw new ArgumentNullException(nameof(user.UserName))),
                new Claim(ClaimTypes.Email, user.Email ?? throw new ArgumentNullException(nameof(user.Email))),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            
            return claims;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    /// Creates signing credentials (for signing JWT token) using the symmetric security key configured in the app settings.
    /// </summary>
    /// <returns>The signing credentials.</returns>
    private SigningCredentials CreateSigningCredentials()
    {
        var symmetricSecurityKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["SymmetricSecurityKey"];
        
        return new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(symmetricSecurityKey ?? throw new ArgumentNullException(nameof(symmetricSecurityKey)))
            ),
            SecurityAlgorithms.HmacSha256
        );
    }
}
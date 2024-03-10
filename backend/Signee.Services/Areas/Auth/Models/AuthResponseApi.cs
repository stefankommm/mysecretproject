using System.Text.Json.Serialization;

namespace Signee.Services.Areas.Auth.Models;

public class AuthResponseApi
{
    [JsonPropertyName("username")]
    public string? Username { get; set; }
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    [JsonPropertyName("token")]
    public string? Token { get; set; }
}
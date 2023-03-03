using System.Text.Json;
using Keycloak.Admin.Core.Models;

namespace Keycloak.Admin.Core.Tests.Api.Ambient;

public static class FakeBearerToken
{
    public static StringContent TokenContent(string accessToken) => 
        new(JsonSerializer.Serialize(Token(accessToken)));
    
    public static StringContent TokenContent() => 
        new(JsonSerializer.Serialize(Token()));

    public static Token? Token() => Token("Test");
    public static Token? Token(string accessToken) => 
        new(accessToken, 0, 0, "0", "Bearer", 0, "", "openid");
}
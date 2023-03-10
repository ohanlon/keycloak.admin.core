using System.Text.Json.Serialization;
using Keycloak.Admin.Core.Exceptions;

namespace Keycloak.Admin.Core.Models;

public record Token(
    [property: JsonPropertyName("access_token")]
    string AccessToken,
    [property: JsonPropertyName("expires_in")]
    int ExpiresIn,
    [property: JsonPropertyName("refresh_expires_in")]
    int RefreshExpiresIn,
    [property: JsonPropertyName("refresh_token")]
    string RefreshToken,
    [property: JsonPropertyName("token_type")]
    string TokenType,
    [property: JsonPropertyName("not-before-policy")]
    int NotBeforePolicy,
    [property: JsonPropertyName("session_state")]
    string SessionState,
    [property: JsonPropertyName("scope")] string Scope
);

public static class BearerToken
{
    public static readonly Token? Empty = new Token("empty", 0, 0, "empty", "empty", 0, "empty", "empty");

    public static void AddTokenToRequestHeader(this Token? token, Request request)
    {
        if (token is null) throw new MissingTokenException();
        
        request.RequestHeaders ??= new();
        request.RequestHeaders.Add("Authorization", $"Bearer {token.AccessToken}");
    }
}
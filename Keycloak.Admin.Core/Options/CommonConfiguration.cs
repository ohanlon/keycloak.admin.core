namespace Keycloak.Admin.Core.Options;

/// <summary>
/// Provide a common access configuration capability to allow a developer to pick a default
/// configuration that they can reuse rather than having to specify the keycloak connection options,
/// realm key, and access key in every call.
/// </summary>
/// <param name="KeycloakConnectionOptions"></param>
/// <param name="RealmKey"></param>
/// <param name="AccessKey"></param>
public record CommonConfiguration(KeycloakConnectionOptions KeycloakConnectionOptions, string RealmKey,
    string AccessKey)
{
    public readonly KeycloakConnectionOptions KeycloakConnectionOptions = KeycloakConnectionOptions;
    public readonly string RealmKey = RealmKey;
    public readonly string AccessKey = AccessKey;
}
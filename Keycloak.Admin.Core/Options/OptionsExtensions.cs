using System.Diagnostics.CodeAnalysis;

namespace Keycloak.Admin.Core.Options;

[ExcludeFromCodeCoverage]
internal static class OptionsExtensions
{
    public static RealmOptions? GetRealm(this CommonConfiguration options) =>
        options.KeycloakConnectionOptions.Realms.FirstOrDefault(realmOptions => realmOptions.Key == options.RealmKey);

    public static RealmOptions? GetRealm(this KeycloakConnectionOptions options, string realmKey) =>
        options.Realms.FirstOrDefault(realmOptions => realmOptions.Key == realmKey);

    public static AuthenticationOptions? GetAuthenticationOptions(this RealmOptions realmOptions, string accessKey) =>
        realmOptions.AuthenticationOptions.FirstOrDefault(auth => auth.Key == accessKey);

    public static string Endpoint(this KeycloakConnectionOptions options) =>
        options.AuthorizationServerUrl.EndsWith("/")
            ? options.AuthorizationServerUrl
            : $"{options.AuthorizationServerUrl}/";

    public static string RealmEndpoint(this CommonConfiguration? options)
    {
        _ = options ?? throw new ArgumentNullException(nameof(options));
        var realm = options.KeycloakConnectionOptions.GetRealm(options.RealmKey);
        return
            $"{options.KeycloakConnectionOptions.Endpoint()}admin/realms/{realm.Realm}/";
    }

    public static string RealmEndpoint(this KeycloakConnectionOptions options, RealmOptions realm) =>
        $"{options.Endpoint()}admin/realms/{realm.Realm}/";
    
    public static string Endpoint(this CommonConfiguration? options, string endpoint) => $"{RealmEndpoint(options)}{endpoint}";

    public static string Endpoint(this KeycloakConnectionOptions? options, RealmOptions realm, string endpoint)
    {
        return $"{RealmEndpoint(options, realm)}{endpoint}";
    }
}

internal static class AuthenticationValidation
{
    public static bool IsValid(AuthenticationOptions? authenticationOptions)
    {
        switch (authenticationOptions?.AuthenticationType)
        {
            case AuthenticationType.Password:
                return !string.IsNullOrWhiteSpace(authenticationOptions?.Password?.Username) &&
                       !string.IsNullOrWhiteSpace(authenticationOptions?.Password?.Password);
            case AuthenticationType.ServiceAccount:
                return !string.IsNullOrWhiteSpace(authenticationOptions?.ServiceAccount?.ClientSecret);
            case null:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return false;
    }
}
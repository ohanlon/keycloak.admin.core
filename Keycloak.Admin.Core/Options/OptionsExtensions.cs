namespace Keycloak.Admin.Core.Options;

internal static class OptionsExtensions
{
    public static RealmOptions? GetRealm(this KeycloakConnectionOptions options, string realmKey) =>
        options.Realms.FirstOrDefault(realmOptions => realmOptions.Key == realmKey);

    public static AuthenticationOptions? GetAuthenticationOptions(this RealmOptions realmOptions, string accessKey) =>
        realmOptions.AuthenticationOptions.FirstOrDefault(auth => auth.Key == accessKey);
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
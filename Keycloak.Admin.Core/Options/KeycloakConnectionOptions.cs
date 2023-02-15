using System.ComponentModel.DataAnnotations;
using Keycloak.Admin.Core.Validations;
using Keycloak.Admin.Core.Support.Options;

namespace Keycloak.Admin.Core.Options;

public class KeycloakConnectionOptions : BaseOptions<KeycloakConnectionOptions>
{
    /// <summary>
    /// The base URL of the Keycloak server.
    /// </summary>
    /// <remarks>In Keycloak configuration, this maps to the auth-server-url</remarks>
    [Required, EnsureValidUriFormat]
    public string AuthorizationServerUrl { get; set; } = string.Empty;

    /// <summary>
    /// The <see cref="RealmOptions"/> for the server. This provides support for multiple realms.
    /// </summary>
    [Required, MinLength(1)]
    public RealmOptions[] Realms { get; set; } = Array.Empty<RealmOptions>();
}
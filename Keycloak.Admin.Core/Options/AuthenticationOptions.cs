using System.ComponentModel.DataAnnotations;
using Keycloak.Admin.Core.Validations;
using Keycloak.Admin.Core.Support.Options;

namespace Keycloak.Admin.Core.Options;

/// <summary>
/// The authentication options, used to retrieve the access token.
/// </summary>
[EnsureAuthenticationTypeMatches]
public class AuthenticationOptions : BaseOptions<AuthenticationOptions>
{
    /// <summary>
    /// The key to select this authentication options instance.
    /// </summary>
    [Required, MinLength(1)]
    public string Key { get; set; } = String.Empty;

    /// <summary>
    /// The type of authentication we are going to apply.
    /// </summary>
    [Required]
    public AuthenticationType AuthenticationType { get; set; } = AuthenticationType.ServiceAccount;
    public PasswordOptions? Password { get; set; }
    public ServiceAccountOptions? ServiceAccount { get; set; }
}
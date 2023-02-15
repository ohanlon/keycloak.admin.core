using System.ComponentModel.DataAnnotations;
using Keycloak.Admin.Core.Support.Options;

namespace Keycloak.Admin.Core.Options;

/// <summary>
/// The password options when the <see cref="AuthenticationOptions.AuthenticationType"/> is
/// set to <see cref="AuthenticationType.Password"/>.
/// </summary>
public class PasswordOptions : BaseOptions<PasswordOptions>
{
    /// <summary>
    /// The user name to authenticate against.
    /// </summary>
    [Required, MinLength(1)]
    public string? Username { get; set; } = string.Empty;

    /// <summary>
    /// The password of the user you want to authenticate with.
    /// </summary>
    [Required, MinLength(1)]
    public string? Password { get; set; } = string.Empty;
}
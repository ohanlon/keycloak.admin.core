using System.ComponentModel.DataAnnotations;
using Keycloak.Admin.Core.Support.Options;

namespace Keycloak.Admin.Core.Options;

/// <summary>
/// The details needed to authenticate against a service account when the
/// <see cref="AuthenticationOptions.AuthenticationType"/> is
/// set to <see cref="AuthenticationType.ServiceAccount"/>
/// </summary>
public class ServiceAccountOptions : BaseOptions<ServiceAccountOptions>
{
    /// <summary>
    /// The client secret for the service account.
    /// </summary>
    [Required]
    public string? ClientSecret { get; set; }
}
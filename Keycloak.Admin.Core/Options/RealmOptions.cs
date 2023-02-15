using System.ComponentModel.DataAnnotations;
using Keycloak.Admin.Core.Support.Options;

namespace Keycloak.Admin.Core.Options;

/// <summary>
/// The options for each Keycloak realm for a particular Keycloak instance.
/// </summary>
public class RealmOptions : BaseOptions<RealmOptions>
{
    /// <summary>
    /// Use the key to programatically retrieve the realm options.
    /// </summary>
    [Required, MinLength(1)]
    public string Key { get; set; } = string.Empty;
    
    /// <summary>
    /// The name of a particular Keycloak Realm. The name is important as it
    /// helps us form the appropriate requests to the server.
    /// </summary>
    [MinLength(1)]
    public string Realm { get; set; } = string.Empty;

    /// <summary>
    /// The required audience in a token. The client id is not automatically mapped into
    /// the audience field 'aud' of a token, so the audience is manually mapped in via a
    /// protocol mapper.
    /// </summary>
    /// <remarks>Maps to the resource setting.</remarks>
    public string Resource { get; set; } = string.Empty;

    /// <summary>
    /// Each Realm, in Keycloak, has an SSL mode associated with it. This defines the SSL/HTTPS needs
    /// of interacting with a particular realm. Anything that interacts with the realm must abide by
    /// the requirements defined in the SslRequired mode.
    /// </summary>
    public SslRequired SslRequired { get; set; } = SslRequired.External;

    /// <summary>
    /// By default, we want to verify that the audience for a token are valid. Where there is a high level
    /// of trust between services, or where we want to try out the behaviour in a test environment, we
    /// set this to false.
    /// </summary>
    /// <see href="https://www.keycloak.org/docs/latest/server_admin/#_audience"/>
    public bool VerifyTokenAudience { get; set; } = true;

    /// <summary>
    /// Is this a public client?
    /// </summary>
    public bool PublicClient { get; set; } = true;
    
    /// <summary>
    /// Does the realm need to use resource role mappings.
    /// </summary>
    public bool UseResourceRoleMappings { get; set; } = true;
    public int ConfidentialPort { get; set; }

    /// <summary>
    /// The authentication options allow us to generate the appropriate access tokens for performing our
    /// Keycloak operations.
    /// </summary>
    [Required, MinLength(1)]
    public AuthenticationOptions[] AuthenticationOptions { get; set; } = Array.Empty<AuthenticationOptions>();
}
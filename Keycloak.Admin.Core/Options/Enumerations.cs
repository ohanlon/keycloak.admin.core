namespace Keycloak.Admin.Core;

/// <summary>
/// The level of SSL support that Keycloak requires for requests.
/// </summary>
public enum SslRequired
{
    /// <summary>
    /// Users are able to interact with Keycloak without SSL support, as long as the IP address is a private
    /// one. The typical example of this would be something like http://localhost.
    /// </summary>
    External,
    /// <summary>
    /// Keycloak is not going to require SSL support.
    /// </summary>
    /// <remarks>Do not rely on this in production environments. It should only be used
    /// in development environments, although you should prefer to use External then.</remarks>
    None,
    /// <summary>
    /// All traffic requires SSL support.
    /// </summary>
    All
}

/// <summary>
/// The type of authentication to apply.
/// </summary>
public enum AuthenticationType
{
    /// <summary>
    /// The authentication flow will use password authentication.
    /// </summary>
    Password,
    /// <summary>
    /// The authentication flow will use client credentials.
    /// </summary>
    ServiceAccount
}
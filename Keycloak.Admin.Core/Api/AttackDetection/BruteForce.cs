using Keycloak.Admin.Core.Exceptions;
using Keycloak.Admin.Core.Options;

namespace Keycloak.Admin.Core.Api.AttackDetection;

/// <summary>
/// When there has been a brute force attack on a Keycloak server, users can be locked out. This API
/// provides the ability to manage the attack detection.
/// </summary>
public class BruteForce
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly Authorize _authorize;

    /// <summary>
    /// Instantiates a new instance of the BruteForce class.
    /// </summary>
    /// <param name="httpClientFactory">The HTTP Client factory</param>
    /// <param name="authorize">The <see cref="Authorize"/> instance used to generate the access token.</param>
    /// <exception cref="ArgumentNullException">Thrown if the parameters are null.</exception>
    public BruteForce(IHttpClientFactory? httpClientFactory, Authorize? authorize)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _authorize = authorize ?? throw new ArgumentNullException(nameof(authorize));
    }

    /// <summary>
    /// Clears the login failures for all users, releasing any temporarily disabled users.
    /// </summary>
    /// <remarks>In order to use this capability, the user must belong to the manage-users role, which
    /// can be set in the realm-management section. If the user does not have this role, this method will
    /// return false</remarks>
    /// <returns>True, if the brute force delete was successful, false otherwise.</returns>
    /// <param name="options">The <see cref="KeycloakConnectionOptions"/> containing the connection information.</param>
    /// <param name="realmKey">The key of the realm to pick up from the options.</param>
    /// <param name="accessKey">The key identifying which access item should be picked from the realm.</param>
    /// <returns>True, if the brute force delete was successful, false otherwise.</returns>
    public virtual async Task<bool> ClearLoginFailuresForAllUsers(KeycloakConnectionOptions options, string realmKey,
        string accessKey)
    {
        return await ClearLoginFailuresForAllUsers(new CommonConfiguration(options, realmKey, accessKey));
    }

    /// <summary>
    /// Clears the login failures for all users, releasing any temporarily disabled users.
    /// </summary>
    /// <remarks>In order to use this capability, the user must belong to the manage-users role, which
    /// can be set in the realm-management section. If the user does not have this role, this method will
    /// return false</remarks>
    /// <param name="options">The <see cref="CommonConfiguration"/> containing the connection options.</param>
    /// <returns>True, if the brute force delete was successful, false otherwise.</returns>
    public virtual async Task<bool> ClearLoginFailuresForAllUsers(CommonConfiguration? options)
    {
        var accessToken = await _authorize.GetAccessToken(options);
        if (accessToken == BearerToken.Empty)
            throw new MissingTokenException(); // Will change to throw an access token exception.
        using var deleteRequest = new DeleteRequest(_httpClientFactory);
        accessToken.AddTokenToRequestHeader(deleteRequest);
        var url = $"{options.RealmEndpoint()}attack-detection/brute-force/users";
        using var responseMessage = await deleteRequest.Execute(url);
        // Decode the response
        return responseMessage!.IsSuccessStatusCode;
    }

    /// <summary>
    /// Clears the login failures for a specific user, releasing a temporarily disabled user.
    /// </summary>
    /// <remarks>In order to use this capability, the user must belong to the manage-users role, which
    /// can be set in the realm-management section. If the user does not have this role, this method will
    /// return false</remarks>
    /// <returns>True, if the brute force delete was successful, false otherwise.</returns>
    /// <param name="options">The <see cref="KeycloakConnectionOptions"/> containing the connection information.</param>
    /// <param name="realmKey">The key of the realm to pick up from the options.</param>
    /// <param name="accessKey">The key identifying which access item should be picked from the realm.</param>
    /// <param name="user">The user id that needs to be unlocked.</param>
    /// <returns>True, if the brute force delete was successful, false otherwise.</returns>
    public virtual async Task<bool> ClearLoginFailuresForAllUsers(KeycloakConnectionOptions options, string realmKey,
        string accessKey, string user)
    {
        return await ClearLoginFailuresForAllUsers(new CommonConfiguration(options, realmKey, accessKey), user);
    }

    /// <summary>
    /// Clears the login failures for a specific user, releasing a temporarily disabled user.
    /// </summary>
    /// <remarks>In order to use this capability, the user must belong to the manage-users role, which
    /// can be set in the realm-management section. If the user does not have this role, this method will
    /// return false</remarks>
    /// <param name="options">The <see cref="CommonConfiguration"/> containing the connection options.</param>
    /// <param name="user">The user id that needs to be unlocked.</param>
    /// <returns>True, if the brute force delete was successful, false otherwise.</returns>
    public virtual async Task<bool> ClearLoginFailuresForAllUsers(CommonConfiguration? options, string user)
    {
        var accessToken = await _authorize.GetAccessToken(options);
        if (accessToken == BearerToken.Empty) return false; // Will change to throw an access token exception.
        using var deleteRequest = new DeleteRequest(_httpClientFactory);
        accessToken.AddTokenToRequestHeader(deleteRequest);
        var url = options.Endpoint($"attack-detection/brute-force/users/{user}");
        using var responseMessage = await deleteRequest.Execute(url);
        return responseMessage!.IsSuccessStatusCode;
    }

    public virtual async Task<object> LoginFailuresForUser(KeycloakConnectionOptions options, string realmKey,
        string accessKey, string user) =>
        await LoginFailuresForUser(new CommonConfiguration(options, realmKey, accessKey), user);

    public virtual async Task<object> LoginFailuresForUser(CommonConfiguration? options, string user)
    {
        var accessToken = await _authorize.GetAccessToken(options);
        if (accessToken == BearerToken.Empty) return new(); // Will change to throw an access token exception.
        using var getRequest = new GetRequest(_httpClientFactory);
        accessToken.AddTokenToRequestHeader(getRequest);
        var url = options.Endpoint($"attack-detection/brute-force/users/{user}");
        using var responseMessage = await getRequest.Execute(url);
        // Decode the response
        return responseMessage!.Content;
    }
}
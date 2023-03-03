using Keycloak.Admin.Core.Options;

namespace Keycloak.Admin.Core.Api;

/// <summary>
/// Authorize using either a client secret, or password based authentication, as provided in the <see cref="KeycloakConnectionOptions"/>
/// </summary>
public class Authorize
{
    private readonly IHttpClientFactory _httpClientFactory;

    /// <summary>
    /// Instantiate a new instance of the Authorize class.
    /// </summary>
    /// <param name="httpClientFactory">The HTTP client factory.</param>
    /// <exception cref="ArgumentNullException">Thrown if the client factory is null.</exception>
    public Authorize(IHttpClientFactory? httpClientFactory)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }

    /// <summary>
    /// Get the access token using the <see cref="KeycloakConnectionOptions"/> for the relevant realm and access key.
    /// </summary>
    /// <param name="options">The <see cref="KeycloakConnectionOptions"/> containing the connection information.</param>
    /// <param name="realmKey">The key of the realm to pick up from the options.</param>
    /// <param name="accessKey">The key identifying which access item should be picked from the realm.</param>
    public virtual async Task<Token?> GetAccessToken(KeycloakConnectionOptions options, string realmKey,
        string accessKey) =>
        await GetAccessToken(new CommonConfiguration(options, realmKey, accessKey));

    /// <summary>
    /// Get the access token using the <see cref="KeycloakConnectionOptions"/> for the relevant realm and access key.
    /// </summary>
    /// <param name="options">The <see cref="CommonConfiguration"/> containing the connection options.</param>
    public virtual async Task<Token?> GetAccessToken(CommonConfiguration? options)
    {
        if (options is null) throw new ArgumentNullException(nameof(options));

        var realm = options.KeycloakConnectionOptions.GetRealm(options.RealmKey);
        if (realm is null) return BearerToken.Empty;
        var authenticationOptions =
            realm.GetAuthenticationOptions(options.AccessKey);

        if (!AuthenticationValidation.IsValid(authenticationOptions)) return BearerToken.Empty;
        switch (authenticationOptions)
        {
            case {AuthenticationType: AuthenticationType.ServiceAccount, ServiceAccount.ClientSecret: { }}:
                return await GetAccessToken(options.KeycloakConnectionOptions.Endpoint(), options.RealmKey,
                    realm!.Resource,
                    authenticationOptions.ServiceAccount.ClientSecret);
            default:
                return await GetAccessToken(options.KeycloakConnectionOptions.Endpoint(), options.RealmKey,
                    realm!.Resource,
                    authenticationOptions?.Password?.Username ?? string.Empty,
                    authenticationOptions?.Password?.Password ?? string.Empty);
        }
    }

    private async Task<Token?> GetAccessToken(string endpoint, string realm, string audience, string userName,
        string password)
    {
        var url = $"{endpoint}realms/{realm}/protocol/openid-connect/token";
        using var postRequest = new PostRequest(_httpClientFactory)
        {
            FormData = new()
        };
        postRequest.FormData.Add("client_id", audience);
        postRequest.FormData.Add("username", userName);
        postRequest.FormData.Add("password", password);
        postRequest.FormData.Add("grant_type", "password");
        using var responseMessage = await postRequest.Execute(url);
        return await responseMessage.Deserialize(BearerToken.Empty);
    }

    private async Task<Token?> GetAccessToken(string endpoint, string realm, string audience, string clientSecret)
    {
        if (!endpoint.EndsWith("/"))
        {
            endpoint += ("/");
        }

        var url = $"{endpoint}realms/{realm}/protocol/openid-connect/token";
        using var postRequest = new PostRequest(_httpClientFactory)
        {
            FormData = new()
        };
        postRequest.FormData.Add("client_id", audience);
        postRequest.FormData.Add("client_secret", clientSecret);
        postRequest.FormData.Add("grant_type", "client_credentials");
        using var responseMessage = await postRequest.Execute(url);
        return await responseMessage.Deserialize(BearerToken.Empty);
    }
}
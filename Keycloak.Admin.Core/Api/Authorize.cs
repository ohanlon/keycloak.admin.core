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
    /// <param name="httpClientFactory">The HTTP client.</param>
    public Authorize(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// Get the access token using the <see cref="KeycloakConnectionOptions"/> for the relevant realm and access key.
    /// </summary>
    /// <param name="options">The <see cref="KeycloakConnectionOptions"/> containing the connection information.</param>
    /// <param name="realmKey">The key of the realm to pick up from the options.</param>
    /// <param name="accessKey">The key identifying which access item should be picked from the realm.</param>
    public virtual async Task<Token?> GetAccessToken(KeycloakConnectionOptions options, string realmKey,
        string accessKey) =>
        await GetAccessToken(new RealmAccessConfiguration(options, realmKey, accessKey));

    /// <summary>
    /// Get the access token using the <see cref="KeycloakConnectionOptions"/> for the relevant realm and access key.
    /// </summary>
    /// <param name="options">The <see cref="RealmAccessConfiguration"/> containing the connection options.</param>
    public virtual async Task<Token?> GetAccessToken(RealmAccessConfiguration options)
    {
        var realm = options.KeycloakConnectionOptions.GetRealm(options.RealmKey);
        if (realm is null) return BearerToken.Empty;
        var authenticationOptions =
            realm.GetAuthenticationOptions(options.AccessKey);

        if (!AuthenticationValidation.IsValid(authenticationOptions)) return BearerToken.Empty;
        switch (authenticationOptions)
        {
            case {AuthenticationType: AuthenticationType.ServiceAccount, ServiceAccount.ClientSecret: { }}:
                return await GetAccessToken(options.KeycloakConnectionOptions.Endpoint(), options.RealmKey, realm!.Resource,
                    authenticationOptions.ServiceAccount.ClientSecret);
            case {AuthenticationType: AuthenticationType.Password, Password: { }}:
                return await GetAccessToken(options.KeycloakConnectionOptions.Endpoint(), options.RealmKey, realm!.Resource,
                    authenticationOptions?.Password?.Username, authenticationOptions?.Password?.Password);
        }

        return BearerToken.Empty;
    }

    private async Task<Token?> GetAccessToken(string endpoint, string realm, string audience, string userName, string password)
    {
        string url = $"{endpoint}realms/{realm}/protocol/openid-connect/token";
        using PostRequest postRequest = new PostRequest(_httpClientFactory)
        {
            FormData = new()
        };
        postRequest.FormData.Add("client_id", audience);
        postRequest.FormData.Add("username", userName);
        postRequest.FormData.Add("password", password);
        postRequest.FormData.Add("grant_type", "password");
        using HttpResponseMessage? responseMessage = await postRequest.Execute(url);
        return await responseMessage.Deserialize(BearerToken.Empty);
    }

    private async Task<Token?> GetAccessToken(string endpoint, string realm, string audience, string clientSecret)
    {
        if (!endpoint.EndsWith("/"))
        {
            endpoint += ("/");
        }

        string url = $"{endpoint}realms/{realm}/protocol/openid-connect/token";
        using PostRequest postRequest = new PostRequest(_httpClientFactory)
        {
            FormData = new()
        };
        postRequest.FormData.Add("client_id", audience);
        postRequest.FormData.Add("client_secret", clientSecret);
        postRequest.FormData.Add("grant_type", "client_credentials");
        using HttpResponseMessage? responseMessage = await postRequest.Execute(url);
        return await responseMessage.Deserialize(BearerToken.Empty);
    }
}
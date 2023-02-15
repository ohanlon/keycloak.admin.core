using Keycloak.Admin.Core.Models;
using Keycloak.Admin.Core.Options;
using Keycloak.Admin.Core.Support.Http;

namespace Keycloak.Admin.Core.Authentication;

/// <summary>
/// Authorize using either a client secret, or password based authentication, as provided in the <see cref="KeycloakConnectionOptions"/>
/// </summary>
public class Authorize
{
    private HttpClient _httpClient;

    /// <summary>
    /// Instantiate a new instance of the Authorize class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    public Authorize(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Get the access token using the <see cref="KeycloakConnectionOptions"/> for the 
    /// </summary>
    /// <param name="options">The <see cref="KeycloakConnectionOptions"/> containing the realm information.</param>
    /// <param name="realmKey">The realm in the options list that contains the information needed to retrieve the access token.</param>
    /// <param name="accessKey">In each realm, we may have multiple authentication options. The key chooses which one we need to apply.</param>
    public virtual async Task<Token> GetAccessToken(KeycloakConnectionOptions options, string realmKey, string accessKey)
    {
        var realm = options.GetRealm(realmKey);
        if (realm is null) return EmptyToken.Empty;
        HttpResponseMessage? responseMessage = null;
        var authenticationOptions =
            realm.GetAuthenticationOptions(accessKey);

        if (!AuthenticationValidation.IsValid(authenticationOptions)) return EmptyToken.Empty;
        switch (authenticationOptions)
        {
            case null:
                return EmptyToken.Empty;
            case {AuthenticationType: AuthenticationType.ServiceAccount, ServiceAccount.ClientSecret: { }}:
                responseMessage = await GetAccessToken(options.AuthorizationServerUrl, realmKey, realm!.Resource,
                    authenticationOptions.ServiceAccount.ClientSecret);
                break;
            case {AuthenticationType: AuthenticationType.Password, Password: { }}:
                responseMessage = await GetAccessToken(options.AuthorizationServerUrl, realmKey, realm!.Resource,
                    authenticationOptions?.Password?.Username, authenticationOptions?.Password?.Password);
                break;
                
        }

        return await responseMessage.Deserialize(EmptyToken.Empty);
    }

    private async Task<HttpResponseMessage?> GetAccessToken(string endpoint, string realm, string audience, string userName, string password)
    {
        if (!endpoint.EndsWith("/"))
        {
            endpoint += ("/");
        }

        string url = $"{endpoint}realms/{realm}/protocol/openid-connect/token";
        PostRequest postRequest = new PostRequest(_httpClient)
        {
            FormData = new()
        };
        postRequest.FormData.Add("client_id", audience);
        postRequest.FormData.Add("username", userName);
        postRequest.FormData.Add("password", password);
        postRequest.FormData.Add("grant_type", "password");
        //postRequest.FormData.Add("client_secret", "42ed0fe5-634b-4d0d-84cd-0bf21ef3a6f5");
        return await postRequest.Execute(url);
    }

    private async Task<HttpResponseMessage?> GetAccessToken(string endpoint, string realm, string audience, string clientSecret)
    {
        if (!endpoint.EndsWith("/"))
        {
            endpoint += ("/");
        }

        string url = $"{endpoint}realms/{realm}/protocol/openid-connect/token";
        PostRequest postRequest = new PostRequest(_httpClient)
        {
            FormData = new()
        };
        postRequest.FormData.Add("client_id", audience);
        postRequest.FormData.Add("client_secret", clientSecret);
        postRequest.FormData.Add("grant_type", "client_credentials");
        return await postRequest.Execute(url);
    }
}
using System.Net;
using Keycloak.Admin.Core.Api;
using Keycloak.Admin.Core.Models;
using Keycloak.Admin.Core.Options;
using Keycloak.Admin.Core.Tests.Api.Ambient;

namespace Keycloak.Admin.Core.Tests.Api;

public class AuthorizeTests
{
    [Fact]
    public void NullException_Is_Thrown_When_No_Client_Factory_Is_Passed_In()
    {
        Assert.Throws<ArgumentNullException>(() => _ = new Authorize(null));
    }

    [Fact]
    public void NullException_Is_Thrown_When_RealmAccessConfiguration_Is_Null()
    {
        var responseMessage = new HttpResponseMessage();
        var authorize = new Authorize(responseMessage.HttpClientFactory());
        Assert.ThrowsAsync<ArgumentNullException>(() => authorize.GetAccessToken(null));
    }

    [Fact]
    public async Task When_Realm_Not_Found_Empty_BearerToken_Is_Returned()
    {
        var responseMessage = new HttpResponseMessage();
        var authorize = new Authorize(responseMessage.HttpClientFactory());
        Assert.Equal(BearerToken.Empty,
            await authorize.GetAccessToken(DefaultKeycloak(), "test1", "test"));
    }

    
    [Fact]
    public async Task When_AuthenticationOptions_Not_Found_Empty_BearerToken_Is_Returned()
    {
        var responseMessage = new HttpResponseMessage();
        var authorize = new Authorize(responseMessage.HttpClientFactory());
        Assert.Equal(BearerToken.Empty,
            await authorize.GetAccessToken(DefaultKeycloak(), "test", "test1"));
    }

    [Fact]
    public async Task When_AuthenticationOptions_Set_Then_AccessToken_Is_Returned()
    {
        var responseMessage = new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.OK,
            Content = FakeBearerToken.TokenContent()
        };
        var authorize = new Authorize(responseMessage.HttpClientFactory());
        var token = await authorize.GetAccessToken(DefaultKeycloak(), "test", "test");
        Assert.Equal("Test", token!.AccessToken);
    }
    
    private KeycloakConnectionOptions DefaultKeycloak()
    {
        var realm = new RealmOptions
        {
            Key = "test",
            Realm = "test",
            AuthenticationOptions = new[]
            {
                new AuthenticationOptions
                {
                    AuthenticationType = AuthenticationType.Password,
                    Key = "test",
                    Password = new PasswordOptions
                    {
                        Password = "test",
                        Username = "test"
                    }
                }
            }
        };

        return new KeycloakConnectionOptions
        {
            AuthorizationServerUrl = "http://localhost",
            Realms = new[] {realm}
        };
    }
}
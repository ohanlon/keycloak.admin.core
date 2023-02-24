using Keycloak.Admin.Core.Api;
using Keycloak.Admin.Core.Models;
using Keycloak.Admin.Core.Options;

namespace Keycloak.Admin.Core.Tests.Api.Ambient;

public class FakeAuthorize : Authorize
{
    public Token AccessToken = FakeBearerToken.Token("Bearer test");
    public FakeAuthorize() : base(new FakeHttpClientFactory())
    {
    }

    public override Task<Token?> GetAccessToken(CommonConfiguration options)
    {
        return Task.FromResult(AccessToken);
    }

    public override Task<Token?> GetAccessToken(KeycloakConnectionOptions options, string realmKey, string accessKey)
    {
        return Task.FromResult(AccessToken);
    }
}
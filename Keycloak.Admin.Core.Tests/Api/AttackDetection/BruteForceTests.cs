using Keycloak.Admin.Core.Api.AttackDetection;
using Keycloak.Admin.Core.Exceptions;
using Keycloak.Admin.Core.Models;
using Keycloak.Admin.Core.Tests.Api.Ambient;
using Moq;

namespace Keycloak.Admin.Core.Tests.Api.AttackDetection;

public class BruteForceTests
{
    [Fact]
    public void Given_Missing_HttpClientFactory_When_AttackDetection_Instantiated_Then_ArgumentNullException_Is_Thrown()
    {
        Assert.Throws<ArgumentNullException>(() => _ = new BruteForce(null, new FakeAuthorize()));
    }
    
    [Fact]
    public void Given_Missing_Authorize_When_AttackDetection_Instantiated_Then_ArgumentNullException_Is_Thrown()
    {
        Assert.Throws<ArgumentNullException>(() => _ = new BruteForce(new Mock<IHttpClientFactory?>().Object, null));
    }

    [Fact]
    public void Given_BruteForceDelete_When_AccessToken_Cannot_Be_Generated_Then()
    {
        var factory = new Mock<IHttpClientFactory>().Object;
        var fakeAuthorize = new FakeAuthorize
        {
            AccessToken = BearerToken.Empty
        };
        var attackDetection = new BruteForce(factory, fakeAuthorize);
        Assert.ThrowsAsync<MissingTokenException>(() => _ = attackDetection.ClearLoginFailuresForAllUsers(FakeConfiguration.Fake()));
    }
}
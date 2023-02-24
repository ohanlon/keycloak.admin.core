namespace Keycloak.Admin.Core.Tests.Api.Ambient;

public class FakeHttpClientFactory : IHttpClientFactory
{
    public HttpClient CreateClient(string name) => new();
}
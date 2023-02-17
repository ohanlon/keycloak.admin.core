namespace Keycloak.Admin.Core.Support.Http;

public class PostRequest : Request
{
    public PostRequest(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }

    protected override async Task<HttpResponseMessage?> Execute(Endpoint endpoint) =>
        await Client.PostAsync(endpoint.Address, FormData!.Apply());
}
namespace Keycloak.Admin.Core.Support.Http;

public class PostRequest : Request
{
    public PostRequest(HttpClient httpClient) : base(httpClient)
    {
    }

    protected override async Task<HttpResponseMessage?> Execute(Endpoint endpoint) =>
        await Client.PostAsync(endpoint.Address, FormData!.Apply());
}
namespace Keycloak.Admin.Core.Support.Http;

public class DeleteRequest : Request
{
    public DeleteRequest(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }

    protected override async Task<HttpResponseMessage?> Execute(Endpoint endpoint)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Delete, endpoint.Address);
        if (FormData != null)
        {
            requestMessage.Content = FormData!.Apply();
        }

        return await Client.SendAsync(requestMessage, CancellationToken.None);
    }
}
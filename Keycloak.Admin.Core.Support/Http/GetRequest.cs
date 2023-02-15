namespace Keycloak.Admin.Core.Support.Http;

/// <summary>
/// All GET requests use this class, standardising the client access throughout the codebase.
/// </summary>
public class GetRequest : Request
{
    /// <summary>
    /// Instantiate a new instance of the GetRequest class, passing in a <see cref="HttpClient"/> instance.
    /// </summary>
    /// <param name="client">The HttpClient instance to execute the Get request.</param>
    public GetRequest(HttpClient client) : base(client)
    {
    }

    protected override async Task<HttpResponseMessage?> Execute(Endpoint endpoint) =>
        await Client.GetAsync(endpoint.Address);
}
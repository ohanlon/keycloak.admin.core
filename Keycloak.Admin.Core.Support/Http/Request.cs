namespace Keycloak.Admin.Core.Support.Http;

public abstract class Request : IDisposable
{
    /// <summary>
    /// Instantiate a new instance of the Request class.
    /// </summary>
    /// <param name="client">The HTTP client to create the connection.</param>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="client"/> is not supplied.</exception>
    protected Request(IHttpClientFactory client)
    {
        _ = client ?? throw new ArgumentNullException(nameof(client));
        Client = client.CreateClient();
    }

    /// <summary>
    /// Execute the HTTP operation using the endpoint and <see cref="QueryString"/>.
    /// </summary>
    /// <param name="endpoint">The URL endpoint to execute.</param>
    /// <returns>The awaitable <see cref="HttpResponseMessage"/></returns>.
    public Task<HttpResponseMessage?> Execute(string endpoint)
    {
        RequestHeaders?.Apply(Client);
        return Execute(new Endpoint(endpoint, QueryString));
    }

    /// <summary>
    /// Any query string parameters that we need to execute against the code.
    /// </summary>
    public QueryString? QueryString { get; set; }

    /// <summary>
    /// Optionally, any request headers that we wish to add to the HTTP request.
    /// </summary>
    public RequestHeaders? RequestHeaders { get; set; } = new();

    public FormData? FormData { get; set; }

    protected HttpClient Client { get; }

    protected abstract Task<HttpResponseMessage?> Execute(Endpoint endpoint);

    public void Dispose()
    {
        Client?.Dispose();
    }
}
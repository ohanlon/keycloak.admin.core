namespace Keycloak.Admin.Core.Support.Http;

public class Endpoint
{
    public Endpoint(string endpoint, QueryString? queryString = null)
    {
        if (queryString != null)
        {
            endpoint = queryString.Transform(endpoint);
        }

        if (!IsHttpFormatEndpoint(endpoint))
        {
            throw new ArgumentException(null, nameof(endpoint));
        }

        Address = endpoint;
    }

    public string Address { get; }

    private bool IsHttpFormatEndpoint(string endpoint) => HttpFormatEndpoint.IsValid(endpoint);
}
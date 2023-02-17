namespace Keycloak.Admin.Core.Support.Http;

public class RequestHeaders
{
    private readonly Dictionary<string, string> _headers = new();

    public void Add(string key, string value)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("You must supply the header name.");

        _headers[key] = value;
    }

    public void Apply(HttpClient httpClientFactory)
    {
        httpClientFactory.DefaultRequestHeaders.Clear();
        foreach (var header in _headers)
        {
            httpClientFactory.DefaultRequestHeaders.Add(header.Key, header.Value);
        }
    }
}
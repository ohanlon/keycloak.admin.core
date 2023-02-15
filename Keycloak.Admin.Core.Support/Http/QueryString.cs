namespace Keycloak.Admin.Core.Support.Http;

public class QueryString
{
    private readonly Dictionary<string, string> _queryStringParameters = new Dictionary<string, string>();

    public void Add(string key, string value)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("You must supply a key for this query string parameter.");
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("You must supply a value for this query string parameter.");

        _queryStringParameters[key] = value;
    }

    public string Transform(string url)
    {
        return _queryStringParameters.Aggregate(url,
            (current, parameter) =>
                current.Replace($"{{{parameter.Key}}}", parameter.Value, StringComparison.OrdinalIgnoreCase));
    }
}
namespace Keycloak.Admin.Core.Support.Http;

public class FormData
{
    private readonly Dictionary<string, string> _formData = new();

    public void Add(string key, string value)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("You must supply the header name.");

        _formData[key] = value;
    }

    public FormUrlEncodedContent Apply() => new(_formData.ToArray());
}
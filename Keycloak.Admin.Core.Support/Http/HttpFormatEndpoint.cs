namespace Keycloak.Admin.Core.Support.Http;

public static class HttpFormatEndpoint
{
    public static bool IsValid(string endpoint) => Uri.TryCreate(endpoint, UriKind.Absolute, out var uri)
                                                  && uri.IsWellFormedOriginalString()
                                                  && (uri.Scheme == Uri.UriSchemeHttp ||
                                                      uri.Scheme == Uri.UriSchemeHttps);
}
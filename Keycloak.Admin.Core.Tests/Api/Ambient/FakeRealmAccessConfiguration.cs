using Keycloak.Admin.Core.Options;

namespace Keycloak.Admin.Core.Tests.Api.Ambient;

public static class FakeConfiguration
{
    public static CommonConfiguration? Fake() => 
        new(new(), string.Empty, string.Empty);
}
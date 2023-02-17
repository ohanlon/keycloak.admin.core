using Keycloak.Admin.Core.Api;
using Keycloak.Admin.Core.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Keycloak.Admin.Core;

public static class Registration
{
    public static void RegisterKeycloakServices(this IServiceCollection serviceCollection) =>
        serviceCollection.AddTransient<Authorize>().AddTransient<AttackDetection>();

    public static KeycloakConnectionOptions GetKeycloakConnectionOptions(this ConfigurationManager configurationManager,
        string keycloakSection) =>
        configurationManager.GetSection(keycloakSection).Get<KeycloakConnectionOptions>();
}
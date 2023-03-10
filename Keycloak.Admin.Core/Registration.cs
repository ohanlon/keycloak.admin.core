using System.Diagnostics.CodeAnalysis;
using Keycloak.Admin.Core.Api;
using Keycloak.Admin.Core.Api.AttackDetection;
using Keycloak.Admin.Core.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Keycloak.Admin.Core;

[ExcludeFromCodeCoverage]
public static class Registration
{
    public static void RegisterKeycloakServices(this IServiceCollection serviceCollection) =>
        serviceCollection.AddTransient<Authorize>().AddTransient<BruteForce>();

    public static KeycloakConnectionOptions GetKeycloakConnectionOptions(this ConfigurationManager configurationManager,
        string keycloakSection) =>
        configurationManager.GetSection(keycloakSection).Get<KeycloakConnectionOptions>();
}
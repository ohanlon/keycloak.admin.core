using Keycloak.Admin.Core;
using Keycloak.Admin.Core.Authentication;
using Keycloak.Admin.Core.Models;
using Keycloak.Admin.Core.Options;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient(); 
builder.Services.AddOptions<KeycloakConnectionOptions>()
    .BindConfiguration("keycloak").
    ValidateDataAnnotations().
    ValidateOnStart();

builder.Services.RegisterKeycloakServices();
builder.Services.AddSingleton(r => r.GetRequiredService<IOptions<KeycloakConnectionOptions>>().Value);
var app = builder.Build();

app.MapGet("/", () => "Hello World");

app.MapGet("/token", async (Authorize authorize) =>
{
    var ko = builder.Configuration.GetKeycloakConnectionOptions("keycloak");
    Token token = await authorize.GetAccessToken(ko, "CP", "Master");
    return Results.Ok(token);
});

app.Run();
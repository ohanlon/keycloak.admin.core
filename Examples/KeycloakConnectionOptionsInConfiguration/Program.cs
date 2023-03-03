/*
 In this example, I have created a local instance of Keycloak running on Port 8080. I configured this instance with
 a new realm called CP (in the settings, I used the same key for the realm as the realm name, but I could have given
 the key any unique value). I then added a new user called pete, who has a password of pete.
 Finally, I added a Client called CP-Test. These values are used inside the JSON file to demonstrate Keycloak 
 configuration.
 */

using Keycloak.Admin.Core;
using Keycloak.Admin.Core.Api;
using Keycloak.Admin.Core.Api.AttackDetection;
using Keycloak.Admin.Core.Models;
using Keycloak.Admin.Core.Options;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();
builder.Services.AddOptions<KeycloakConnectionOptions>()
    .BindConfiguration("keycloak").ValidateDataAnnotations().ValidateOnStart();

builder.Services.RegisterKeycloakServices();
builder.Services.AddSingleton(r => r.GetRequiredService<IOptions<KeycloakConnectionOptions>>().Value);
var app = builder.Build();

app.MapGet("/", () => "Hello World");
var ko = builder.Configuration.GetKeycloakConnectionOptions("keycloak");

app.MapGet("/token", async (Authorize authorize) =>
{
    var token = await authorize.GetAccessToken(ko, "CP", "Master");
    return Results.Ok(token);
});

app.MapGet("/bruteforce",
    async (BruteForce attackDetection) => await attackDetection.ClearLoginFailuresForAllUsers(ko, "CP", "Master"));

app.MapGet("/bruteforcestatus",
    async (BruteForce attackDetection) =>
        await attackDetection.LoginFailuresForUser(ko, "CP", "Master", "Peter"));
app.Run();
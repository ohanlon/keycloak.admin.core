using Keycloak.Admin.Core.Options;

namespace Keycloak.Admin.Core.Tests;

public class KeycloakConnectionOptionsTests
{
    [Fact]
    public void SslRequired_Should_Be_External_By_Default()
    {
        var options = new RealmOptions();

        Assert.Equal(Admin.Core.SslRequired.External, options.SslRequired);
    }
    
    [Fact]
    public void VerifyTokenAudience_Should_Be_True_By_Default()
    {
        var options = new RealmOptions();

        Assert.True(options.VerifyTokenAudience);
    }

    [Fact]
    public void VerifyPublicClient_Should_Be_True_By_Default()
    {
        var options = new RealmOptions();

        Assert.True(options.PublicClient);
    }
    
    [Fact]
    public void VerifyUseResourceRoleMappings_Should_Be_True_By_Default()
    {
        var options = new RealmOptions();

        Assert.True(options.UseResourceRoleMappings);
    }
    
    [Fact]
    public void VerifyConfidentialPort_Should_Be_Zero_By_Default()
    {
        var options = new RealmOptions();

        Assert.Equal(0, options.ConfidentialPort);
    }

    
    [Fact]
    public void Realm_Should_Not_Be_EmptyString()
    {
        RealmOptions realmOptions = new() { };
        var options = new KeycloakConnectionOptions { AuthorizationServerUrl = "https://www.example.com", Realms = new[]{ realmOptions }};

        var context = new ValidationContext(options, null, null);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(options, context, results, true);

        Assert.False(isValid);
    }

    [Fact]
    public void Realm_Should_Not_Be_Empty()
    {
        var options = new KeycloakConnectionOptions { AuthorizationServerUrl = "https://www.example.com" };

        var context = new ValidationContext(options, null, null);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(options, context, results, true);

        Assert.False(isValid);
    }

    [Fact]
    public void AuthorizationServerUrl_Should_Be_Valid_Uri_Format()
    {
        var options = DefaultKeycloak();
        var context = new ValidationContext(options, null, null);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(options, context, results, true);

        Assert.True(isValid);
    }
    
    private KeycloakConnectionOptions DefaultKeycloak()
    {
        RealmOptions realm = new RealmOptions
        {
            Key = "test",
            Realm = "test",
            AuthenticationOptions = new[]
            {
                new AuthenticationOptions
                {
                    AuthenticationType = AuthenticationType.Password,
                    Key = "test",
                    Password = new PasswordOptions
                    {
                        Password = "test",
                        Username = "test"
                    }
                }
            }
        };

        return new KeycloakConnectionOptions
        {
            AuthorizationServerUrl = "http://www.example.com",
            Realms = new[] {realm}
        };
    }
    
    [Fact]
    public void AuthorizationServerUrl_Should_Not_Be_Valid_Uri_Format_WhenAddressIsNotUrl()
    {
        var realm = new RealmOptions {Realm = "test"};
        var options = new KeycloakConnectionOptions { AuthorizationServerUrl = "https://.example.com", Realms = new[]{ realm }};

        var context = new ValidationContext(options, null, null);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(options, context, results, true);

        Assert.False(isValid);
    }
}
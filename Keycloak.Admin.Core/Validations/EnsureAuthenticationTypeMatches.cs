using System.ComponentModel.DataAnnotations;
using Keycloak.Admin.Core.Options;

namespace Keycloak.Admin.Core.Validations;

[AttributeUsage(AttributeTargets.Class)]
public class EnsureAuthenticationTypeMatches : ValidationAttribute
{
    public EnsureAuthenticationTypeMatches() : 
        base("When AuthenticationOptions is Password, the Password section must be provided, else the ServiceAccount must be provided.")
    {
    }
    public override bool IsValid(object? value)
    {
        if (value == null) return false;
        var options = value as AuthenticationOptions;
        if (options is {AuthenticationType: AuthenticationType.Password})
        {
            return options.Password != null;
        }

        return options?.ServiceAccount != null;
    }
}
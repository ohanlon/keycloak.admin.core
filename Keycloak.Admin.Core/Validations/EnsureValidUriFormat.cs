using System.ComponentModel.DataAnnotations;
using Keycloak.Admin.Core.Support.Http;

namespace Keycloak.Admin.Core.Validations;

[AttributeUsage(AttributeTargets.Property)]
public class EnsureValidUriFormat : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        return value != null && HttpFormatEndpoint.IsValid(value.ToString());
    }
}
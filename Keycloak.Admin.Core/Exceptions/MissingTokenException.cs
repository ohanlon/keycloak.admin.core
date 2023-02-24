using System.Diagnostics.CodeAnalysis;

namespace Keycloak.Admin.Core.Exceptions;

[ExcludeFromCodeCoverage]
public class MissingTokenException : Exception
{
    public MissingTokenException() : base("The generated access token is not present.")
    {
    }
}
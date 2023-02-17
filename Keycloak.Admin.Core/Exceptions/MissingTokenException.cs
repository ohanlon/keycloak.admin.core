namespace Keycloak.Admin.Core.Exceptions;

public class MissingTokenException : Exception
{
    public MissingTokenException() : base("The generated access token is not present.")
    {
    }
}
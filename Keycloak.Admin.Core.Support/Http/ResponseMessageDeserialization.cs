using System.Net;
using System.Text.Json;

namespace Keycloak.Admin.Core.Support.Http;

public static class ResponseMessageDeserialization
{
    public static async Task<T?> Deserialize<T>(this HttpResponseMessage responseMessage)
    {
        if (responseMessage is {IsSuccessStatusCode: true} && 
            responseMessage.StatusCode != HttpStatusCode.NoContent)
        {
            // Deserialize the response.
            T instance = await JsonSerializer.DeserializeAsync<T>(await responseMessage.Content.ReadAsStreamAsync());
            responseMessage.Dispose();
            return instance;
        }

        return default;
    }
    
    public static async Task<T?> Deserialize<T>(this HttpResponseMessage? responseMessage, T? empty)
    {
        if (responseMessage is {IsSuccessStatusCode: true} && 
            responseMessage.StatusCode != HttpStatusCode.NoContent)
        {
            // Deserialize the response.
            T instance = await JsonSerializer.DeserializeAsync<T>(await responseMessage.Content.ReadAsStreamAsync());
            responseMessage.Dispose();
            return instance;
        }

        return empty;
    }
}
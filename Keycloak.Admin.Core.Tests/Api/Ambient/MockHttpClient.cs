using Moq;
using Moq.Protected;

namespace Keycloak.Admin.Core.Tests.Api.Ambient;

internal static class MockHttpClient
{
    public static IHttpClientFactory HttpClientFactory(this HttpResponseMessage responseMessage) =>
        MockHttpClientFactory(responseMessage).Object;

    public static Mock<IHttpClientFactory> MockHttpClientFactory(this HttpResponseMessage responseMessage)
    {
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected().Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()).ReturnsAsync(responseMessage);
        var factory = new Mock<IHttpClientFactory>();
        factory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient(handler.Object));
        return factory;
    }
}
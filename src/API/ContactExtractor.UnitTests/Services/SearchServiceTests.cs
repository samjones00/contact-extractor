using System.Net;
using ContactExtractor.Core.Services;
using FluentAssertions;
using Moq;
using Moq.Protected;

namespace ContactExtractor.UnitTests.Services;

public class SearchServiceTests
{
    private Mock<IHttpClientFactory> _httpClientFactory;

    public SearchServiceTests()
    {
        _httpClientFactory = new Mock<IHttpClientFactory>();
    }

    [Test]
    public async Task Search_Returns_ResponseBody_When_200()
    {
        // Arrange
        const string expectedBody = "<html>some results</html>";
        var httpClient = BuildHttpClient(HttpStatusCode.OK, expectedBody);
        _httpClientFactory.Setup(x => x.CreateClient(nameof(SearchService))).Returns(httpClient);
        var sut = new SearchService(_httpClientFactory.Object);

        // Act
        var result = await sut.Search("London", CancellationToken.None);

        // Assert
        result.Should().Be(expectedBody);
    }

    [Test]
    public async Task Search_Throws_HttpRequestException_When_400()
    {
        // Arrange
        var httpClient = BuildHttpClient(HttpStatusCode.BadRequest);
        _httpClientFactory.Setup(x => x.CreateClient(nameof(SearchService))).Returns(httpClient);
        var sut = new SearchService(_httpClientFactory.Object);

        // Act
        var act = () => sut.Search("London", CancellationToken.None);

        // Act & Assert
        await act.Should().ThrowAsync<HttpRequestException>();
    }

    private static HttpClient BuildHttpClient(HttpStatusCode statusCode, string responseBody = "")
    {
        var handlerMock = new Mock<HttpMessageHandler>();

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(responseBody)
            });

        return new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://example.com")
        };
    }
}
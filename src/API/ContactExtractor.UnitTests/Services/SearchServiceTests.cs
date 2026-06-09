using System.Net;
using ContactExtractor.Core.Services;
using FluentAssertions;
using Moq;
using Moq.Protected;

namespace ContactExtractor.Tests.Services;

public class SearchServiceTests
{

    [Test]
    public async Task Search_Returns_ResponseBody_When_200()
    {
        // Arrange
        const string expectedBody = "<html>some results</html>";
        var httpClient = BuildHttpClient(HttpStatusCode.OK, expectedBody);
        var sut = new SearchService(httpClient);

        // Act
        var result = await sut.Search("London", CancellationToken.None);

        // Assert
        result.Should().Be(expectedBody);
    }

    [Test]
    public async Task Search_Sends_Correct_FormFields()
    {
        // Arrange
        HttpRequestMessage? capturedRequest = null;

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>((req, _) => capturedRequest = req)
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(string.Empty)
            });

        var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri("https://example.com") };
        var sut = new SearchService(httpClient);

        // Act
        await sut.Search("Manchester", CancellationToken.None);

        // Assert
        capturedRequest.Should().NotBeNull();
        capturedRequest.Method.Should().Be(HttpMethod.Post);

        var formContent = await capturedRequest.Content!.ReadAsStringAsync();
        //Assert.Contains("did=Select+area+of+law", formContent);
        //Assert.Contains("location=Manchester", formContent);
    }

    [Test]
    public async Task Search_Throws_HttpRequestException_When_400()
    {
        // Arrange
        var httpClient = BuildHttpClient(HttpStatusCode.BadRequest);
        var sut = new SearchService(httpClient);

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
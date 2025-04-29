using System.Net;
using AlphaPlus.Core.Models;
using AlphaPlus.Main.Site.Features.Language;
using Moq;
using Moq.Protected;

namespace AlphaPlus.Main.Site.UnitTests.Features.Language;

public class LanguageServiceTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly ILanguageService _languageService;

    public LanguageServiceTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("http://test.com/")
        };
        _languageService = new LanguageService(_httpClient);
    }

    [Fact]
    public async Task UpdateUsageLanguage_WhenSuccessful_ReturnsSuccessResult()
    {
        // Arrange
        var custId = 1;
        var subAccId = 2;
        var languageKey = "en-US";
        var ip = "127.0.0.1";
        var expectedResult = Result.CreateSuccessfulResult();

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(expectedResult))
            });

        // Act
        var result = await _languageService.UpdateUsageLanguage(custId, subAccId, languageKey, ip);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccessful);

        _mockHttpMessageHandler.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Post &&
                req.RequestUri.ToString().EndsWith("customer/usage-language")),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    [Fact]
    public async Task UpdateUsageLanguage_WhenFailure_ReturnsFailureResult()
    {
        // Arrange
        var custId = 1;
        var subAccId = 2;
        var languageKey = "en-US";
        var ip = "127.0.0.1";
        var expectedResult = Result.CreateErrorResult("Error occurred");

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(expectedResult))
            });

        // Act
        var result = await _languageService.UpdateUsageLanguage(custId, subAccId, languageKey, ip);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsFailed);
        Assert.Equal("Error occurred", result.Message);
    }

    [Fact]
    public async Task UpdateUsageLanguage_WhenHttpRequestFails_ThrowsException()
    {
        // Arrange
        var custId = 1;
        var subAccId = 2;
        var languageKey = "en-US";
        var ip = "127.0.0.1";

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ThrowsAsync(new HttpRequestException("Network error"));

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            _languageService.UpdateUsageLanguage(custId, subAccId, languageKey, ip));
    }
}
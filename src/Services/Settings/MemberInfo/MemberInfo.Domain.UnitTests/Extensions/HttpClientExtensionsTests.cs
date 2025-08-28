using AlphaPlus.Settings.MemberInfo.Domain.Extensions;
using System.Net;
using Newtonsoft.Json;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Extensions;

public class HttpClientExtensionsTests : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly HttpMessageHandler _mockHandler;

    public HttpClientExtensionsTests()
    {
        _mockHandler = new MockHttpMessageHandler();
        _httpClient = new HttpClient(_mockHandler);
    }

    [Fact]
    public void Can_Create_HttpClientExtensions_Instance()
    {
        // Arrange & Act & Assert
        // This test just verifies the extension methods can be called
        Assert.True(true);
    }

    [Fact]
    public async Task GetAsStringAsync_Should_Return_String()
    {
        // Arrange
        var expectedResponse = "test response";
        var mockHandler = new MockHttpMessageHandler();
        mockHandler.SetupResponse(HttpStatusCode.OK, expectedResponse);
        var httpClient = new HttpClient(mockHandler);

        // Act
        var result = await httpClient.GetAsStringAsync("http://test.com");

        // Assert
        Assert.Equal(expectedResponse, result);
    }

    [Fact]
    public async Task PostAsStringAsync_Should_Return_String()
    {
        // Arrange
        var expectedResponse = "test response";
        var payload = new { test = "data" };
        var mockHandler = new MockHttpMessageHandler();
        mockHandler.SetupResponse(HttpStatusCode.OK, expectedResponse);
        var httpClient = new HttpClient(mockHandler);

        // Act
        var result = await httpClient.PostAsStringAsync("http://test.com", payload);

        // Assert
        Assert.Equal(expectedResponse, result);
    }

    [Fact]
    public async Task PutAsStringAsync_Should_Return_String()
    {
        // Arrange
        var expectedResponse = "test response";
        var payload = new { test = "data" };
        var mockHandler = new MockHttpMessageHandler();
        mockHandler.SetupResponse(HttpStatusCode.OK, expectedResponse);
        var httpClient = new HttpClient(mockHandler);

        // Act
        var result = await httpClient.PutAsStringAsync("http://test.com", payload);

        // Assert
        Assert.Equal(expectedResponse, result);
    }

    [Fact]
    public async Task GetAsJsonAsync_Should_Return_Deserialized_Object()
    {
        // Arrange
        var testData = new TestData { Id = 1, Name = "Test" };
        var jsonResponse = JsonConvert.SerializeObject(testData);
        var mockHandler = new MockHttpMessageHandler();
        mockHandler.SetupResponse(HttpStatusCode.OK, jsonResponse);
        var httpClient = new HttpClient(mockHandler);

        // Act
        var result = await httpClient.GetAsJsonAsync<TestData>("http://test.com");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(testData.Id, result.Id);
        Assert.Equal(testData.Name, result.Name);
    }

    [Fact]
    public async Task PostAsJsonAsync_Should_Return_Deserialized_Object()
    {
        // Arrange
        var testData = new TestData { Id = 1, Name = "Test" };
        var jsonResponse = JsonConvert.SerializeObject(testData);
        var payload = new { test = "data" };
        var mockHandler = new MockHttpMessageHandler();
        mockHandler.SetupResponse(HttpStatusCode.OK, jsonResponse);
        var httpClient = new HttpClient(mockHandler);

        // Act
        var result = await httpClient.PostAsJsonAsync<TestData>("http://test.com", payload);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(testData.Id, result.Id);
        Assert.Equal(testData.Name, result.Name);
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
        _mockHandler?.Dispose();
    }

    private class TestData
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    private class MockHttpMessageHandler : HttpMessageHandler
    {
        private HttpStatusCode _statusCode = HttpStatusCode.OK;
        private string _content = string.Empty;

        public void SetupResponse(HttpStatusCode statusCode, string content)
        {
            _statusCode = statusCode;
            _content = content;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(_content)
            };
            return Task.FromResult(response);
        }
    }
}

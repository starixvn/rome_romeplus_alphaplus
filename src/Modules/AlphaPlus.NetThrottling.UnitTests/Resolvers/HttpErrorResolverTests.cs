using AlphaPlus.NetThrottling.Resolvers;
using AutoFixture;

namespace AlphaPlus.NetThrottling.UnitTests.Resolvers
{
    public class HttpErrorResolverTests
    {
        private readonly Fixture _fixture;
        private readonly HttpErrorResolver _httpErrorResolver;

        public HttpErrorResolverTests()
        {
            _fixture = new Fixture();
            _httpErrorResolver = new HttpErrorResolver();
        }

        [Fact]
        public void BlockNonAjaxRequest_ShouldSetStatusCodeTo429()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            // Act
            var task = _httpErrorResolver.BlockNonAjaxRequest(httpContext);

            // Assert
            Assert.Equal(429, httpContext.Response.StatusCode);
            Assert.Equal("text/plain", httpContext.Response.ContentType);
        }

        [Fact]
        public void BlockNonAjaxRequest_ShouldWriteResponse()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Response.Body = new MemoryStream();

            // Act
            var task = _httpErrorResolver.BlockNonAjaxRequest(httpContext);

            // Assert
            Assert.Equal(Task.CompletedTask, task);
            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = new StreamReader(httpContext.Response.Body).ReadToEnd();
            Assert.Contains("Too many requests", responseBody);
        }

        [Fact]
        public void BlockNonAjaxRequest_ShouldHandleHttpContextWithExistingResponse()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Response.StatusCode = 200;
            httpContext.Response.ContentType = "text/html";

            // Act
            var task = _httpErrorResolver.BlockNonAjaxRequest(httpContext);

            // Assert
            Assert.Equal(429, httpContext.Response.StatusCode);
            Assert.Equal("text/plain", httpContext.Response.ContentType);
        }

        [Fact]
        public async Task BlockNonAjaxRequest_ShouldNotThrowExceptionOnMultipleCalls()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            // Act & Assert
            Assert.Null(await Record.ExceptionAsync(() => _httpErrorResolver.BlockNonAjaxRequest(httpContext)));
            Assert.Null(await Record.ExceptionAsync(() => _httpErrorResolver.BlockNonAjaxRequest(httpContext)));
        }
    }
}

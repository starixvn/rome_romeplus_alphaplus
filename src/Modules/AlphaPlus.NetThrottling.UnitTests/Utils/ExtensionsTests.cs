using AlphaPlus.NetThrottling.Models;
using AlphaPlus.NetThrottling.Utils;
using AutoFixture;

namespace AlphaPlus.NetThrottling.UnitTests.Utils
{
    public class ExtensionsTests
    {
        private readonly Fixture _fixture;

        public ExtensionsTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void IsMatchRequest_WithMatchingPathAndMethod_ShouldReturnTrue()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/api/test";
            httpContext.Request.Method = "GET";
            var policy = new ThrottlePolicy { Path = "/api/test", Method = "GET" };

            // Act
            var result = httpContext.Request.IsMatchRequest(policy);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsMatchRequest_WithWildcardPath_ShouldReturnTrue()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/api/test";
            httpContext.Request.Method = "GET";
            var policy = new ThrottlePolicy { Path = "*", Method = "GET" };

            // Act
            var result = httpContext.Request.IsMatchRequest(policy);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsMatchRequest_WithWildcardMethod_ShouldReturnTrue()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/api/test";
            httpContext.Request.Method = "POST";
            var policy = new ThrottlePolicy { Path = "/api/test", Method = "*" };

            // Act
            var result = httpContext.Request.IsMatchRequest(policy);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsMatchRequest_WithNonMatchingPath_ShouldReturnFalse()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/api/test";
            httpContext.Request.Method = "GET";
            var policy = new ThrottlePolicy { Path = "/api/other", Method = "GET" };

            // Act
            var result = httpContext.Request.IsMatchRequest(policy);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsMatchRequest_WithNonMatchingMethod_ShouldReturnFalse()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/api/test";
            httpContext.Request.Method = "GET";
            var policy = new ThrottlePolicy { Path = "/api/test", Method = "POST" };

            // Act
            var result = httpContext.Request.IsMatchRequest(policy);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsAjaxRequest_WithXRequestedWithHeader_ShouldReturnTrue()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["X-Requested-With"] = "XMLHttpRequest";

            // Act
            var result = httpContext.Request.IsAjaxRequest();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsAjaxRequest_WithIsPostAjaxHeader_ShouldReturnTrue()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["IsPostAjax"] = "true";

            // Act
            var result = httpContext.Request.IsAjaxRequest();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsAjaxRequest_WithoutAjaxHeaders_ShouldReturnFalse()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            // Act
            var result = httpContext.Request.IsAjaxRequest();

            // Assert
            Assert.False(result);
        }
    }
}

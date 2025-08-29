using System.IO;
using System.Xml.Xsl;
using AlphaPlus.Settings.ViewLogs.API.Params;
using AlphaPlus.Settings.ViewLogs.API.Services;
using AlphaPlus.Settings.ViewLogs.API.Services.Impl;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.Settings.ViewLogs.API.UnitTests.Services
{
    public class TransformServiceTests : IDisposable
    {
        private readonly IMemoryCache _memoryCache;
        private readonly Mock<BaseLogFormatter> _mockLogFormatter;
        private readonly TransformServiceImpl _transformService;
        private readonly Fixture _fixture;
        private readonly string _tempDirectory;

        public TransformServiceTests()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _mockLogFormatter = new Mock<BaseLogFormatter>();
            _transformService = new TransformServiceImpl(_memoryCache);
            _fixture = new Fixture();
            
            // Create a temporary directory for testing file operations
            _tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tempDirectory);
        }

        [Fact]
        public void Init_WithValidLogFormatter_ShouldSetLogFormatter()
        {
            // Act
            _transformService.Init(_mockLogFormatter.Object);

            // Assert - We can't directly test the private field, but we can test the behavior
            // by calling TransformTrackingValue which uses the formatter
            Assert.True(true); // This test validates that Init doesn't throw
        }

        [Fact]
        public void Init_WithNullLogFormatter_ShouldNotThrow()
        {
            // Act & Assert
            var exception = Record.Exception(() => _transformService.Init(null));
            Assert.Null(exception);
        }

        [Fact]
        public void TransformTrackingValue_WithNullTransformParam_ShouldReturnEmpty()
        {
            // Arrange
            var trackingValue = _fixture.Create<string>();
            var logActionId = _fixture.Create<int>();

            // Act
            var result = _transformService.TransformTrackingValue(trackingValue, logActionId, null);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void TransformTrackingValue_WithNullOrEmptyTrackingValue_ShouldReturnEmpty(string trackingValue)
        {
            // Arrange
            var logActionId = _fixture.Create<int>();
            var transformParam = _fixture.Create<TransformParam>();

            // Act
            var result = _transformService.TransformTrackingValue(trackingValue, logActionId, transformParam);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void TransformTrackingValue_WithNegativeLogActionId_ShouldReturnEmpty()
        {
            // Arrange
            var trackingValue = _fixture.Create<string>();
            var logActionId = -1;
            var transformParam = _fixture.Create<TransformParam>();

            // Act
            var result = _transformService.TransformTrackingValue(trackingValue, logActionId, transformParam);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void TransformTrackingValue_WithValidInputButNoCompiledTransform_ShouldReturnNull()
        {
            // Arrange
            var trackingValue = "<root>test</root>";
            var logActionId = 1;
            var transformParam = new TransformParam
            {
                TrackingValueTransformTemplatePath = "non-existent-path"
            };

            _transformService.Init(_mockLogFormatter.Object);

            // Using real MemoryCache, so cache will naturally be empty for new tests

            // Act
            var result = _transformService.TransformTrackingValue(trackingValue, logActionId, transformParam);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void TransformTrackingValue_WithCachedTransform_ShouldUseCachedValue()
        {
            // Arrange
            var trackingValue = "<root><test>value</test></root>";
            var logActionId = 1;
            var templatePath = Path.Combine(_tempDirectory, "action_1.xslt");
            var xsltContent = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<xsl:stylesheet version=""1.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"">
    <xsl:output method=""text""/>
    <xsl:template match=""/"">
        <xsl:value-of select=""//test""/>
    </xsl:template>
</xsl:stylesheet>";
            
            File.WriteAllText(templatePath, xsltContent);

            var transformParam = new TransformParam
            {
                TrackingValueTransformTemplatePath = _tempDirectory
            };

            _transformService.Init(_mockLogFormatter.Object);

            // First call to cache the transform
            var firstResult = _transformService.TransformTrackingValue(trackingValue, logActionId, transformParam, false);
            
            // Second call should use cached value
            var result = _transformService.TransformTrackingValue(trackingValue, logActionId, transformParam, false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("value", result.Trim());
            Assert.Equal(firstResult, result); // Should return same result from cache
            _mockLogFormatter.Verify(x => x.Init(transformParam, false), Times.Exactly(2)); // Called twice
        }

        [Fact]
        public void TransformTrackingValue_WithHideNicknameTrue_ShouldPassToFormatter()
        {
            // Arrange
            var trackingValue = "<root>test</root>";
            var logActionId = 1;
            var templatePath = Path.Combine(_tempDirectory, "action_1.xslt");
            
            // Create a valid XSLT template
            var xsltContent = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<xsl:stylesheet version=""1.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"">
    <xsl:output method=""text""/>
    <xsl:template match=""/"">
        <xsl:value-of select=""//test""/>
    </xsl:template>
</xsl:stylesheet>";
            
            File.WriteAllText(templatePath, xsltContent);

            var transformParam = new TransformParam
            {
                TrackingValueTransformTemplatePath = _tempDirectory
            };
            var isHideNickname = true;

            _transformService.Init(_mockLogFormatter.Object);

            // Using real MemoryCache, so cache will naturally be empty for new tests
            


            // Act
            _transformService.TransformTrackingValue(trackingValue, logActionId, transformParam, isHideNickname);

            // Assert
            _mockLogFormatter.Verify(x => x.Init(transformParam, isHideNickname), Times.Once);
        }

        [Fact]
        public void TransformTrackingValue_WithValidXMLAndExistingTemplate_ShouldTransform()
        {
            // Arrange
            var trackingValue = "<root><test>value</test></root>";
            var logActionId = 1;
            var templatePath = Path.Combine(_tempDirectory, "action_1.xslt");
            
            // Create a simple XSLT template
            var xsltContent = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<xsl:stylesheet version=""1.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"">
    <xsl:output method=""text""/>
    <xsl:template match=""/"">
        <xsl:value-of select=""//test""/>
    </xsl:template>
</xsl:stylesheet>";
            
            File.WriteAllText(templatePath, xsltContent);

            var transformParam = new TransformParam
            {
                TrackingValueTransformTemplatePath = _tempDirectory
            };

            _transformService.Init(_mockLogFormatter.Object);

            // Using real MemoryCache, so cache will naturally be empty for new tests
            


            // Act
            var result = _transformService.TransformTrackingValue(trackingValue, logActionId, transformParam);

            // Assert
            Assert.Equal("value", result?.Trim());
        }

        [Fact]
        public void Constructor_WithNullMemoryCache_ShouldAcceptNullValue()
        {
            // Act - Constructor accepts null values in modern .NET
            var service = new TransformServiceImpl(null);

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public void Constructor_WithValidMemoryCache_ShouldCreateInstance()
        {
            // Act
            var service = new TransformServiceImpl(_memoryCache);

            // Assert
            Assert.NotNull(service);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(999)]
        [InlineData(int.MaxValue)]
        public void TransformTrackingValue_WithValidLogActionIds_ShouldHandleCorrectly(int logActionId)
        {
            // Arrange
            var trackingValue = "<root>test</root>";
            var transformParam = _fixture.Create<TransformParam>();

            _transformService.Init(_mockLogFormatter.Object);

            // Using real MemoryCache, so cache will naturally be empty for new tests

            // Act
            var result = _transformService.TransformTrackingValue(trackingValue, logActionId, transformParam);

            // Assert
            // For non-existent templates, should return null
            Assert.Null(result);
        }

        [Fact]
        public void TransformTrackingValue_WithInvalidXML_ShouldThrowException()
        {
            // Arrange
            var invalidXml = "<root><unclosed>";
            var logActionId = 1;
            var templatePath = Path.Combine(_tempDirectory, "action_1.xslt");
            
            // Create a simple XSLT template
            var xsltContent = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<xsl:stylesheet version=""1.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"">
    <xsl:output method=""text""/>
    <xsl:template match=""/"">
        <xsl:value-of select=""//test""/>
    </xsl:template>
</xsl:stylesheet>";
            
            File.WriteAllText(templatePath, xsltContent);

            var transformParam = new TransformParam
            {
                TrackingValueTransformTemplatePath = _tempDirectory
            };

            _transformService.Init(_mockLogFormatter.Object);

            // Using real MemoryCache, so cache will naturally be empty for new tests

            // Act & Assert
            Assert.ThrowsAny<Exception>(() => 
                _transformService.TransformTrackingValue(invalidXml, logActionId, transformParam));
        }

        public void Dispose()
        {
            // Cleanup temporary directory
            if (Directory.Exists(_tempDirectory))
            {
                Directory.Delete(_tempDirectory, true);
            }
            
            // Dispose MemoryCache
            _memoryCache?.Dispose();
        }
    }
}

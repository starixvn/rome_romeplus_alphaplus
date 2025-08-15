using AlphaPlus.Main.Site.Features.Dashboard.Banner;
using FluentAssertions;
using Xunit;

namespace AlphaPlus.Main.Site.UnitTests.Features.Dashboard.Banner
{
    public class BannerConfigurationTests
    {
        [Fact]
        public void Properties_ShouldBeReadableAndWritable()
        {
            // Arrange
            var configuration = new BannerConfiguration();

            // Act & Assert - Properties should be readable and writable
            var type = configuration.GetType();
            var nameProperty = type.GetProperty(nameof(BannerConfiguration.Name));
            var imageSourceProperty = type.GetProperty(nameof(BannerConfiguration.ImageSource));
            var imageMobileSourceProperty = type.GetProperty(nameof(BannerConfiguration.ImageMobileSource));
            var urlProperty = type.GetProperty(nameof(BannerConfiguration.Url));
            var isVideoProperty = type.GetProperty(nameof(BannerConfiguration.IsVideo));
            var logClickProperty = type.GetProperty(nameof(BannerConfiguration.LogClick));

            nameProperty.Should().NotBeNull();
            imageSourceProperty.Should().NotBeNull();
            imageMobileSourceProperty.Should().NotBeNull();
            urlProperty.Should().NotBeNull();
            isVideoProperty.Should().NotBeNull();
            logClickProperty.Should().NotBeNull();

            nameProperty.CanRead.Should().BeTrue();
            imageSourceProperty.CanRead.Should().BeTrue();
            imageMobileSourceProperty.CanRead.Should().BeTrue();
            urlProperty.CanRead.Should().BeTrue();
            isVideoProperty.CanRead.Should().BeTrue();
            logClickProperty.CanRead.Should().BeTrue();

            nameProperty.CanWrite.Should().BeTrue();
            imageSourceProperty.CanWrite.Should().BeTrue();
            imageMobileSourceProperty.CanWrite.Should().BeTrue();
            urlProperty.CanWrite.Should().BeTrue();
            isVideoProperty.CanWrite.Should().BeTrue();
            logClickProperty.CanWrite.Should().BeTrue();
        }

        [Fact]
        public void Name_ShouldBeSetAndRetrieved()
        {
            // Arrange
            const string expectedName = "TestBanner";
            var configuration = new BannerConfiguration();

            // Act
            configuration.Name = expectedName;

            // Assert
            configuration.Name.Should().Be(expectedName);
        }

        [Fact]
        public void Name_WithNullValue_ShouldBeSetAndRetrieved()
        {
            // Arrange
            var configuration = new BannerConfiguration();

            // Act
            configuration.Name = null;

            // Assert
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Name_WithEmptyString_ShouldBeSetAndRetrieved()
        {
            // Arrange
            const string expectedName = "";
            var configuration = new BannerConfiguration();

            // Act
            configuration.Name = expectedName;

            // Assert
            configuration.Name.Should().Be(expectedName);
        }

        [Fact]
        public void ImageSource_ShouldBeSetAndRetrieved()
        {
            // Arrange
            const string expectedImageSource = "/images/banner.jpg";
            var configuration = new BannerConfiguration();

            // Act
            configuration.ImageSource = expectedImageSource;

            // Assert
            configuration.ImageSource.Should().Be(expectedImageSource);
        }

        [Fact]
        public void ImageSource_WithNullValue_ShouldBeSetAndRetrieved()
        {
            // Arrange
            var configuration = new BannerConfiguration();

            // Act
            configuration.ImageSource = null;

            // Assert
            configuration.ImageSource.Should().BeNull();
        }

        [Fact]
        public void ImageMobileSource_ShouldBeSetAndRetrieved()
        {
            // Arrange
            const string expectedImageMobileSource = "/images/banner-mobile.jpg";
            var configuration = new BannerConfiguration();

            // Act
            configuration.ImageMobileSource = expectedImageMobileSource;

            // Assert
            configuration.ImageMobileSource.Should().Be(expectedImageMobileSource);
        }

        [Fact]
        public void ImageMobileSource_WithNullValue_ShouldBeSetAndRetrieved()
        {
            // Arrange
            var configuration = new BannerConfiguration();

            // Act
            configuration.ImageMobileSource = null;

            // Assert
            configuration.ImageMobileSource.Should().BeNull();
        }

        [Fact]
        public void Url_ShouldBeSetAndRetrieved()
        {
            // Arrange
            const string expectedUrl = "https://example.com/promo";
            var configuration = new BannerConfiguration();

            // Act
            configuration.Url = expectedUrl;

            // Assert
            configuration.Url.Should().Be(expectedUrl);
        }

        [Fact]
        public void Url_WithNullValue_ShouldBeSetAndRetrieved()
        {
            // Arrange
            var configuration = new BannerConfiguration();

            // Act
            configuration.Url = null;

            // Assert
            configuration.Url.Should().BeNull();
        }

        [Fact]
        public void IsVideo_ShouldBeSetAndRetrieved()
        {
            // Arrange
            const bool expectedIsVideo = true;
            var configuration = new BannerConfiguration();

            // Act
            configuration.IsVideo = expectedIsVideo;

            // Assert
            configuration.IsVideo.Should().Be(expectedIsVideo);
        }

        [Fact]
        public void IsVideo_WithFalseValue_ShouldBeSetAndRetrieved()
        {
            // Arrange
            const bool expectedIsVideo = false;
            var configuration = new BannerConfiguration();

            // Act
            configuration.IsVideo = expectedIsVideo;

            // Assert
            configuration.IsVideo.Should().Be(expectedIsVideo);
        }

        [Fact]
        public void LogClick_ShouldBeSetAndRetrieved()
        {
            // Arrange
            const bool expectedLogClick = true;
            var configuration = new BannerConfiguration();

            // Act
            configuration.LogClick = expectedLogClick;

            // Assert
            configuration.LogClick.Should().Be(expectedLogClick);
        }

        [Fact]
        public void LogClick_WithFalseValue_ShouldBeSetAndRetrieved()
        {
            // Arrange
            const bool expectedLogClick = false;
            var configuration = new BannerConfiguration();

            // Act
            configuration.LogClick = expectedLogClick;

            // Assert
            configuration.LogClick.Should().Be(expectedLogClick);
        }

        [Fact]
        public void AllProperties_ShouldBeIndependent()
        {
            // Arrange
            var configuration = new BannerConfiguration();

            // Act
            configuration.Name = "Banner1";
            configuration.ImageSource = "/images/banner1.jpg";
            configuration.ImageMobileSource = "/images/banner1-mobile.jpg";
            configuration.Url = "https://example.com/banner1";
            configuration.IsVideo = true;
            configuration.LogClick = true;

            // Assert
            configuration.Name.Should().Be("Banner1");
            configuration.ImageSource.Should().Be("/images/banner1.jpg");
            configuration.ImageMobileSource.Should().Be("/images/banner1-mobile.jpg");
            configuration.Url.Should().Be("https://example.com/banner1");
            configuration.IsVideo.Should().BeTrue();
            configuration.LogClick.Should().BeTrue();

            // Change some properties
            configuration.Name = "Banner2";
            configuration.IsVideo = false;

            // Assert that only changed properties are affected
            configuration.Name.Should().Be("Banner2");
            configuration.ImageSource.Should().Be("/images/banner1.jpg");
            configuration.ImageMobileSource.Should().Be("/images/banner1-mobile.jpg");
            configuration.Url.Should().Be("https://example.com/banner1");
            configuration.IsVideo.Should().BeFalse();
            configuration.LogClick.Should().BeTrue();
        }
    }
}


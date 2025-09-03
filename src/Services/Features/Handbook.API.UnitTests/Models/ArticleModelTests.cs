using AlphaPlus.Features.Handbook.API.Constants;
using AlphaPlus.Features.Handbook.API.Models;
using Xunit;

namespace AlphaPlus.Features.Handbook.API.UnitTests.Models
{
    public class ArticleModelTests
    {
        [Fact]
        public void PublishedAtStr_WhenPublishedAtIsNull_ReturnsEmptyString()
        {
            // Arrange
            var article = new ArticleModel
            {
                PublishedAt = null
            };

            // Act
            var result = article.PublishedAtStr;

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void PublishedAtStr_WhenPublishedAtHasValue_ReturnsFormattedDate()
        {
            // Arrange
            var publishedDate = new DateTime(2024, 1, 15);
            var article = new ArticleModel
            {
                PublishedAt = publishedDate
            };

            // Act
            var result = article.PublishedAtStr;

            // Assert
            Assert.Equal("Mon, Jan 15, 2024", result);
        }

        [Fact]
        public void GetTargetFrame_WhenReferenceURLIsReference_ReturnsBlank()
        {
            // Arrange
            var article = new ArticleModel
            {
                ReferenceURL = ReferenceURLConsts.CustomerSettingInformation
            };

            // Act
            var result = article.GetTargetFrame();

            // Assert
            Assert.Equal("_blank", result);
        }

        [Fact]
        public void GetTargetFrame_WhenReferenceURLIsNotReference_ReturnsMain()
        {
            // Arrange
            var article = new ArticleModel
            {
                ReferenceURL = "some-other-url"
            };

            // Act
            var result = article.GetTargetFrame();

            // Assert
            Assert.Equal("main", result);
        }

        [Fact]
        public void GetTargetFrame_WhenReferenceURLIsNull_ReturnsMain()
        {
            // Arrange
            var article = new ArticleModel
            {
                ReferenceURL = null
            };

            // Act
            var result = article.GetTargetFrame();

            // Assert
            Assert.Equal("main", result);
        }

        [Fact]
        public void TargetFrame_Property_UsesGetTargetFrame()
        {
            // Arrange
            var article = new ArticleModel
            {
                ReferenceURL = ReferenceURLConsts.CustomerSettingInformation
            };

            // Act
            var result = article.TargetFrame;

            // Assert
            Assert.Equal("_blank", result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void GetUrl_WhenReferenceURLIsNullOrEmpty_ReturnsHandbookArticleUrl(string referenceURL)
        {
            // Arrange
            var article = new ArticleModel
            {
                ReferenceURL = referenceURL,
                Slug = "test-article"
            };

            // Act
            var result = article.GetUrl();

            // Assert
            Assert.Equal("HandbookArticle/GetBySlug?slug=test-article", result);
        }

        [Fact]
        public void GetUrl_WhenReferenceURLStartsWithHttp_ReturnsReferenceURL()
        {
            // Arrange
            var referenceURL = "https://example.com";
            var article = new ArticleModel
            {
                ReferenceURL = referenceURL,
                Slug = "test-article"
            };

            // Act
            var result = article.GetUrl();

            // Assert
            Assert.Equal(referenceURL, result);
        }

        [Fact]
        public void GetUrl_WithRelativePath_PrependRelativePath()
        {
            // Arrange
            var article = new ArticleModel
            {
                ReferenceURL = null,
                Slug = "test-article"
            };
            var relativePath = "/features/";

            // Act
            var result = article.GetUrl(relativePath);

            // Assert
            Assert.Equal("/features/HandbookArticle/GetBySlug?slug=test-article", result);
        }

        [Fact]
        public void GetUrl_WhenReferenceURLIsCustomerSettingInformation_ReturnsSettingHandbookUrl()
        {
            // Arrange
            var article = new ArticleModel
            {
                ReferenceURL = ReferenceURLConsts.CustomerSettingInformation,
                SessionID = "test-session-123"
            };

            // Act
            var result = article.GetUrl();

            // Assert
            Assert.Contains("SettingHandbook/SportBookInformation?tabindex=0", result);
            Assert.Contains("(S(test-session-123))", result);
        }

        [Fact]
        public void GetUrl_WhenReferenceURLIsCustomerSettingInformationAndSessionIDIsEmpty_ReturnsUrlWithoutSession()
        {
            // Arrange
            var article = new ArticleModel
            {
                ReferenceURL = ReferenceURLConsts.CustomerSettingInformation,
                SessionID = ""
            };

            // Act
            var result = article.GetUrl();

            // Assert
            Assert.Contains("SettingHandbook/SportBookInformation?tabindex=0", result);
            Assert.DoesNotContain("(S(", result);
        }

        [Fact]
        public void Url_Property_UsesGetUrl()
        {
            // Arrange
            var article = new ArticleModel
            {
                ReferenceURL = null,
                Slug = "test-article"
            };

            // Act
            var result = article.Url;

            // Assert
            Assert.Equal("HandbookArticle/GetBySlug?slug=test-article", result);
        }

        [Fact]
        public void CorrectImageUrls_WhenContentIsNull_DoesNothing()
        {
            // Arrange
            var article = new ArticleModel
            {
                Content = null
            };

            // Act
            article.CorrectImageUrls();

            // Assert
            Assert.Null(article.Content);
        }

        [Fact]
        public void CorrectImageUrls_WhenContentIsEmpty_DoesNothing()
        {
            // Arrange
            var article = new ArticleModel
            {
                Content = ""
            };

            // Act
            article.CorrectImageUrls();

            // Assert
            Assert.Equal("", article.Content);
        }

        [Fact]
        public void CorrectImageUrls_WhenContentIsWhitespace_DoesNothing()
        {
            // Arrange
            var article = new ArticleModel
            {
                Content = "   "
            };

            // Act
            article.CorrectImageUrls();

            // Assert
            Assert.Equal("   ", article.Content);
        }

        [Fact]
        public void CorrectImageUrls_WhenNoImageTags_DoesNotModifyContent()
        {
            // Arrange
            var originalContent = "<p>This is some content without images</p>";
            var article = new ArticleModel
            {
                Content = originalContent
            };

            // Act
            article.CorrectImageUrls();

            // Assert
            Assert.Equal(originalContent, article.Content);
        }

        [Fact]
        public void CorrectImageUrls_WhenImageTagWithoutApiPath_DoesNotModifyContent()
        {
            // Arrange
            var originalContent = "<img src=\"/other/path/image.jpg\" alt=\"test\">";
            var article = new ArticleModel
            {
                Content = originalContent
            };

            // Act
            article.CorrectImageUrls();

            // Assert
            Assert.Equal(originalContent, article.Content);
        }

        [Fact]
        public void CorrectImageUrls_WhenImageTagWithApiPath_ReplacesWithHandbookImageUrl()
        {
            // Arrange
            var originalContent = "<img src=\"/api/images/get/test-image.jpg\" alt=\"test\">";
            var article = new ArticleModel
            {
                Content = originalContent
            };

            // Act
            article.CorrectImageUrls();

            // Assert
            Assert.Contains("HandbookImage/Get?name=", article.Content);
            Assert.Contains("test-image.jpg", article.Content);
        }

        [Fact]
        public void CorrectImageUrls_WithMultipleImageTags_ReplacesAllApiPaths()
        {
            // Arrange
            var originalContent = "<p><img src=\"/api/images/get/image1.jpg\"> and <img src=\"/api/images/get/image2.png\"></p>";
            var article = new ArticleModel
            {
                Content = originalContent
            };

            // Act
            article.CorrectImageUrls();

            // Assert
            Assert.Contains("HandbookImage/Get?name=image1.jpg", article.Content);
            Assert.Contains("HandbookImage/Get?name=image2.png", article.Content);
            Assert.DoesNotContain("/api/images/get/", article.Content);
        }

        [Fact]
        public void CorrectImageUrls_WithMixedImageSources_OnlyReplacesApiPaths()
        {
            // Arrange
            var originalContent = "<img src=\"/api/images/get/api-image.jpg\"><img src=\"/other/path/other-image.jpg\">";
            var article = new ArticleModel
            {
                Content = originalContent
            };

            // Act
            article.CorrectImageUrls();

            // Assert
            Assert.Contains("HandbookImage/Get?name=api-image.jpg", article.Content);
            Assert.Contains("/other/path/other-image.jpg", article.Content);
        }

        [Fact]
        public void AllProperties_CanBeSetAndRetrieved()
        {
            // Arrange
            var categories = new List<CategoryModel> { new CategoryModel { Name = "Test Category" } };
            var article = new ArticleModel
            {
                Name = "Test Article",
                Slug = "test-article",
                Title = "Test Title",
                Summary = "Test Summary",
                Content = "Test Content",
                ReferenceURL = "test-url",
                Order = 1,
                SessionID = "test-session",
                Categories = categories,
                PublishedAt = DateTime.Now
            };

            // Act & Assert
            Assert.Equal("Test Article", article.Name);
            Assert.Equal("test-article", article.Slug);
            Assert.Equal("Test Title", article.Title);
            Assert.Equal("Test Summary", article.Summary);
            Assert.Equal("Test Content", article.Content);
            Assert.Equal("test-url", article.ReferenceURL);
            Assert.Equal(1, article.Order);
            Assert.Equal("test-session", article.SessionID);
            Assert.Equal(categories, article.Categories);
            Assert.NotNull(article.PublishedAt);
        }
    }
}

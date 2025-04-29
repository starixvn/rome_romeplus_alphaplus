using AlphaPlus.Core.Models;
using AlphaPlus.Main.Site.Features.Language;
using MediatR;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Configuration;
using Moq;

namespace AlphaPlus.Main.Site.UnitTests.Features.Language
{
    public class LanguageListViewComponentTests : IAsyncDisposable
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<IUserSession> _userSession;
        private readonly LanguageListViewComponent _component;
        private readonly IConfiguration _configuration;

        public LanguageListViewComponentTests()
        {
            _mediator = new Mock<IMediator>();
            _userSession = new Mock<IUserSession>();
            _component = new LanguageListViewComponent(_mediator.Object, _userSession.Object);

            // Setup mock configuration
            var inMemorySettings = new Dictionary<string, string> {
                {"SiteLabel", "MBC"}
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            Core.Configurations.ConfigurationManager.Init(_configuration);
        }

        public ValueTask DisposeAsync()
        {
            // Reset configuration to avoid affecting other tests
            Core.Configurations.ConfigurationManager.Init(null);
            return ValueTask.CompletedTask;
        }

        [Theory]
        [InlineData("en-US")]
        [InlineData("zh-TW")]
        [InlineData("zh-CN")]
        [InlineData("ja-JP")]
        [InlineData("th-TH")]
        [InlineData("ko-KR")]
        [InlineData("vi-VN")]
        public async Task InvokeAsync_ReturnsCorrectViewModel(string languageCode)
        {
            // Arrange
            _userSession.Setup(x => x.Language).Returns(languageCode);

            var expectedViewModel = new LanguageViewModel(
                Shared.Constants.Language.CodeNames,
                languageCode);

            _mediator
                .Setup(x => x.Send(
                    It.Is<AllLanguagesQuery>(q =>
                        q.LanguageCode == languageCode &&
                        q.DisabledVietnamese == AppSettings.DisabledVietnamese),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedViewModel);

            // Act
            var result = await _component.InvokeAsync();

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewViewComponentResult>(result);
            var model = Assert.IsType<LanguageViewModel>(viewResult.ViewData.Model);
            Assert.Equal(languageCode, model.CurrentLanguage);
            Assert.Equal(Shared.Constants.Language.CodeNames, model.Data);

            _mediator.Verify(
                x => x.Send(
                    It.Is<AllLanguagesQuery>(q =>
                        q.LanguageCode == languageCode &&
                        q.DisabledVietnamese == AppSettings.DisabledVietnamese),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Theory]
        [InlineData("en-US")]
        [InlineData("zh-TW")]
        [InlineData("zh-CN")]
        [InlineData("ja-JP")]
        [InlineData("th-TH")]
        [InlineData("ko-KR")]
        public async Task InvokeAsync_WithDisabledVietnamese_ReturnsCorrectViewModel(string languageCode)
        {
            // Arrange
            _userSession.Setup(x => x.Language).Returns(languageCode);

            var expectedViewModel = new LanguageViewModel(
                Shared.Constants.Language.CodeNamesWithoutVietNamese,
                languageCode);

            _mediator
                .Setup(x => x.Send(
                    It.Is<AllLanguagesQuery>(q =>
                        q.LanguageCode == languageCode &&
                        q.DisabledVietnamese == AppSettings.DisabledVietnamese),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedViewModel);

            // Act
            var result = await _component.InvokeAsync();

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewViewComponentResult>(result);
            var model = Assert.IsType<LanguageViewModel>(viewResult.ViewData.Model);
            Assert.Equal(languageCode, model.CurrentLanguage);
            Assert.Equal(Shared.Constants.Language.CodeNamesWithoutVietNamese, model.Data);
            Assert.DoesNotContain(model.Data, x => x.Key == "vi-VN");

            _mediator.Verify(
                x => x.Send(
                    It.Is<AllLanguagesQuery>(q =>
                        q.LanguageCode == languageCode &&
                        q.DisabledVietnamese == AppSettings.DisabledVietnamese),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
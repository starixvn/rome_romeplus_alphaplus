using AlphaPlus.Core.Models;
using AlphaPlus.Main.Site.Features.Language;
using Moq;

namespace AlphaPlus.Main.Site.UnitTests.Features.Language
{
    public class LanguageHandlerTests
    {
        private readonly Mock<ILanguageService> _languageService;
        private readonly Mock<IUserSession> _userSession;
        private readonly LanguageHandler _handler;

        public LanguageHandlerTests()
        {
            _languageService = new Mock<ILanguageService>();
            _userSession = new Mock<IUserSession>();
            _userSession.Setup(x => x.Language).Returns("en-US");
            _handler = new LanguageHandler(_languageService.Object);
        }

        [Theory]
        [InlineData("en-US")]
        [InlineData("zh-TW")]
        [InlineData("zh-CN")]
        [InlineData("ja-JP")]
        [InlineData("th-TH")]
        [InlineData("ko-KR")]
        [InlineData("vi-VN")]
        public async Task Handle_AllLanguagesQuery_ReturnsCorrectViewModel(string languageCode)
        {
            // Arrange
            var query = new AllLanguagesQuery(languageCode, false);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(languageCode, result.CurrentLanguage);
            Assert.Equal(Shared.Constants.Language.CodeNames, result.Data);
        }

        [Theory]
        [InlineData("en-US")]
        [InlineData("zh-TW")]
        [InlineData("zh-CN")]
        [InlineData("ja-JP")]
        [InlineData("th-TH")]
        [InlineData("ko-KR")]
        public async Task Handle_AllLanguagesQuery_WithDisabledVietnamese_ReturnsCorrectViewModel(string languageCode)
        {
            // Arrange
            var query = new AllLanguagesQuery(languageCode, true);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(languageCode, result.CurrentLanguage);
            Assert.Equal(Shared.Constants.Language.CodeNamesWithoutVietNamese, result.Data);
            Assert.DoesNotContain(result.Data, x => x.Key == "vi-VN");
        }

        [Theory]
        [InlineData("en-US", "E")]
        [InlineData("zh-TW", "CT")]
        [InlineData("zh-CN", "CS")]
        [InlineData("ja-JP", "J")]
        [InlineData("th-TH", "TH")]
        [InlineData("ko-KR", "KO")]
        [InlineData("vi-VN", "VI")]
        public async Task Handle_ChangeLanguageCommand_UpdatesLanguageSuccessfully(string languageCode, string expectedKey)
        {
            // Arrange
            _userSession.Setup(x => x.CustId).Returns(123);
            _userSession.Setup(x => x.SubAccId).Returns(456);
            _userSession.Setup(x => x.UserLoginIp).Returns("127.0.0.1");

            var command = new ChangeLanguageCommand(
                _userSession.Object,
                languageCode,
                disabledVietnamese: false,
                isUpdateDisplayLanguage: true);

            _languageService
                .Setup(x => x.UpdateUsageLanguage(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(new Result(0));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(languageCode, result);
            _languageService.Verify(
                x => x.UpdateUsageLanguage(123, 456, expectedKey, "127.0.0.1"),
                Times.Once);
        }

        [Theory]
        [InlineData("en-US")]
        [InlineData("zh-TW")]
        public async Task Handle_ChangeLanguageCommand_WithoutUpdate_DoesNotCallService(string languageCode)
        {
            // Arrange
            _userSession.Setup(x => x.CustId).Returns(123);
            _userSession.Setup(x => x.SubAccId).Returns(456);
            _userSession.Setup(x => x.UserLoginIp).Returns("127.0.0.1");

            var command = new ChangeLanguageCommand(
                _userSession.Object,
                languageCode,
                disabledVietnamese: false,
                isUpdateDisplayLanguage: false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(languageCode, result);
            _languageService.Verify(
                x => x.UpdateUsageLanguage(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("invalid-code")]
        public async Task Handle_ChangeLanguageCommand_WithInvalidLanguageCode_ReturnsDefaultLanguage(string languageCode)
        {
            // Arrange
            _userSession.Setup(x => x.CustId).Returns(123);
            _userSession.Setup(x => x.SubAccId).Returns(456);
            _userSession.Setup(x => x.UserLoginIp).Returns("127.0.0.1");

            var command = new ChangeLanguageCommand(
                _userSession.Object,
                languageCode,
                disabledVietnamese: false,
                isUpdateDisplayLanguage: true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Shared.Constants.Language.Default, result);
        }
    }
}
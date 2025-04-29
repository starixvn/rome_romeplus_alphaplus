using AlphaPlus.Core;
using AlphaPlus.Core.Models;
using AlphaPlus.Main.Site.Features.Language;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AlphaPlus.Main.Site.Tests.Features.Language
{
    public class LanguageControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IUserSession> _userSessionMock;
        private readonly LanguageController _controller;

        public LanguageControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _userSessionMock = new Mock<IUserSession>();
            _controller = new LanguageController(_mediatorMock.Object, _userSessionMock.Object);

            // Setup default HttpContext with host header
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Host"] = "test.example.com";
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public async Task Change_WhenDisplayLanguageIsTrueAndUserIsInternal_ReturnsNullJson()
        {
            // Arrange
            _userSessionMock.Setup(x => x.IsInternal).Returns(true);

            // Act
            var result = await _controller.Change("en", true);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Change_WhenDisplayLanguageIsFalse_SetsCookiesAndReturnsViewModel()
        {
            // Arrange
            const string expectedLanguageCode = "en";
            _userSessionMock.Setup(x => x.IsInternal).Returns(false);
            _mediatorMock.Setup(x => x.Send(
                It.IsAny<ChangeLanguageCommand>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedLanguageCode);

            // Act
            var result = await _controller.Change(expectedLanguageCode, false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedLanguageCode, result.Value);
            _mediatorMock.Verify(x => x.Send(
                It.Is<ChangeLanguageCommand>(cmd =>
                    cmd.LanguageCode == expectedLanguageCode &&
                    !cmd.DisplayLanguage),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Change_WhenDisplayLanguageIsNull_SetsCookiesAndReturnsViewModel()
        {
            // Arrange
            const string expectedLanguageCode = "en";
            _userSessionMock.Setup(x => x.IsInternal).Returns(false);
            _mediatorMock.Setup(x => x.Send(
                It.IsAny<ChangeLanguageCommand>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedLanguageCode);

            // Act
            var result = await _controller.Change(expectedLanguageCode, null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedLanguageCode, result.Value);
            _mediatorMock.Verify(x => x.Send(
                It.Is<ChangeLanguageCommand>(cmd =>
                    cmd.LanguageCode == expectedLanguageCode &&
                    !cmd.DisplayLanguage),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Change_SetsCorrectCookieOptions()
        {
            // Arrange
            const string expectedLanguageCode = "en";
            _userSessionMock.Setup(x => x.IsInternal).Returns(false);
            _mediatorMock.Setup(x => x.Send(
                It.IsAny<ChangeLanguageCommand>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedLanguageCode);

            var responseCookies = new Mock<IResponseCookies>();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Host"] = "test.example.com";
            httpContext.Response.Cookies = responseCookies.Object;
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            // Act
            await _controller.Change(expectedLanguageCode, false);

            // Assert
            responseCookies.Verify(x => x.Append(
                "LANGUAGE",
                expectedLanguageCode,
                It.Is<CookieOptions>(opt =>
                    opt.Path == "/" &&
                    opt.Domain == ".example.com" &&
                    !opt.HttpOnly)),
                Times.Once);

            responseCookies.Verify(x => x.Append(
                "hidLanguage",
                expectedLanguageCode,
                It.Is<CookieOptions>(opt =>
                    opt.Path == "/" &&
                    opt.Domain == ".example.com" &&
                    !opt.HttpOnly)),
                Times.Once);
        }
    }
}
using AlphaPlus.Features.Site.Messages.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AlphaPlus.Features.Site.UnitTests.Controllers
{
    public class SiteBaseControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly TestSiteBaseController _controller;

        // Test implementation of abstract class
        public class TestSiteBaseController : SiteBaseController
        {
            public TestSiteBaseController(IMediator mediator) : base(mediator) { }

            public async Task<ActionResult> TestView<TResponse>(IRequest<TResponse> message)
                => await View(message);

            public async Task<ActionResult> TestJson<TResponse>(IRequest<TResponse> message)
                => await Json(message);

            public async Task<ActionResult> TestJsonAsync<TResponse>(IRequest<TResponse> message)
                => await JsonAsync(message);
        }

        // Test request and response models
        public class TestRequest : IRequest<TestResponse> { }
        public class TestResponse { public string Message { get; set; } = "Test"; }

        public SiteBaseControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new TestSiteBaseController(_mediatorMock.Object);
        }

        [Fact]
        public void Constructor_ShouldInitializeController()
        {
            // Assert
            _controller.Should().NotBeNull();
        }

        [Fact]
        public async Task View_ShouldCallMediatorAndReturnViewResult()
        {
            // Arrange
            var request = new TestRequest();
            var expectedResponse = new TestResponse { Message = "Success" };

            _mediatorMock
                .Setup(x => x.Send(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.TestView(request);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result as ViewResult;
            viewResult!.Model.Should().Be(expectedResponse);

            _mediatorMock.Verify(
                x => x.Send(request, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Json_ShouldCallMediatorAndReturnJsonResult()
        {
            // Arrange
            var request = new TestRequest();
            var expectedResponse = new TestResponse { Message = "JSON Response" };

            _mediatorMock
                .Setup(x => x.Send(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.TestJson(request);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            jsonResult!.Value.Should().Be(expectedResponse);

            _mediatorMock.Verify(
                x => x.Send(request, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task JsonAsync_ShouldCallMediatorAndReturnJsonResult()
        {
            // Arrange
            var request = new TestRequest();
            var expectedResponse = new TestResponse { Message = "Async JSON Response" };

            _mediatorMock
                .Setup(x => x.Send(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.TestJsonAsync(request);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            jsonResult!.Value.Should().Be(expectedResponse);

            _mediatorMock.Verify(
                x => x.Send(request, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task View_WhenMediatorThrowsException_ShouldPropagateException()
        {
            // Arrange
            var request = new TestRequest();
            var expectedException = new InvalidOperationException("Mediator error");

            _mediatorMock
                .Setup(x => x.Send(request, It.IsAny<CancellationToken>()))
                .ThrowsAsync(expectedException);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _controller.TestView(request));

            exception.Should().Be(expectedException);
        }

        [Fact]
        public async Task Json_WhenMediatorThrowsException_ShouldPropagateException()
        {
            // Arrange
            var request = new TestRequest();
            var expectedException = new InvalidOperationException("Mediator error");

            _mediatorMock
                .Setup(x => x.Send(request, It.IsAny<CancellationToken>()))
                .ThrowsAsync(expectedException);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _controller.TestJson(request));

            exception.Should().Be(expectedException);
        }

        [Fact]
        public async Task JsonAsync_WhenMediatorThrowsException_ShouldPropagateException()
        {
            // Arrange
            var request = new TestRequest();
            var expectedException = new InvalidOperationException("Mediator error");

            _mediatorMock
                .Setup(x => x.Send(request, It.IsAny<CancellationToken>()))
                .ThrowsAsync(expectedException);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _controller.TestJsonAsync(request));

            exception.Should().Be(expectedException);
        }
    }
}

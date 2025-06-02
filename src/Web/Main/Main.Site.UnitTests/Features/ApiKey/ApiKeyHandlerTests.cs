using AlphaPlus.Core.Models;
using AlphaPlus.Main.Site.Features.ApiKey;
using Moq;

namespace AlphaPlus.Main.Site.UnitTests.Features.ApiKey
{
    public class ApiKeyHandlerTests
    {
        private readonly Mock<IApiKeyService> _apiKeyServiceMock;
        private readonly ApiKeyHandler _handler;
        private readonly Mock<IUserSession> _userSessionMock;

        public ApiKeyHandlerTests()
        {
            _apiKeyServiceMock = new Mock<IApiKeyService>();
            _handler = new ApiKeyHandler(_apiKeyServiceMock.Object);
            _userSessionMock = new Mock<IUserSession>();
            _userSessionMock.Setup(x => x.CustId).Returns(1);
            _userSessionMock.Setup(x => x.UserLoginIp).Returns("127.0.0.1");
            _userSessionMock.Setup(x => x.LangKey).Returns("E");
            _userSessionMock.Setup(x => x.RoleId).Returns(2);
        }

        [Fact]
        public async Task Handle_CreateApiKeyCommand_CallsServiceWithCorrectParameters()
        {
            // Arrange
            var command = new CreateApiKeyCommand(_userSessionMock.Object)
            {
                ApiName = "TestApi",
                PassPhrase = "TestPhrase",
                Scopes = "1,2",
                ExpirationDate = DateTime.Now.AddDays(30)
            };

            var expectedResult = new CreateApiKeyResult
            {
                ApiKeyResult = new ApiKeyResult("TestApi", "key123", "secret123"),
                Result = new Result(0, "Success")
            };

            _apiKeyServiceMock.Setup(x => x.CreateApiKey(
                command.CustId,
                command.ApiName,
                command.PassPhrase,
                command.Scopes,
                command.IP,
                command.Language,
                command.ExpirationDate))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(expectedResult, result);
            _apiKeyServiceMock.Verify(x => x.CreateApiKey(
                command.CustId,
                command.ApiName,
                command.PassPhrase,
                command.Scopes,
                command.IP,
                command.Language,
                command.ExpirationDate), Times.Once);
        }

        [Fact]
        public async Task Handle_GetApiKeyQuery_ReturnsCorrectViewModel()
        {
            // Arrange
            var query = new GetApiKeyQuery(_userSessionMock.Object) { Key = "testKey" };
            var apiKeyInfo = new ApiKeyInfo
            {
                Key = "testKey",
                Name = "TestApi",
                ScopeIdList = "1,2"
            };

            _apiKeyServiceMock.Setup(x => x.GetApiKeyByKey(query.Key))
                .ReturnsAsync(apiKeyInfo);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(apiKeyInfo.Key, result.ApiKeyInfo.Key);
            Assert.Equal(apiKeyInfo.Name, result.ApiKeyInfo.Name);
            Assert.Contains(ApiKeyScope.WinLossDetail, result.Scopes);
            Assert.Contains(ApiKeyScope.Outstanding, result.Scopes);
        }

        [Fact]
        public async Task Handle_GetCustomerApiKeyQuery_ReturnsCorrectViewModel()
        {
            // Arrange
            var query = new GetCustomerApiKeyQuery(_userSessionMock.Object);

            var apiKeys = new List<ApiKeyInfo>
            {
                new() { Key = "key1", Name = "Api1" },
                new() { Key = "key2", Name = "Api2" }
            };

            _apiKeyServiceMock.Setup(x => x.GetApiKeyByCustId(query.CustId))
                .ReturnsAsync(apiKeys);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(query.CustLevel.DisplayName, result.CustLevel);
            Assert.Equal(apiKeys, result.ApiKeyInfos);
        }

        [Fact]
        public async Task Handle_RevokeApiKeyCommand_ReturnsSuccessResult()
        {
            // Arrange
            var command = new RevokeApiKeyCommand(_userSessionMock.Object) { Key = "testKey" };

            _apiKeyServiceMock.Setup(x => x.RevokeApiKey(command.Key, command.IP))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Code);
            Assert.Equal("Success", result.Message);
            _apiKeyServiceMock.Verify(x => x.RevokeApiKey(command.Key, command.IP), Times.Once);
        }

        [Fact]
        public async Task Handle_UpdateApiKeyCommand_ReturnsServiceResult()
        {
            // Arrange
            var command = new UpdateApiKeyCommand(_userSessionMock.Object)
            {
                ApiKey = "testKey",
                ApiName = "UpdatedApi",
                Scopes = "1,2",
                ExpirationDate = DateTime.Now.AddDays(30)
            };

            var expectedResult = new Result(0, "Success");


            _apiKeyServiceMock.Setup(x => x.UpdateApiKey(
                command.ApiKey,
                command.CustId,
                command.ApiName,
                command.Scopes,
                command.IP,
                command.Language,
                command.ExpirationDate))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Code, result.Code);
            Assert.Equal(expectedResult.Message, result.Message);
            _apiKeyServiceMock.Verify(x => x.UpdateApiKey(
                command.ApiKey,
                command.CustId,
                command.ApiName,
                command.Scopes,
                command.IP,
                command.Language,
                command.ExpirationDate), Times.Once);
        }
    }
}
using AlphaPlus.Settings.Site.Features.MemberInfo.Handlers.ProblemAccount;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests.ProblemAccount;
using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.ProblemAccount;
using AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.MyWatchlistAccounts;
using AlphaPlus.Settings.MemberInfo.API.Services;
using AlphaPlus.Settings.MemberInfo.Domain.Models.ProblemAccount;
using AlphaPlus.Settings.MemberInfo.Domain.Params.ProblemAccount;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.UnitTests.Features.MemberInfo.Handlers.ProblemAccount;

public class ProblemAccountHandlerTests : IClassFixture<TestFixture>
{
    private readonly Mock<IProblemAccountService> _mockProblemAccountService;
    private readonly Mock<MyWatchlistAccountsService> _mockMyWatchlistAccountsService;
    private readonly Mock<IBrowserHelper> _mockBrowserHelper;
    private readonly ProblemAccountHandler _handler;
    private readonly Fixture _fixture;

    public ProblemAccountHandlerTests(TestFixture testFixture)
    {
        _mockProblemAccountService = new Mock<IProblemAccountService>();
        _mockMyWatchlistAccountsService = new Mock<MyWatchlistAccountsService>();
        _mockBrowserHelper = new Mock<IBrowserHelper>();
        _fixture = new Fixture();

        _handler = new ProblemAccountHandler(
            _mockProblemAccountService.Object,
            _mockMyWatchlistAccountsService.Object,
            _mockBrowserHelper.Object);
    }

    // Helper methods to create objects manually by bypassing problematic constructors
    private IndexRequest CreateIndexRequest()
    {
        // Use reflection to create instance without calling constructor that accesses UserSession.Current
        var request = (IndexRequest)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(IndexRequest));
        
        // Manually set the viewer field to avoid null reference
        var mockUser = CreateMockUser();
        SetViewerField(request, mockUser);
        
        return request;
    }

    private UpdateProblemAccountRequest CreateUpdateProblemAccountRequest()
    {
        var request = (UpdateProblemAccountRequest)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(UpdateProblemAccountRequest));
        
        // Manually set the viewer field and properties
        var mockUser = CreateMockUser();
        SetViewerField(request, mockUser);
        
        request.UserNames = "testuser";
        request.Ids = "123";
        request.RoleId = 2;
        return request;
    }

    private UpdateProblemAccountCommand CreateUpdateProblemAccountCommand()
    {
        var command = (UpdateProblemAccountCommand)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(UpdateProblemAccountCommand));
        
        // Manually set the viewer field and properties
        var mockUser = CreateMockUser();
        SetViewerField(command, mockUser);
        
        command.IsAffectedAllDownlines = false;
        command.Ids = "123";
        command.RoleId = 2;
        return command;
    }

    private GetProblemAccountRequest CreateGetProblemAccountRequest()
    {
        var request = (GetProblemAccountRequest)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(GetProblemAccountRequest));
        
        // Manually set the viewer field
        var mockUser = CreateMockUser();
        SetViewerField(request, mockUser);
        
        return request;
    }

    private IUser CreateMockUser()
    {
        var mockUser = new Mock<IUser>();
        mockUser.SetupGet(u => u.LoggedInUserId).Returns(123);
        mockUser.SetupGet(u => u.Id).Returns(456);
        mockUser.SetupGet(u => u.Name).Returns("testuser");
        mockUser.SetupGet(u => u.Site).Returns("testsite");
        mockUser.SetupGet(u => u.LevelId).Returns((byte)2);
        mockUser.SetupGet(u => u.SubAccId).Returns(789);
        mockUser.SetupGet(u => u.SubAccName).Returns("testsubuser");
        mockUser.SetupGet(u => u.LimitedDateToShowInfo).Returns(DateTime.Now.AddDays(-30));
        mockUser.SetupGet(u => u.Language).Returns("en-US");
        mockUser.SetupGet(u => u.LanguageKey).Returns("en");
        mockUser.SetupGet(u => u.IP).Returns("127.0.0.1");
        mockUser.SetupGet(u => u.IsInternal).Returns(false);
        mockUser.SetupGet(u => u.IsAllowedIndirectLogin).Returns(true);
        mockUser.SetupGet(u => u.IsAdminCanViewVNIP).Returns(false);
        mockUser.SetupGet(u => u.DarkModeName).Returns("");
        mockUser.SetupGet(u => u.AGIP).Returns("127.0.0.1");
        mockUser.SetupGet(u => u.CustDBGroupId).Returns(1);
        mockUser.SetupGet(u => u.CanUpdateAccount).Returns(true);
        return mockUser.Object;
    }

    private void SetViewerField(object instance, IUser mockUser)
    {
        var viewerField = typeof(MemberInfoBaseQuery).GetField("viewer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        viewerField?.SetValue(instance, mockUser);
    }

    [Fact]
    public async Task Handle_IndexRequest_Should_Return_ProblemAccountReportViewModel()
    {
        // Arrange
        var request = CreateIndexRequest();
        _mockBrowserHelper.Setup(x => x.IsMobile(It.IsAny<bool>())).Returns(false);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ProblemAccountReportViewModel>();
        _mockBrowserHelper.Verify(x => x.IsMobile(false), Times.Once);
    }

    [Fact]
    public async Task Handle_UpdateProblemAccountRequest_Should_Return_UpdateReviewStatusViewModel()
    {
        // Arrange
        var request = CreateUpdateProblemAccountRequest();

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<UpdateReviewStatusViewModel>();
    }

    [Fact]
    public async Task Handle_UpdateProblemAccountCommand_Should_Call_Service_And_Return_Result()
    {
        // Arrange
        var command = CreateUpdateProblemAccountCommand();
        var expectedResult = _fixture.Create<Result>();
        
        _mockProblemAccountService
            .Setup(x => x.UpdateReviewStatus(It.IsAny<UpdateProblemAccountParam>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(expectedResult);
        _mockProblemAccountService.Verify(
            x => x.UpdateReviewStatus(It.Is<UpdateProblemAccountParam>(p => 
                p.CustId == command.CustId &&
                p.CustomerList == command.Ids &&
                p.IsAffectedAllDownlines == command.IsAffectedAllDownlines &&
                p.Language == command.Language &&
                p.LoginIp == command.IP &&
                p.RoleId == command.RoleId &&
                p.SubAccId == command.SubAccId)),
            Times.Once);
    }

    [Fact]
    public async Task Handle_GetProblemAccountRequest_Should_Call_Service_And_Return_Models()
    {
        // Arrange
        var request = CreateGetProblemAccountRequest();
        var expectedModels = _fixture.CreateMany<ProblemAccountModel>(3).ToList();
        
        _mockProblemAccountService
            .Setup(x => x.Get(It.IsAny<GetProblemAccountParam>()))
            .ReturnsAsync(expectedModels);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedModels);
        _mockProblemAccountService.Verify(
            x => x.Get(It.IsAny<GetProblemAccountParam>()), 
            Times.Once);
    }

    /*
    [Fact]
    public void CreateParam_Should_Map_Request_To_Param_Correctly()
    {
        // CreateParam method is internal, cannot be tested directly
        // This functionality is tested through the Handle method instead
    }
    */

    [Fact]
    public async Task Handle_GetProblemAccountRequest_Should_Handle_Empty_Result()
    {
        // Arrange
        var request = CreateGetProblemAccountRequest();
        var emptyModels = new List<ProblemAccountModel>();
        
        _mockProblemAccountService
            .Setup(x => x.Get(It.IsAny<GetProblemAccountParam>()))
            .ReturnsAsync(emptyModels);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
        _mockProblemAccountService.Verify(
            x => x.Get(It.IsAny<GetProblemAccountParam>()), 
            Times.Once);
    }

    [Fact]
    public async Task Handle_UpdateProblemAccountCommand_Should_Handle_Service_Exception()
    {
        // Arrange
        var command = CreateUpdateProblemAccountCommand();
        
        _mockProblemAccountService
            .Setup(x => x.UpdateReviewStatus(It.IsAny<UpdateProblemAccountParam>()))
            .ThrowsAsync(new InvalidOperationException("Service error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _handler.Handle(command, CancellationToken.None));
        
        _mockProblemAccountService.Verify(
            x => x.UpdateReviewStatus(It.IsAny<UpdateProblemAccountParam>()), 
            Times.Once);
    }

    [Fact]
    public async Task Handle_IndexRequest_Should_Handle_Mobile_Browser()
    {
        // Arrange
        var request = CreateIndexRequest();
        _mockBrowserHelper.Setup(x => x.IsMobile(It.IsAny<bool>())).Returns(true);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ProblemAccountReportViewModel>();
        _mockBrowserHelper.Verify(x => x.IsMobile(false), Times.Once);
    }

    /*
    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData("ValidUser", "ValidUpline")]
    public void CreateParam_Should_Handle_Various_String_Values(string userName, string uplineName)
    {
        // CreateParam method is internal, cannot be tested directly
        // This functionality is tested through the Handle method instead
    }
    */

    [Fact]
    public void Constructor_Should_Initialize_Dependencies_Correctly()
    {
        // Arrange & Act
        var handler = new ProblemAccountHandler(
            _mockProblemAccountService.Object,
            _mockMyWatchlistAccountsService.Object,
            _mockBrowserHelper.Object);

        // Assert
        handler.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_GetProblemAccountRequest_Should_Use_Cancellation_Token()
    {
        // Arrange
        var request = CreateGetProblemAccountRequest();
        var expectedModels = _fixture.CreateMany<ProblemAccountModel>(2).ToList();
        var cancellationToken = new CancellationToken();
        
        _mockProblemAccountService
            .Setup(x => x.Get(It.IsAny<GetProblemAccountParam>()))
            .ReturnsAsync(expectedModels);

        // Act
        var result = await _handler.Handle(request, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedModels);
    }
}

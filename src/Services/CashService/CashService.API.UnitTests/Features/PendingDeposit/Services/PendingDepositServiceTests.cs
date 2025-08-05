using AlphaPlus.CashService.API.Features.PendingDeposit.Criteria;
using AlphaPlus.CashService.API.Features.PendingDeposit.Models;
using AlphaPlus.CashService.API.Features.PendingDeposit.Services;
using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Features.Transactions.Models;
using AlphaPlus.CashService.API.Features.Transactions.Services;
using Fanex.Data.Repository;
using Moq;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.PendingDeposit.Services;

public class PendingDepositServiceTests
{
    private readonly Mock<IDynamicRepository> mockDynamicRepository;
    private readonly Mock<ITransactionService> mockTransactionService;
    private readonly IPendingDepositService pendingDepositService;

    public PendingDepositServiceTests()
    {
        mockDynamicRepository = new Mock<IDynamicRepository>();
        mockTransactionService = new Mock<ITransactionService>();
        pendingDepositService = new PendingDepositService(mockDynamicRepository.Object, mockTransactionService.Object);
    }

    [Fact]
    public async Task GetAccumulateCreditAsync_ShouldReturnAccumulatedCredit()
    {
        // Arrange
        var agentId = 1;
        var expectedCredit = 100.50m;

        mockDynamicRepository
            .Setup(repo => repo.GetAsync<decimal>(It.IsAny<GetAccumulateCreditCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback((ICriteria criteria, string s, CancellationToken ct) =>
            {
                ((GetAccumulateCreditCriteria)criteria).AccumCreditReturn = expectedCredit;
            })
            .ReturnsAsync(expectedCredit); // The return value of GetAsync itself isn't directly used by the method for its final result here

        // Act
        var result = await ((PendingDepositService)pendingDepositService).GetAccumulateCreditAsync(agentId); // Cast to access public method if it were private

        // Assert
        Assert.Equal(expectedCredit, result);
        mockDynamicRepository.Verify(repo => repo.GetAsync<decimal>(
            It.Is<GetAccumulateCreditCriteria>(c => c.CustId == agentId),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetLimitAmtDepositAsync_ShouldReturnAgentAvailableCredit()
    {
        // Arrange
        var agentId = 1;
        var custId = 2;
        var custDbGroupId = 3;
        var accumCreditReturn = 100m;
        var expectedAvailableCredit = 50m;

        var pendingDepositService = new PendingDepositService(mockDynamicRepository.Object, mockTransactionService.Object);
        // Mock GetAccumulateCreditAsync behavior (as it's called internally by GetLimitAmtDepositAsync)
        mockDynamicRepository
            .Setup(repo => repo.GetAsync<decimal>(It.Is<GetAccumulateCreditCriteria>(c => c.CustId == agentId), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback((ICriteria criteria, string s, CancellationToken ct) =>
            {
                ((GetAccumulateCreditCriteria)criteria).AccumCreditReturn = accumCreditReturn;
            })
            .ReturnsAsync(accumCreditReturn);

        mockDynamicRepository
            .Setup(repo => repo.GetAsync<CreditAmount>(It.IsAny<GetLimitAmtDepositCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback((ICriteria criteria, string s, CancellationToken ct) =>
            {
                ((GetLimitAmtDepositCriteria)criteria).AgentAvailableCredit = expectedAvailableCredit;
            })
            .ReturnsAsync(new CreditAmount()); // Return type of GetAsync<CreditAmount>

        // Act
        var result = await this.pendingDepositService.GetLimitAmtDepositAsync(agentId, custId, custDbGroupId);

        // Assert
        Assert.Equal(expectedAvailableCredit, result);
        mockDynamicRepository.Verify(repo => repo.GetAsync<CreditAmount>(
            It.Is<GetLimitAmtDepositCriteria>(c =>
                c.CustId == custId &&
                c.AccumCreditReturn == accumCreditReturn),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddManualDepositAsync_ShouldReturnResultModelAndCallTransactionService()
    {
        // Arrange
        var param = new CreateManualDepositModel
        {
            CustId = 1,
            UplineId = 2,
            SubAccId = 3,
            SiteDbGroupId = 4,
            CustDbGroupId = 5,
            PaymentFrom = "From",
            PaymentTo = "To",
            Amount = 100,
            PaySlip = [1],
            Ip = "ip",
            Remark = "remark"
        };
        var memberAgentCredit = 50m;
        var memberCreditMemberCredit = 25m;
        var expectedLogs = new List<WriteLogDBCommandModel> { new() };
        var errCode = 0;
        var errMsg = "Success";
        var cashAccumTbl = "tbl_cash_accum";

        // Mock GetMemberSettingDepositAsync
        mockDynamicRepository
            .Setup(repo => repo.GetAsync<MemberSettingDeposit>(It.Is<GetMemberSettingDepositCriteria>(c => c.CustId == param.CustId && c.CustDbGroupId == param.CustDbGroupId), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback((ICriteria c, string s, CancellationToken ct) =>
            {
                ((GetMemberSettingDepositCriteria)c).AgentGivenCredit = memberAgentCredit;
                ((GetMemberSettingDepositCriteria)c).CreditMembers_GivenCredit = memberCreditMemberCredit;
            })
            .ReturnsAsync(new MemberSettingDeposit());

        mockDynamicRepository
            .Setup(repo => repo.FetchAsync<WriteLogDBCommandModel>(It.IsAny<CreateManualDepositCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback((ICriteria criteria, string s, CancellationToken ct) =>
            {
                var cmd = (CreateManualDepositCommand)criteria;
                cmd.ErrCode = errCode;
                cmd.ErrMsg = errMsg;
                cmd.CashAccumCreditTbl = cashAccumTbl;
            })
            .ReturnsAsync(expectedLogs);

        // Act
        var result = await pendingDepositService.AddManualDepositAsync(param);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(errCode, result.Code);
        Assert.Equal(errMsg, result.Message);
        mockDynamicRepository.Verify(repo => repo.FetchAsync<WriteLogDBCommandModel>(
            It.Is<CreateManualDepositCommand>(c =>
                c.CustId == param.CustId &&
                c.AgentGivenCredit == memberAgentCredit &&
                c.CreditMembers_GivenCredit == memberCreditMemberCredit &&
                c.SiteDbGroupId == param.SiteDbGroupId
                ),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Once);
        mockTransactionService.Verify(ts => ts.OnAfterUpdateTransaction(cashAccumTbl, param.SiteDbGroupId, param.CustDbGroupId, expectedLogs), Times.Once);
    }

    [Fact]
    public async Task ProcessDepositAsync_ShouldReturnResultModel()
    {
        // Arrange
        var param = new ProcessDepositTransaction { TransId = 123, NewAmount = 100, Remark = "remark", StatusId = 1, ActorId = 1, CustId = 1 };
        var errCode = 0;
        var errMsg = "Success";

        mockDynamicRepository
            .Setup(repo => repo.FetchAsync<WriteLogDBCommandModel>(It.IsAny<ProcessDepositCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback((ICriteria criteria, string s, CancellationToken ct) =>
            {
                var cmd = (ProcessDepositCommand)criteria;
                cmd.ErrCode = errCode;
                cmd.ErrMsg = errMsg;
            })
            .ReturnsAsync(new List<WriteLogDBCommandModel>());

        // Act
        var result = await pendingDepositService.ProcessDepositAsync(param);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(errCode, result.Code);
        Assert.Equal(errMsg, result.Message);
        mockDynamicRepository.Verify(repo => repo.FetchAsync<WriteLogDBCommandModel>(
            It.Is<ProcessDepositCommand>(c =>
                c.TransId == param.TransId &&
                c.NewAmount == param.NewAmount &&
                c.StatusId == param.StatusId &&
                c.CustId == param.CustId &&
                c.AgentId == param.ActorId),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
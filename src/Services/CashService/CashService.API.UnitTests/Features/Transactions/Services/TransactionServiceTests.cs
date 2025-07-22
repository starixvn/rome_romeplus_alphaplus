using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Features.Shared.Requests;
using AlphaPlus.CashService.API.Features.Shared.Services;
using AlphaPlus.CashService.API.Features.Transactions.Criteria;
using AlphaPlus.CashService.API.Features.Transactions.Models;
using AlphaPlus.CashService.API.Features.Transactions.Services;
using Fanex.Data.Repository;
using Fanex.Logging;
using Moq;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Transactions.Services;

public class TransactionServiceTests
{
    private readonly Mock<IDynamicRepository> mockDynamicRepository;
    private readonly Mock<ICommonService> mockCommonService;
    private readonly Mock<ILogger> mockLogger;
    private readonly ITransactionService transactionService;

    public TransactionServiceTests()
    {
        mockDynamicRepository = new Mock<IDynamicRepository>();
        mockCommonService = new Mock<ICommonService>();
        mockLogger = new Mock<ILogger>();
        transactionService = new TransactionService(mockDynamicRepository.Object, mockCommonService.Object, mockLogger.Object);
    }

    [Fact]
    public async Task GetTransactionsAsync_ShouldReturnTransactionResponse()
    {
        // Arrange
        var criteria = new GetAllTransactionsWithPagingCriteria { TotalRecords = 1 };
        var expectedTransactions = new List<TransactionModel> { new() { TransId = 123, Amount = 100 } };
        mockDynamicRepository
            .Setup(repo => repo.FetchAsync<TransactionModel>(criteria, It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedTransactions);

        // Act
        var result = await transactionService.GetTransactionsAsync(criteria);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedTransactions.Count, result.Items.Count());
        Assert.Equal(criteria.TotalRecords, result.TotalCount);
        Assert.Equal(expectedTransactions.First().TransId, result.Items.First().TransId);
        mockDynamicRepository.Verify(repo => repo.FetchAsync<TransactionModel>(criteria, It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetTransactionByIdAsync_ShouldReturnTransactionModel()
    {
        // Arrange
        long transactionId = 123;
        var custId = 1;
        short siteDbGroupId = 1;
        var expectedTransaction = new TransactionModel { TransId = transactionId, CustId = custId };

        mockDynamicRepository
            .Setup(repo => repo.GetAsync<TransactionModel>(It.IsAny<GetTransactionDetailCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedTransaction);

        // Act
        var result = await transactionService.GetTransactionByIdAsync(transactionId, custId, siteDbGroupId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedTransaction.TransId, result.TransId);
        Assert.Equal(expectedTransaction.CustId, result.CustId);
        mockDynamicRepository.Verify(repo => repo.GetAsync<TransactionModel>(
            It.Is<GetTransactionDetailCriteria>(c =>
                c.TransId == transactionId &&
                c.CustId == custId &&
                c.SiteDbGroupId == siteDbGroupId),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RevertDepositTransactionToPending_ShouldReturnResultModel()
    {
        // Arrange
        var command = new RevertDepositTransactionToPendingCommand { TransId = 1, ErrCode = 0, ErrMsg = "Success" };
        mockDynamicRepository
            .Setup(repo => repo.FetchAsync<WriteLogDBCommandModel>(command, It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback((ICriteria cmd, string s, CancellationToken ct) =>
            {
                ((RevertDepositTransactionToPendingCommand)cmd).ErrCode = 0;
                ((RevertDepositTransactionToPendingCommand)cmd).ErrMsg = "Success";
            });

        // Act
        var result = await transactionService.RevertDepositTransactionToPending(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.ErrCode, result.Code);
        Assert.Equal(command.ErrMsg, result.Message);
        mockDynamicRepository.Verify(repo => repo.FetchAsync<WriteLogDBCommandModel>(command, It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RevertWithdrawalTransactionToPending_ShouldReturnResultModel()
    {
        // Arrange
        var command = new RevertWithdrawalTransactionToPendingCommand { TransId = 1, ErrCode = 0, ErrMsg = "Success" };
        mockDynamicRepository
           .Setup(repo => repo.ExecuteAsync(command, It.IsAny<string>(), It.IsAny<CancellationToken>()))
           .Returns(Task.FromResult(1))
           .Callback((INonQueryCommand cmd, string s, CancellationToken ct) =>
           {
               ((RevertWithdrawalTransactionToPendingCommand)cmd).ErrCode = 0;
               ((RevertWithdrawalTransactionToPendingCommand)cmd).ErrMsg = "Success";
           });

        // Act
        var result = await transactionService.RevertWithdrawalTransactionToPending(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.ErrCode, result.Code);
        Assert.Equal(command.ErrMsg, result.Message);
        mockDynamicRepository.Verify(repo => repo.FetchAsync<WriteLogDBCommandModel>(command, It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetMembersAsync_ShouldReturnMemberInfoList()
    {
        // Arrange
        var agentId = 1;
        short custDbGroupId = 1;
        var expectedMembers = new List<MemberInfo> { new() { CustId = 10, Username = "TestUser" } };

        mockDynamicRepository
            .Setup(repo => repo.FetchAsync<MemberInfo>(It.IsAny<GetMembersCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedMembers);

        // Act
        var result = await transactionService.GetMembersAsync(agentId, custDbGroupId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedMembers.Count, result.Count());
        Assert.Equal(expectedMembers.First().Username, result.First().Username);
        mockDynamicRepository.Verify(repo => repo.FetchAsync<MemberInfo>(
            It.Is<GetMembersCriteria>(c =>
                c.CustId == agentId &&
                c.CustDbGroupId == custDbGroupId),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task OnAfterUpdateTransaction_WithValidLogs_ShouldAttemptSyncAndLog()
    {
        // Arrange
        var cashAccumCreditTbl = "tblAccum";
        short siteDbGroupId = 1;
        short custDbGroupId = 1;
        var logs = new List<WriteLogDBCommandModel>
    {
        new() { LogTypeId = 1, UserId = 1, FromCustId = 2, ToCustId = 3, Ip = "1.1.1.1", Doing = "Transfer" },
        new() { LogTypeId = 2, CustId = 4, UserId = 5, Ip = "Update Status" }
    };

        mockDynamicRepository
            .Setup(r => r.ExecuteAsync(It.IsAny<SyncCashAccumCreditCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback((INonQueryCommand cmd, string s, CancellationToken ct) =>
            {
                var syncCmd = (SyncCashAccumCreditCommand)cmd;
                syncCmd.ErrCode = 0; // Simulate success
            })
            .ReturnsAsync(1);

        mockDynamicRepository
            .Setup(r => r.ExecuteAsync(It.IsAny<InsertTransactionLogTransferCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        mockCommonService
            .Setup(s => s.WriteLogDBCommand(It.IsAny<BaseActionRequest>(), It.IsAny<WriteLogDBCommandModel>()))
            .Returns(Task.CompletedTask);

        // Act
        transactionService.OnAfterUpdateTransaction(cashAccumCreditTbl, siteDbGroupId, custDbGroupId, logs);
        await Task.Delay(100); // Give Task.Run a chance to execute

        // Assert
        mockDynamicRepository.Verify(r => r.ExecuteAsync(
            It.Is<SyncCashAccumCreditCommand>(c => c.CashAccumCreditTbl == cashAccumCreditTbl && c.CustDbGroupId == custDbGroupId),
            It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);

        mockDynamicRepository.Verify(r => r.ExecuteAsync(
            It.Is<InsertTransactionLogTransferCommand>(c => c.UserId == logs[0].UserId && c.SiteDbGroupId == -1),
            It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);

        mockCommonService.Verify(s => s.WriteLogDBCommand(
            It.Is<BaseActionRequest>(br => br.ActorId == logs[1].CustId && br.CustDBGroupId == custDbGroupId && br.UserId == logs[1].UserId),
            It.Is<WriteLogDBCommandModel>(l => l == logs[1])), Times.Once);

        mockLogger.Verify(l => l.ErrorAsync(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
    }

    [Fact]
    public async Task OnAfterUpdateTransaction_WithEmptyCashAccumCreditTbl_ShouldNotCallSync()
    {
        // Arrange
        var cashAccumCreditTbl = string.Empty;
        short siteDbGroupId = 1;
        short custDbGroupId = 1;
        var logs = new List<WriteLogDBCommandModel>
        {
            new() { LogTypeId = 1, UserId = 1, FromCustId = 2, ToCustId = 3, Ip = "1.1.1.1", Doing = "Transfer" }
        };

        // Act
        transactionService.OnAfterUpdateTransaction(cashAccumCreditTbl, siteDbGroupId, custDbGroupId, logs);
        await Task.Delay(100);

        // Assert
        mockDynamicRepository.Verify(r => r.ExecuteAsync(It.IsAny<SyncCashAccumCreditCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        mockDynamicRepository.Verify(r => r.ExecuteAsync(It.Is<InsertTransactionLogTransferCommand>(c => c.UserId == logs[0].UserId), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        mockLogger.Verify(l => l.ErrorAsync(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
    }

    [Fact]
    public async Task OnAfterUpdateTransaction_WhenSyncFails_ShouldLogError()
    {
        // Arrange
        var cashAccumCreditTbl = "tblAccum";
        short siteDbGroupId = 1;
        short custDbGroupId = 1;
        var logs = new List<WriteLogDBCommandModel>(); // No logs to simplify

        mockDynamicRepository
            .Setup(r => r.ExecuteAsync(It.IsAny<SyncCashAccumCreditCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback((INonQueryCommand cmd, string s, CancellationToken ct) =>
            {
                var syncCmd = (SyncCashAccumCreditCommand)cmd;
                syncCmd.ErrCode = 1; // Simulate error
                syncCmd.ErrMsg = "Sync Failed";
            })
            .ReturnsAsync(0);

        // Act
        transactionService.OnAfterUpdateTransaction(cashAccumCreditTbl, siteDbGroupId, custDbGroupId, logs);
        await Task.Delay(100);

        // Assert
        mockDynamicRepository.Verify(r => r.ExecuteAsync(It.Is<SyncCashAccumCreditCommand>(c => c.CashAccumCreditTbl == cashAccumCreditTbl && c.CustDbGroupId == custDbGroupId), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task OnAfterUpdateTransaction_WhenWriteTransactionLogThrowsFirstAttempt_ShouldRetryAndLogError()
    {
        // Arrange
        var cashAccumCreditTbl = "tblTest";
        short siteDbGroupId = 1;
        short custDbGroupId = 1;
        var logs = new List<WriteLogDBCommandModel>
        {
            new() { LogTypeId = 1, UserId = 1, FromCustId = 100, ToCustId = 200, Ip = "127.0.0.1", Doing = "Test Transfer" }
        };
        var exceptionMessage = "Simulated DB error on first attempt";

        mockDynamicRepository
            .SetupSequence(r => r.ExecuteAsync(It.IsAny<InsertTransactionLogTransferCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception(exceptionMessage)) // First call to InsertTransactionLogTransferCommand throws
            .ReturnsAsync(1); // Second call succeeds

        // Act
        transactionService.OnAfterUpdateTransaction(cashAccumCreditTbl, siteDbGroupId, custDbGroupId, logs);
        await Task.Delay(100);

        // Assert
        mockLogger.Verify(l => l.ErrorAsync(exceptionMessage, It.IsAny<Exception>()), Times.Once); // Original exception logged
        mockDynamicRepository.Verify(r => r.ExecuteAsync(It.Is<InsertTransactionLogTransferCommand>(c => c.SiteDbGroupId == -1), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once); // First attempt with SiteDbGroupId = -1
        mockDynamicRepository.Verify(r => r.ExecuteAsync(It.Is<InsertTransactionLogTransferCommand>(c => c.SiteDbGroupId == siteDbGroupId), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once); // Second attempt with actual siteDbGroupId
    }
}
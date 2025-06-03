using AlphaPlus.BetLists.Shared.Criteria;
using AlphaPlus.BetLists.Shared.DataAccesses;
using AlphaPlus.SmartPunter.Criteria;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.UnitTests.Shared.DataAccesses;

[Trait("Category", "BetLists")]
public class SmartPunterDataAccessTest
{
    private readonly Mock<IDynamicRepository> _repositoryMock;
    private readonly SmartPunterDataAccess _dataAccess;

    public SmartPunterDataAccessTest()
    {
        _repositoryMock = new Mock<IDynamicRepository>();
        _dataAccess = new SmartPunterDataAccess(_repositoryMock.Object);
    }

    [Fact]
    public void WriteLog_ShouldCallRepositoryExecuteWithCorrectCommand()
    {
        // Arrange
        string listUserName = "testUser1,testUser2";
        int actionId = 123;
        bool isRestricted = true;
        int userId = 456;
        string ip = "192.168.1.1";

        var commandData = new SmartPunterWriteLog(
            listUserName,
            actionId,
            isRestricted,
            userId,
            ip);

        // Act
        _dataAccess.WriteLog(commandData);

        // Assert
        _repositoryMock.Verify(r => r.Execute(It.Is<SmartPunterWriteLogCommand>(c =>
            c.ListUserName == listUserName &&
            c.ActionId == actionId &&
            c.IsRestricted == isRestricted &&
            c.UserId == userId &&
            c.IP == ip), string.Empty), Times.Once);
    }

    [Fact]
    public void WriteLogNoData_ShouldCallRepositoryExecuteWithCorrectCommand()
    {
        // Arrange
        int? custId = 12345;
        int actionId = 123;
        bool isRestricted = true;
        int userId = 456;
        string ip = "192.168.1.1";

        var commandData = new SmartPunterWriteLogNoData(
            custId,
            actionId,
            isRestricted,
            userId,
            ip);

        // Act
        _dataAccess.WriteLogNoData(commandData);

        // Assert
        _repositoryMock.Verify(r => r.Execute(It.Is<SmartPunterWriteLogNoDataCommand>(c =>
            c.CustId == custId &&
            c.ActionId == actionId &&
            c.IsRestricted == isRestricted &&
            c.UserId == userId &&
            c.IP == ip), string.Empty), Times.Once);
    }

    [Fact]
    public void WriteLogNoData_WithNullCustId_ShouldCallRepositoryExecuteWithCorrectCommand()
    {
        // Arrange
        int? custId = null;
        int actionId = 123;
        bool isRestricted = true;
        int userId = 456;
        string ip = "192.168.1.1";

        var commandData = new SmartPunterWriteLogNoData(
            custId,
            actionId,
            isRestricted,
            userId,
            ip);

        // Act
        _dataAccess.WriteLogNoData(commandData);

        // Assert
        _repositoryMock.Verify(r => r.Execute(It.Is<SmartPunterWriteLogNoDataCommand>(c =>
            c.CustId == null &&
            c.ActionId == actionId &&
            c.IsRestricted == isRestricted &&
            c.UserId == userId &&
            c.IP == ip), string.Empty), Times.Once);
    }

    [Fact]
    public void WriteLog_WithEmptyListUserName_ShouldStillCallRepositoryExecute()
    {
        // Arrange
        string listUserName = string.Empty;
        int actionId = 123;
        bool isRestricted = true;
        int userId = 456;
        string ip = "192.168.1.1";

        var commandData = new SmartPunterWriteLog(
            listUserName,
            actionId,
            isRestricted,
            userId,
            ip);

        // Act
        _dataAccess.WriteLog(commandData);

        // Assert
        _repositoryMock.Verify(r => r.Execute(It.Is<SmartPunterWriteLogCommand>(c =>
            c.ListUserName == listUserName &&
            c.ActionId == actionId &&
            c.IsRestricted == isRestricted &&
            c.UserId == userId &&
            c.IP == ip), string.Empty), Times.Once);
    }
}
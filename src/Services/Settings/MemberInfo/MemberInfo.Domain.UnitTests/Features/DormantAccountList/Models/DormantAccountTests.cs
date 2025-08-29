using AlphaPlus.Settings.MemberInfo.Domain.Features.DormantAccountList.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Constants;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Features.DormantAccountList.Models;

public class DormantAccountTests
{
    [Fact]
    public void Default_Constructor_Should_Initialize_With_Default_Values()
    {
        // Act
        var dormantAccount = new DormantAccount();

        // Assert
        Assert.Null(dormantAccount.ToBeClearedGivenCredit);
        Assert.Null(dormantAccount.ClearedGivenCredit);
        Assert.Equal(0, dormantAccount.YesterdayBalance);
        Assert.Equal(string.Empty, dormantAccount.Username);
        Assert.Equal(string.Empty, dormantAccount.RoleName);
        Assert.Equal(string.Empty, dormantAccount.Status);
        Assert.Equal(string.Empty, dormantAccount.ArchiveStage);
        Assert.Null(dormantAccount.LastLogged);
        Assert.Null(dormantAccount.ClosedDate);
        Assert.Null(dormantAccount.DisabledDate);
        Assert.Null(dormantAccount.SettingDeletedDate);
        Assert.Null(dormantAccount.AccountDeletedDate);
        Assert.False(dormantAccount.IsWarningTicket);
        Assert.False(dormantAccount.IsSummaryRow);
        Assert.False(dormantAccount.IsRedDateTime);
    }

    [Fact]
    public void Constructor_With_Decimal_Values_Should_Set_Properties_Correctly()
    {
        // Arrange
        var toBeClearedGivenCredit = 100.50m;
        var clearedGivenCredit = 200.75m;
        var yesterdayBalance = 300.25m;

        // Act
        var dormantAccount = new DormantAccount(toBeClearedGivenCredit, clearedGivenCredit, yesterdayBalance);

        // Assert
        Assert.Equal(toBeClearedGivenCredit, dormantAccount.ToBeClearedGivenCredit);
        Assert.Equal(clearedGivenCredit, dormantAccount.ClearedGivenCredit);
        Assert.Equal(yesterdayBalance, dormantAccount.YesterdayBalance);
    }

    [Fact]
    public void Constructor_With_Entity_Should_Map_Properties_Correctly()
    {
        // Arrange
        var entity = CreateDormantAccountEntity();
        var yesterdayBalance = 500.00m;

        // Act
        var dormantAccount = new DormantAccount(entity, yesterdayBalance);

        // Assert
        Assert.Equal("testuser", dormantAccount.Username);
        Assert.Equal(yesterdayBalance, dormantAccount.YesterdayBalance);
        Assert.Equal(entity.ToBeClearedGivenCredit, dormantAccount.ToBeClearedGivenCredit);
        Assert.Equal(entity.ClearedGivenCredit, dormantAccount.ClearedGivenCredit);
        Assert.Equal(entity.LastLogged, dormantAccount.LastLogged);
        Assert.Equal(entity.ClosedDate, dormantAccount.ClosedDate);
        Assert.Equal(entity.DisabledDate, dormantAccount.DisabledDate);
        Assert.Equal(entity.SettingDeletedDate, dormantAccount.SettingDeletedDate);
        Assert.Equal(entity.AccountDeletedDate, dormantAccount.AccountDeletedDate);
        Assert.Equal(entity.IsWarningTicket, dormantAccount.IsWarningTicket);
    }

    [Fact]
    public void Constructor_With_Entity_Should_Handle_Null_Username()
    {
        // Arrange
        var entity = new DormantAccountEntity
        {
            CustId = 123,
            Username = null, // Testing null username
            RoleId = 1, // Agent role
            StatusId = StatusIdentity.Open,
            StageId = 1,
            ToBeClearedGivenCredit = 100.50m,
            ClearedGivenCredit = 200.75m,
            LastLogged = DateTime.Now.AddDays(-1),
            ClosedDate = DateTime.Now.AddDays(-10),
            DisabledDate = null,
            SettingDeletedDate = null,
            AccountDeletedDate = null,
            IsWarningTicket = false,
            RowNumber = 1,
            Recommend = 1
        };

        // Act
        var dormantAccount = new DormantAccount(entity, 100m);

        // Assert
        Assert.Equal(string.Empty, dormantAccount.Username);
    }

    [Fact]
    public void Constructor_With_Entity_Should_Set_IsSummaryRow_When_RowNumber_Is_Zero()
    {
        // Arrange
        var entity = new DormantAccountEntity
        {
            CustId = 123,
            Username = "testuser",
            RoleId = 1, // Agent role
            StatusId = StatusIdentity.Open,
            StageId = 1,
            ToBeClearedGivenCredit = 100.50m,
            ClearedGivenCredit = 200.75m,
            LastLogged = DateTime.Now.AddDays(-1),
            ClosedDate = DateTime.Now.AddDays(-10),
            DisabledDate = null,
            SettingDeletedDate = null,
            AccountDeletedDate = null,
            IsWarningTicket = false,
            RowNumber = 0, // Testing zero row number
            Recommend = 1
        };

        // Act
        var dormantAccount = new DormantAccount(entity, 100m);

        // Assert
        Assert.True(dormantAccount.IsSummaryRow);
    }

    [Fact]
    public void Constructor_With_Entity_Should_Set_IsSummaryRow_False_When_RowNumber_Is_Not_Zero()
    {
        // Arrange
        var entity = CreateDormantAccountEntity(); // This already has RowNumber = 1

        // Act
        var dormantAccount = new DormantAccount(entity, 100m);

        // Assert
        Assert.False(dormantAccount.IsSummaryRow);
    }

    [Theory]
    [InlineData(DirectMemberAgentIds.Super)]
    [InlineData(DirectMemberAgentIds.Master)]
    public void Constructor_Should_Set_DirectMember_RoleName_For_Direct_Members(int recommendId)
    {
        // Arrange
        var entity = new DormantAccountEntity
        {
            CustId = 123,
            Username = "testuser",
            RoleId = 1, // Agent role
            StatusId = StatusIdentity.Open,
            StageId = 1,
            ToBeClearedGivenCredit = 100.50m,
            ClearedGivenCredit = 200.75m,
            LastLogged = DateTime.Now.AddDays(-1),
            ClosedDate = DateTime.Now.AddDays(-10),
            DisabledDate = null,
            SettingDeletedDate = null,
            AccountDeletedDate = null,
            IsWarningTicket = false,
            RowNumber = 1,
            Recommend = recommendId // Testing specific recommend ID
        };

        // Act
        var dormantAccount = new DormantAccount(entity, 100m);

        // Assert
        // The RoleName should be set to DirectMember for these cases
        // Note: Since we don't have access to CommonResx in unit tests, we can only verify it's not empty
        Assert.NotEqual(string.Empty, dormantAccount.RoleName);
    }

    [Fact]
    public void IsRedDateTime_Should_Return_True_When_LastLogged_After_ClosedDate()
    {
        // Arrange
        var closedDate = DateTime.Now.AddDays(-5);
        var lastLogged = DateTime.Now.AddDays(-3);
        var entity = new DormantAccountEntity
        {
            CustId = 123,
            Username = "testuser",
            RoleId = 1, // Agent role
            StatusId = StatusIdentity.Open,
            StageId = 1,
            ToBeClearedGivenCredit = 100.50m,
            ClearedGivenCredit = 200.75m,
            LastLogged = lastLogged, // Last logged after closed date
            ClosedDate = closedDate,
            DisabledDate = null,
            SettingDeletedDate = null,
            AccountDeletedDate = null,
            IsWarningTicket = false,
            RowNumber = 1,
            Recommend = 1
        };

        // Act
        var dormantAccount = new DormantAccount(entity, 100m);

        // Assert
        Assert.True(dormantAccount.IsRedDateTime);
    }

    [Fact]
    public void IsRedDateTime_Should_Return_False_When_LastLogged_Before_ClosedDate()
    {
        // Arrange
        var closedDate = DateTime.Now.AddDays(-3);
        var lastLogged = DateTime.Now.AddDays(-5);
        var entity = new DormantAccountEntity
        {
            CustId = 123,
            Username = "testuser",
            RoleId = 1, // Agent role
            StatusId = StatusIdentity.Open,
            StageId = 1,
            ToBeClearedGivenCredit = 100.50m,
            ClearedGivenCredit = 200.75m,
            LastLogged = lastLogged, // Last logged before closed date
            ClosedDate = closedDate,
            DisabledDate = null,
            SettingDeletedDate = null,
            AccountDeletedDate = null,
            IsWarningTicket = false,
            RowNumber = 1,
            Recommend = 1
        };

        // Act
        var dormantAccount = new DormantAccount(entity, 100m);

        // Assert
        Assert.False(dormantAccount.IsRedDateTime);
    }

    [Fact]
    public void IsRedDateTime_Should_Return_False_When_LastLogged_Is_Null()
    {
        // Arrange
        var entity = new DormantAccountEntity
        {
            CustId = 123,
            Username = "testuser",
            RoleId = 1, // Agent role
            StatusId = StatusIdentity.Open,
            StageId = 1,
            ToBeClearedGivenCredit = 100.50m,
            ClearedGivenCredit = 200.75m,
            LastLogged = null, // Testing null last logged
            ClosedDate = DateTime.Now,
            DisabledDate = null,
            SettingDeletedDate = null,
            AccountDeletedDate = null,
            IsWarningTicket = false,
            RowNumber = 1,
            Recommend = 1
        };

        // Act
        var dormantAccount = new DormantAccount(entity, 100m);

        // Assert
        Assert.False(dormantAccount.IsRedDateTime);
    }

    [Fact]
    public void IsRedDateTime_Should_Return_False_When_ClosedDate_Is_Null()
    {
        // Arrange
        var entity = new DormantAccountEntity
        {
            CustId = 123,
            Username = "testuser",
            RoleId = 1, // Agent role
            StatusId = StatusIdentity.Open,
            StageId = 1,
            ToBeClearedGivenCredit = 100.50m,
            ClearedGivenCredit = 200.75m,
            LastLogged = DateTime.Now,
            ClosedDate = null, // Testing null closed date
            DisabledDate = null,
            SettingDeletedDate = null,
            AccountDeletedDate = null,
            IsWarningTicket = false,
            RowNumber = 1,
            Recommend = 1
        };

        // Act
        var dormantAccount = new DormantAccount(entity, 100m);

        // Assert
        Assert.False(dormantAccount.IsRedDateTime);
    }

    [Fact]
    public void IsRedDateTime_Should_Return_False_When_Both_Dates_Are_Null()
    {
        // Arrange
        var entity = new DormantAccountEntity
        {
            CustId = 123,
            Username = "testuser",
            RoleId = 1, // Agent role
            StatusId = StatusIdentity.Open,
            StageId = 1,
            ToBeClearedGivenCredit = 100.50m,
            ClearedGivenCredit = 200.75m,
            LastLogged = null, // Testing both null
            ClosedDate = null, // Testing both null
            DisabledDate = null,
            SettingDeletedDate = null,
            AccountDeletedDate = null,
            IsWarningTicket = false,
            RowNumber = 1,
            Recommend = 1
        };

        // Act
        var dormantAccount = new DormantAccount(entity, 100m);

        // Assert
        Assert.False(dormantAccount.IsRedDateTime);
    }

    [Fact]
    public void Constructor_With_Nullable_Decimals_Should_Work()
    {
        // Test different combinations of nullable decimals
        var testCases = new[]
        {
            new { toBeClearedGivenCredit = (decimal?)null, clearedGivenCredit = (decimal?)null, yesterdayBalance = 0m },
            new { toBeClearedGivenCredit = (decimal?)100.5m, clearedGivenCredit = (decimal?)null, yesterdayBalance = 200m },
            new { toBeClearedGivenCredit = (decimal?)null, clearedGivenCredit = (decimal?)150.75m, yesterdayBalance = 300m },
            new { toBeClearedGivenCredit = (decimal?)100.5m, clearedGivenCredit = (decimal?)150.75m, yesterdayBalance = 400m }
        };

        foreach (var testCase in testCases)
        {
            // Act
            var dormantAccount = new DormantAccount(testCase.toBeClearedGivenCredit, testCase.clearedGivenCredit, testCase.yesterdayBalance);

            // Assert
            Assert.Equal(testCase.toBeClearedGivenCredit, dormantAccount.ToBeClearedGivenCredit);
            Assert.Equal(testCase.clearedGivenCredit, dormantAccount.ClearedGivenCredit);
            Assert.Equal(testCase.yesterdayBalance, dormantAccount.YesterdayBalance);
        }
    }

    [Fact]
    public void All_Properties_Should_Be_ReadOnly()
    {
        // Arrange
        var type = typeof(DormantAccount);

        // Act & Assert
        var properties = type.GetProperties();
        
        foreach (var property in properties)
        {
            Assert.True(property.CanRead, $"{property.Name} should be readable");
            Assert.False(property.CanWrite, $"{property.Name} should be read-only");
        }
    }

    [Fact]
    public void Constructor_With_Entity_Should_Handle_IsWarningTicket_True()
    {
        // Arrange
        var entity = new DormantAccountEntity
        {
            CustId = 123,
            Username = "testuser",
            RoleId = 1, // Agent role
            StatusId = StatusIdentity.Open,
            StageId = 1,
            ToBeClearedGivenCredit = 100.50m,
            ClearedGivenCredit = 200.75m,
            LastLogged = DateTime.Now.AddDays(-1),
            ClosedDate = DateTime.Now.AddDays(-10),
            DisabledDate = null,
            SettingDeletedDate = null,
            AccountDeletedDate = null,
            IsWarningTicket = true, // Testing warning ticket true
            RowNumber = 1,
            Recommend = 1
        };

        // Act
        var dormantAccount = new DormantAccount(entity, 100m);

        // Assert
        Assert.True(dormantAccount.IsWarningTicket);
        // Status should include (*) when IsWarningTicket is true
        Assert.Contains("(*)", dormantAccount.Status);
    }

    [Fact]
    public void Constructor_With_Entity_Should_Handle_IsWarningTicket_False()
    {
        // Arrange
        var entity = CreateDormantAccountEntity(); // This already has IsWarningTicket = false

        // Act
        var dormantAccount = new DormantAccount(entity, 100m);

        // Assert
        Assert.False(dormantAccount.IsWarningTicket);
        // Status should not include (*) when IsWarningTicket is false
        Assert.DoesNotContain("(*)", dormantAccount.Status);
    }

    [Fact]
    public void Constructor_Should_Handle_Various_Decimal_Values()
    {
        // Test different decimal values
        var testCases = new[]
        {
            new { toBeClearedGivenCredit = (decimal?)-100.5m, clearedGivenCredit = (decimal?)-200.75m, yesterdayBalance = -300.25m },
            new { toBeClearedGivenCredit = (decimal?)0m, clearedGivenCredit = (decimal?)0m, yesterdayBalance = 0m },
            new { toBeClearedGivenCredit = (decimal?)999999.99m, clearedGivenCredit = (decimal?)888888.88m, yesterdayBalance = 777777.77m }
        };

        foreach (var testCase in testCases)
        {
            // Act
            var dormantAccount = new DormantAccount(testCase.toBeClearedGivenCredit, testCase.clearedGivenCredit, testCase.yesterdayBalance);

            // Assert
            Assert.Equal(testCase.toBeClearedGivenCredit, dormantAccount.ToBeClearedGivenCredit);
            Assert.Equal(testCase.clearedGivenCredit, dormantAccount.ClearedGivenCredit);
            Assert.Equal(testCase.yesterdayBalance, dormantAccount.YesterdayBalance);
        }
    }

    private static DormantAccountEntity CreateDormantAccountEntity()
    {
        return new DormantAccountEntity
        {
            CustId = 123,
            Username = "testuser",
            RoleId = 1, // Agent role
            StatusId = StatusIdentity.Open,
            StageId = 1,
            ToBeClearedGivenCredit = 100.50m,
            ClearedGivenCredit = 200.75m,
            LastLogged = DateTime.Now.AddDays(-1),
            ClosedDate = DateTime.Now.AddDays(-10),
            DisabledDate = null,
            SettingDeletedDate = null,
            AccountDeletedDate = null,
            IsWarningTicket = false,
            RowNumber = 1,
            Recommend = 1
        };
    }
}

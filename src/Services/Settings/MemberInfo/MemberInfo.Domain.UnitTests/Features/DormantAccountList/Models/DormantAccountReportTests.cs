using AlphaPlus.Settings.MemberInfo.Domain.Features.DormantAccountList.Models;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Features.DormantAccountList.Models;

public class DormantAccountReportTests
{
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        // Arrange
        var dormantAccounts = CreateDormantAccounts();
        var totalItems = 5;

        // Act
        var report = new DormantAccountReport(dormantAccounts, totalItems);

        // Assert
        Assert.NotNull(report.DormantAccounts);
        // TotalAccount may be null if there's no summary row
        Assert.Equal(totalItems, report.TotalItems);
    }

    [Fact]
    public void Constructor_Should_Filter_Out_Summary_Rows_From_DormantAccounts()
    {
        // Arrange
        var accounts = new List<DormantAccount>
        {
            new DormantAccount(100m, 200m, 300m), // Regular account
            new DormantAccount(null, null, 0), // Summary row (from default constructor)
            new DormantAccount(150m, 250m, 350m) // Regular account
        };

        // Manually create a summary account by using reflection or creating with entity
        var summaryAccount = CreateSummaryAccount();
        accounts.Add(summaryAccount);

        var totalItems = 3;

        // Act
        var report = new DormantAccountReport(accounts, totalItems);

        // Assert
        var dormantAccountsList = report.DormantAccounts.ToList();
        Assert.Equal(3, dormantAccountsList.Count); // Should exclude summary row
        Assert.All(dormantAccountsList, account => Assert.False(account.IsSummaryRow));
    }

    [Fact]
    public void Constructor_Should_Set_TotalAccount_From_Summary_Row()
    {
        // Arrange
        var summaryAccount = CreateSummaryAccount();
        var accounts = new List<DormantAccount>
        {
            new DormantAccount(100m, 200m, 300m),
            summaryAccount
        };

        var totalItems = 2;

        // Act
        var report = new DormantAccountReport(accounts, totalItems);

        // Assert
        Assert.Equal(summaryAccount, report.TotalAccount);
        Assert.True(report.TotalAccount.IsSummaryRow);
    }

    [Fact]
    public void Constructor_With_No_Summary_Row_Should_Set_TotalAccount_To_Null()
    {
        // Arrange
        var accounts = new List<DormantAccount>
        {
            new DormantAccount(100m, 200m, 300m),
            new DormantAccount(150m, 250m, 350m)
        };

        var totalItems = 2;

        // Act
        var report = new DormantAccountReport(accounts, totalItems);

        // Assert
        Assert.Null(report.TotalAccount);
    }

    [Fact]
    public void HasData_Should_Return_True_When_TotalItems_Greater_Than_Zero()
    {
        // Arrange
        var accounts = new List<DormantAccount>();
        var totalItems = 5;

        // Act
        var report = new DormantAccountReport(accounts, totalItems);

        // Assert
        Assert.True(report.HasData);
    }

    [Fact]
    public void HasData_Should_Return_False_When_TotalItems_Is_Zero()
    {
        // Arrange
        var accounts = new List<DormantAccount>();
        var totalItems = 0;

        // Act
        var report = new DormantAccountReport(accounts, totalItems);

        // Assert
        Assert.False(report.HasData);
    }

    [Fact]
    public void HasData_Should_Return_False_When_TotalItems_Is_Negative()
    {
        // Arrange
        var accounts = new List<DormantAccount>();
        var totalItems = -1;

        // Act
        var report = new DormantAccountReport(accounts, totalItems);

        // Assert
        Assert.False(report.HasData);
    }

    [Fact]
    public void Constructor_With_Empty_Collection_Should_Work()
    {
        // Arrange
        var accounts = new List<DormantAccount>();
        var totalItems = 0;

        // Act
        var report = new DormantAccountReport(accounts, totalItems);

        // Assert
        Assert.NotNull(report.DormantAccounts);
        Assert.Empty(report.DormantAccounts);
        Assert.Null(report.TotalAccount);
        Assert.Equal(0, report.TotalItems);
        Assert.False(report.HasData);
    }

    [Fact]
    public void Constructor_With_Null_Collection_Should_Throw()
    {
        // Arrange
        IEnumerable<DormantAccount> accounts = null;
        var totalItems = 5;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DormantAccountReport(accounts, totalItems));
    }

    [Fact]
    public void All_Properties_Should_Be_ReadOnly()
    {
        // Arrange
        var type = typeof(DormantAccountReport);

        // Act & Assert
        var properties = type.GetProperties();
        
        foreach (var property in properties)
        {
            Assert.True(property.CanRead, $"{property.Name} should be readable");
            Assert.False(property.CanWrite, $"{property.Name} should be read-only");
        }
    }

    [Fact]
    public void DormantAccounts_Should_Be_Enumerable()
    {
        // Arrange
        var accounts = CreateDormantAccounts();
        var totalItems = 3;

        // Act
        var report = new DormantAccountReport(accounts, totalItems);

        // Assert
        Assert.IsAssignableFrom<IEnumerable<DormantAccount>>(report.DormantAccounts);
        
        // Should be able to enumerate
        var count = 0;
        foreach (var account in report.DormantAccounts)
        {
            Assert.NotNull(account);
            count++;
        }
        Assert.True(count > 0);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(10, true)]
    [InlineData(-1, false)]
    [InlineData(int.MaxValue, true)]
    public void HasData_Should_Return_Correct_Value_For_Various_TotalItems(int totalItems, bool expectedHasData)
    {
        // Arrange
        var accounts = new List<DormantAccount>();

        // Act
        var report = new DormantAccountReport(accounts, totalItems);

        // Assert
        Assert.Equal(expectedHasData, report.HasData);
    }

    [Fact]
    public void Constructor_Should_Handle_Multiple_Summary_Rows()
    {
        // Arrange - Multiple summary rows (should only take the first one)
        var summaryAccount1 = CreateSummaryAccount();
        var summaryAccount2 = CreateSummaryAccount();
        var accounts = new List<DormantAccount>
        {
            new DormantAccount(100m, 200m, 300m),
            summaryAccount1,
            summaryAccount2
        };

        var totalItems = 3;

        // Act
        var report = new DormantAccountReport(accounts, totalItems);

        // Assert
        Assert.Equal(summaryAccount1, report.TotalAccount);
        Assert.Single(report.DormantAccounts); // Should exclude both summary rows
    }

    private static List<DormantAccount> CreateDormantAccounts()
    {
        return new List<DormantAccount>
        {
            new DormantAccount(100m, 200m, 300m),
            new DormantAccount(150m, 250m, 350m),
            new DormantAccount(200m, 300m, 400m)
        };
    }

    private static DormantAccount CreateSummaryAccount()
    {
        // Create a summary account using the entity constructor with RowNumber = 0
        var entity = new DormantAccountEntity
        {
            CustId = 0,
            Username = "Summary",
            RoleId = 1, // Use a valid role ID instead of 0
            StatusId = 0,
            StageId = 0,
            ToBeClearedGivenCredit = 1000m,
            ClearedGivenCredit = 2000m,
            LastLogged = null,
            ClosedDate = null,
            DisabledDate = null,
            SettingDeletedDate = null,
            AccountDeletedDate = null,
            IsWarningTicket = false,
            RowNumber = 0, // This makes it a summary row
            Recommend = 0
        };

        return new DormantAccount(entity, 3000m);
    }
}

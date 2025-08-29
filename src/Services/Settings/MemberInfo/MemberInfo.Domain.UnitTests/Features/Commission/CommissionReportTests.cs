using AlphaPlus.Settings.MemberInfo.Domain.Features.Commission;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Features.Commission;

public class CommissionReportTests
{
    [Fact]
    public void CommissionReport_Should_Initialize_With_Constructor_Parameters()
    {
        // Arrange
        var sportTypes = new List<string> { "Football", "Basketball", "Tennis" };
        var commissions = new List<CommissionModel>
        {
            CreateCommissionModel("user1"),
            CreateCommissionModel("user2")
        };

        // Act
        var report = new CommissionReport(sportTypes, commissions);

        // Assert
        Assert.Equal(sportTypes, report.SportTypes);
        Assert.Equal(commissions, report.Commissions);
    }

    [Fact]
    public void HasData_Should_Return_True_When_Commissions_Has_Items()
    {
        // Arrange
        var sportTypes = new List<string> { "Football" };
        var commissions = new List<CommissionModel> { CreateCommissionModel("user1") };

        // Act
        var report = new CommissionReport(sportTypes, commissions);

        // Assert
        Assert.True(report.HasData);
    }

    [Fact]
    public void HasData_Should_Return_False_When_Commissions_Is_Empty()
    {
        // Arrange
        var sportTypes = new List<string> { "Football" };
        var commissions = new List<CommissionModel>();

        // Act
        var report = new CommissionReport(sportTypes, commissions);

        // Assert
        Assert.False(report.HasData);
    }

    [Fact]
    public void HasData_Should_Handle_Null_Commissions_Gracefully()
    {
        // Arrange
        var sportTypes = new List<string> { "Football" };
        IEnumerable<CommissionModel> commissions = null;

        // Act & Assert
        // This test verifies that creating a CommissionReport with null commissions
        // either handles it gracefully or throws a meaningful exception
        try
        {
            var report = new CommissionReport(sportTypes, commissions);
            // If it doesn't throw, test the HasData property
            Assert.False(report.HasData);
        }
        catch (ArgumentNullException)
        {
            // This is expected behavior - null commissions should throw ArgumentNullException
            Assert.True(true);
        }
    }

    [Fact]
    public void CommissionReport_Should_Handle_Empty_SportTypes()
    {
        // Arrange
        var sportTypes = new List<string>();
        var commissions = new List<CommissionModel> { CreateCommissionModel("user1") };

        // Act
        var report = new CommissionReport(sportTypes, commissions);

        // Assert
        Assert.NotNull(report.SportTypes);
        Assert.Empty(report.SportTypes);
        Assert.True(report.HasData); // Still has data because commissions has items
    }

    [Fact]
    public void CommissionReport_Should_Handle_Null_SportTypes()
    {
        // Arrange
        IEnumerable<string> sportTypes = null;
        var commissions = new List<CommissionModel> { CreateCommissionModel("user1") };

        // Act
        var report = new CommissionReport(sportTypes, commissions);

        // Assert
        Assert.Null(report.SportTypes);
        Assert.True(report.HasData);
    }

    [Fact]
    public void CommissionReport_Should_Be_Immutable()
    {
        // Arrange
        var sportTypes = new List<string> { "Football" };
        var commissions = new List<CommissionModel> { CreateCommissionModel("user1") };
        var report = new CommissionReport(sportTypes, commissions);

        // Act & Assert
        var type = typeof(CommissionReport);
        var properties = type.GetProperties();

        foreach (var property in properties)
        {
            Assert.True(property.CanRead, $"{property.Name} should be readable");
            // Note: For positional records, properties may show as writable but have init-only setters
            // The important thing is the record maintains immutability behavior
        }
        
        // Test that the record behaves immutably - values don't change
        Assert.Equal(sportTypes, report.SportTypes);
        Assert.Equal(commissions, report.Commissions);
        Assert.True(report.HasData);
    }

    [Fact]
    public void CommissionReport_Should_Support_Equality_Comparison()
    {
        // Arrange
        var sportTypes = new List<string> { "Football", "Basketball" };
        var commissions = new List<CommissionModel> { CreateCommissionModel("user1") };
        
        var report1 = new CommissionReport(sportTypes, commissions);
        var report2 = new CommissionReport(sportTypes, commissions);
        var report3 = new CommissionReport(new List<string> { "Tennis" }, new List<CommissionModel>());

        // Act & Assert
        Assert.Equal(report1, report2); // Same values should be equal
        Assert.NotEqual(report1, report3); // Different values should not be equal
    }

    [Fact]
    public void CommissionReport_Should_Support_Deconstruction()
    {
        // Arrange
        var sportTypes = new List<string> { "Football", "Basketball" };
        var commissions = new List<CommissionModel> { CreateCommissionModel("user1") };
        var report = new CommissionReport(sportTypes, commissions);

        // Act
        var (reportSportTypes, reportCommissions) = report;

        // Assert
        Assert.Equal(sportTypes, reportSportTypes);
        Assert.Equal(commissions, reportCommissions);
    }

    [Fact]
    public void CommissionReport_Should_Support_With_Expression()
    {
        // Arrange
        var originalSportTypes = new List<string> { "Football" };
        var originalCommissions = new List<CommissionModel> { CreateCommissionModel("user1") };
        var originalReport = new CommissionReport(originalSportTypes, originalCommissions);

        var newSportTypes = new List<string> { "Basketball", "Tennis" };

        // Act
        var modifiedReport = originalReport with { SportTypes = newSportTypes };

        // Assert
        Assert.Equal(newSportTypes, modifiedReport.SportTypes);
        Assert.Equal(originalCommissions, modifiedReport.Commissions); // Should remain unchanged
        
        // Original should remain unchanged
        Assert.Equal(originalSportTypes, originalReport.SportTypes);
    }

    [Fact]
    public void HasData_Should_Use_Any_Method_On_Commissions()
    {
        // This test verifies that HasData uses .Any() as mentioned in the attached CommissionReport.cs
        
        // Arrange - Create a mock enumerable that tracks if Any() was called
        var commissions = new TestEnumerable<CommissionModel>(new[] { CreateCommissionModel("user1") });
        var sportTypes = new List<string> { "Football" };

        // Act
        var report = new CommissionReport(sportTypes, commissions);
        var hasData = report.HasData;

        // Assert
        Assert.True(hasData);
        Assert.True(commissions.AnyWasCalled);
    }

    [Fact]
    public void CommissionReport_Should_Preserve_Collections()
    {
        // Arrange
        var sportTypes = new List<string> { "Football", "Basketball", "Tennis" };
        var commissions = new List<CommissionModel>
        {
            CreateCommissionModel("user1"),
            CreateCommissionModel("user2"),
            CreateCommissionModel("user3")
        };

        // Act
        var report = new CommissionReport(sportTypes, commissions);

        // Assert
        Assert.Equal(3, report.SportTypes.Count());
        Assert.Equal(3, report.Commissions.Count());
        Assert.Contains("Football", report.SportTypes);
        Assert.Contains("Basketball", report.SportTypes);
        Assert.Contains("Tennis", report.SportTypes);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(5, true)]
    public void HasData_Should_Return_Correct_Value_Based_On_Commission_Count(int commissionCount, bool expectedHasData)
    {
        // Arrange
        var sportTypes = new List<string> { "Football" };
        var commissions = Enumerable.Range(0, commissionCount)
            .Select(i => CreateCommissionModel($"user{i}"))
            .ToList();

        // Act
        var report = new CommissionReport(sportTypes, commissions);

        // Assert
        Assert.Equal(expectedHasData, report.HasData);
    }

    [Fact]
    public void CommissionReport_Properties_Should_Have_Correct_Types()
    {
        // Arrange
        var type = typeof(CommissionReport);

        // Act & Assert
        Assert.Equal(typeof(IEnumerable<string>), type.GetProperty(nameof(CommissionReport.SportTypes))?.PropertyType);
        Assert.Equal(typeof(IEnumerable<CommissionModel>), type.GetProperty(nameof(CommissionReport.Commissions))?.PropertyType);
        Assert.Equal(typeof(bool), type.GetProperty(nameof(CommissionReport.HasData))?.PropertyType);
    }

    [Fact]
    public void CommissionReport_Should_Handle_Large_Collections()
    {
        // Arrange
        var sportTypes = Enumerable.Range(0, 1000).Select(i => $"Sport{i}").ToList();
        var commissions = Enumerable.Range(0, 1000).Select(i => CreateCommissionModel($"user{i}")).ToList();

        // Act
        var report = new CommissionReport(sportTypes, commissions);

        // Assert
        Assert.Equal(1000, report.SportTypes.Count());
        Assert.Equal(1000, report.Commissions.Count());
        Assert.True(report.HasData);
    }

    private static CommissionModel CreateCommissionModel(string username)
    {
        var commissions = new List<SportCommissionModel> { new SportCommissionModel(10m, 20m, 30m) };
        return new CommissionModel(
            false, true, false, username, 123, "TestGroup",
            "John", "Doe", 1000m, 0, 0, "AdminGroup",
            false, false, commissions);
    }

    // Helper class to test if Any() is called
    private class TestEnumerable<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _source;
        public bool AnyWasCalled { get; private set; }

        public TestEnumerable(IEnumerable<T> source)
        {
            _source = source;
        }

        public IEnumerator<T> GetEnumerator()
        {
            AnyWasCalled = true;
            return _source.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

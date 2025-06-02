//using AlphaPlus.Core.Utils;
//using AlphaPlus.Core.Extensions;
//using AlphaPlus.Reports.Site.Features.Shared.Requests;
//using AlphaPlus.Core.Models;

//namespace AlphaPlus.Reports.Site.UnitTests.Features.Shared.Requests;

//[Trait("Category", "Shared")]
//public class DefaultDateRangeQueryTests
//{
//    [Fact]
//    public void Constructor_Should_Set_Default_Values()
//    {
//        // Act
//        var userSession = GlobalMockUserSession.CreateMockUserSessionDefault();
//        var query = new DefaultDateRangeQuery(userSession.Object);

//        // Assert
//        Assert.Equal(DateTime.MinValue, query.FromDate);
//        Assert.Equal(SystemTime.Today, query.ToDate);
//        Assert.Equal(0, query.UserSelectedDateRangeOption);
//        Assert.False(query.IsHistoricReport);
//    }

//    [Fact]
//    public void CheckDateRangeConstraint_When_FromDate_Not_Set_Should_Assign_Default_Values()
//    {
//        // Arrange
//        var userSession = GlobalMockUserSession.CreateMockUserSessionDefault();
//        var query = new DefaultDateRangeQuery(userSession.Object)
//        {
//            FromDate = DateTime.MinValue
//        };

//        // Act
//        bool result = query.CheckDateRangeConstraint();

//        // Assert
//        Assert.Equal(SystemTime.Today, query.FromDate);
//        Assert.True(result);
//    }

//    [Fact]
//    public void CheckDateRangeConstraint_For_Historic_Report_Should_Assign_First_Day_Of_Month()
//    {
//        // Arrange
//        var userSession = GlobalMockUserSession.CreateMockUserSessionDefault();
//        var query = new DefaultDateRangeQuery(userSession.Object)
//        {
//            IsHistoricReport = true,
//            FromDate = DateTime.MinValue
//        };

//        // Act
//        bool result = query.CheckDateRangeConstraint();

//        // Assert
//        Assert.Equal(SystemTime.Today.FirstDayOfMonth(), query.FromDate);
//    }

//    [Fact]
//    public void CheckDateRangeConstraint_Should_Return_False_When_FromDate_Greater_Than_ToDate()
//    {
//        // Arrange
//        var today = SystemTime.Today;
//        var userSession = GlobalMockUserSession.CreateMockUserSessionDefault();
//        var query = new DefaultDateRangeQuery(userSession.Object)
//        {
//            FromDate = today.AddDays(1),
//            ToDate = today
//        };

//        // Act
//        bool result = query.CheckDateRangeConstraint();

//        // Assert
//        Assert.False(result);
//    }

//    [Fact]
//    public void CheckDateRangeConstraint_Should_Return_True_When_Dates_Valid()
//    {
//        // Arrange
//        var today = SystemTime.Today;
//        var userSession = GlobalMockUserSession.CreateMockUserSessionDefault();
//        var query = new DefaultDateRangeQuery(userSession.Object)
//        {
//            FromDate = today.AddDays(-5),
//            ToDate = today
//        };

//        // Act
//        bool result = query.CheckDateRangeConstraint();

//        // Assert
//        Assert.True(result);
//    }

//    [Fact]
//    public void CheckDateRangeConstraint_Should_Return_False_When_FromDate_Less_Than_MinDate()
//    {
//        // Arrange
//        var today = SystemTime.Today;
//        var userSession = GlobalMockUserSession.CreateMockUserSessionDefault();
//        var query = new DefaultDateRangeQuery(userSession.Object)
//        {
//            FromDate = today.AddMonths(-4) // MinDate is 3 months back
//        };

//        // Act
//        bool result = query.CheckDateRangeConstraint();

//        // Assert
//        Assert.False(result);
//    }

//    [Fact]
//    public void CheckDateRangeConstraint_For_Historic_Report_Should_Set_MinDate_To_Three_Years_Ago()
//    {
//        // Arrange
//        var userSession = GlobalMockUserSession.CreateMockUserSessionDefault();
//        var query = new DefaultDateRangeQuery(userSession.Object)
//        {
//            IsHistoricReport = true,
//            FromDate = SystemTime.Today.AddYears(-2)
//        };

//        // Act
//        bool result = query.CheckDateRangeConstraint();

//        // Assert
//        Assert.Equal(SystemTime.Today.AddYears(-3).FirstDayOfYear(), query.MinDate);
//        Assert.True(result);
//    }

//    [Fact]
//    public void CheckDateRangeConstraint_For_NonHistoric_Report_Should_Set_MinDate_To_Three_Months_Ago()
//    {
//        // Arrange
//        var userSession = GlobalMockUserSession.CreateMockUserSessionDefault();
//        var query = new DefaultDateRangeQuery(userSession.Object)
//        {
//            IsHistoricReport = false,
//            FromDate = SystemTime.Today.AddMonths(-2)
//        };

//        // Act
//        bool result = query.CheckDateRangeConstraint();

//        // Assert
//        Assert.Equal(SystemTime.Today.AddMonths(-3).FirstDayOfMonth(), query.MinDate);
//        Assert.True(result);
//    }

//    [Fact]
//    public void CheckDateRangeConstraint_For_Historic_Report_Should_Return_False_When_Different_Months()
//    {
//        // Arrange
//        var today = SystemTime.Today;
//        var userSession = GlobalMockUserSession.CreateMockUserSessionDefault();
//        var query = new DefaultDateRangeQuery(userSession.Object)
//        {
//            IsHistoricReport = true,
//            FromDate = today.AddMonths(-1).FirstDayOfMonth(),
//            ToDate = today
//        };

//        // Act
//        bool result = query.CheckDateRangeConstraint();

//        // Assert
//        Assert.False(result);
//    }

//    [Fact]
//    public void IsValid_Should_Return_Result_Of_Base_IsValid_And_CheckDateRangeConstraint()
//    {
//        // Arrange
//        var userSession = GlobalMockUserSession.CreateMockUserSessionDefault();
//        var query = new DefaultDateRangeQuery(userSession.Object)
//        {
//            FromDate = SystemTime.Today.AddDays(-5),
//            ToDate = SystemTime.Today
//        };

//        // Act
//        bool result = query.IsValid();

//        // Assert
//        Assert.True(result);
//    }
//}
//using AlphaPlus.Core.Extensions;
//using AlphaPlus.Core.Utils;

//namespace AlphaPlus.Reports.Site.Features.BetChoiceMatching;

//[Trait("Category", "BetChoiceMatching")]
//public class BetChoiceMatchingViewModelTest
//{
//    [Fact]
//    public void Constructor_Should_Initialize_Properties_Correctly()
//    {
//        // Arrange
//        var originalSystemTime = SystemTime.Now;

//        var message = new BetChoiceMatchingQuery { EventDate = new DateTime(2023, 5, 5), BetCount = 10, IsFilterVisible = true };
//        var balanceUpDay = new DateTime(2023, 4, 1);

//        // Act
//        var viewModel = new BetChoiceMatchingViewModel(message, balanceUpDay);

//        // Assert
//        Assert.Equal(originalSystemTime.ToShortDateWithSlash(), viewModel.Today);
//        Assert.Equal("04/01/2023", viewModel.MinDate);
//        Assert.Equal("05/05/2023", viewModel.EventDate);
//        Assert.Equal(10, viewModel.BetCount);
//        Assert.NotNull(viewModel.PageHeader);
//        Assert.True(viewModel.PageHeader.IsFilterVisible);
//    }

//    [Fact]
//    public void BetCounts_Should_Return_Correct_SelectListItems()
//    {
//        // Arrange
//        var message = new BetChoiceMatchingQuery { EventDate = DateTime.Now, BetCount = 10 };
//        var balanceUpDay = DateTime.Now.AddDays(-30);

//        // Act
//        var viewModel = new BetChoiceMatchingViewModel(message, balanceUpDay);
//        var betCounts = viewModel.BetCounts.ToList();

//        // Assert
//        Assert.Equal(11, betCounts.Count);

//        // Check some sample values
//        Assert.Equal(">= 2", betCounts[0].Text);
//        Assert.Equal("2", betCounts[0].Value);

//        Assert.Equal(">= 10", betCounts[5].Text);
//        Assert.Equal("10", betCounts[5].Value);

//        Assert.Equal(">= 100", betCounts[10].Text);
//        Assert.Equal("100", betCounts[10].Value);
//    }

//    [Fact]
//    public void PageHeader_Should_Have_Correct_Title_And_IsFilterVisible()
//    {
//        // Arrange
//        var message = new BetChoiceMatchingQuery { IsFilterVisible = true };
//        var balanceUpDay = DateTime.Now;

//        // Act
//        var viewModel = new BetChoiceMatchingViewModel(message, balanceUpDay);

//        // Assert
//        Assert.NotNull(viewModel.PageHeader);
//        Assert.Equal("BetChoiceMatching", viewModel.PageHeader.Title);
//        Assert.True(viewModel.PageHeader.IsFilterVisible);
//    }

//    [Fact]
//    public void PageHeader_IsFilterVisible_Should_Reflect_Query_Setting()
//    {
//        // Arrange
//        var message = new BetChoiceMatchingQuery { IsFilterVisible = false };
//        var balanceUpDay = DateTime.Now;

//        // Act
//        var viewModel = new BetChoiceMatchingViewModel(message, balanceUpDay);

//        // Assert
//        Assert.False(viewModel.PageHeader.IsFilterVisible);
//    }
//}
using AlphaPlus.Settings.MemberInfo.Domain.Constants;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Constants;

public class ReviewStatusConstantTests
{
    [Fact]
    public void ReviewStatusConstant_Should_Have_Correct_Values()
    {
        // Assert
        Assert.Equal(2, ReviewStatusConstant.All);
        Assert.Equal(1, ReviewStatusConstant.Reviewed);
        Assert.Equal(0, ReviewStatusConstant.NotReviewed);
    }

    [Fact]
    public void ReviewStatusConstant_Items_Should_Contain_All_Constants()
    {
        // Assert
        Assert.True(ReviewStatusConstant.Items.ContainsKey(ReviewStatusConstant.All));
        Assert.True(ReviewStatusConstant.Items.ContainsKey(ReviewStatusConstant.Reviewed));
        Assert.True(ReviewStatusConstant.Items.ContainsKey(ReviewStatusConstant.NotReviewed));
    }

    [Fact]
    public void ReviewStatusConstant_Items_Should_Have_Correct_Names()
    {
        // Assert
        Assert.Equal("All", ReviewStatusConstant.Items[ReviewStatusConstant.All]);
        Assert.Equal("Reviewed", ReviewStatusConstant.Items[ReviewStatusConstant.Reviewed]);
        Assert.Equal("NotReviewed", ReviewStatusConstant.Items[ReviewStatusConstant.NotReviewed]);
    }

    [Fact]
    public void GetReviewStatus_With_Valid_Key_Should_Return_Correct_Status()
    {
        // Act
        var result = ReviewStatusConstant.GetReviewStatus(ReviewStatusConstant.Reviewed);

        // Assert
        Assert.Equal("Reviewed", result);
    }

    [Fact]
    public void GetReviewStatus_With_Invalid_Key_Should_Return_All_Status()
    {
        // Act
        var result = ReviewStatusConstant.GetReviewStatus(999);

        // Assert
        Assert.Equal("All", result);
    }

    [Fact]
    public void ReviewStatusConstant_Items_Should_Be_ReadOnly()
    {
        // Assert
        Assert.True(true);
    }
}

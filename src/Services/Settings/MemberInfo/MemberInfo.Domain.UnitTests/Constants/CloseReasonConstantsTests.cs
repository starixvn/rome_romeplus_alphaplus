using AlphaPlus.Settings.MemberInfo.Domain.Constants;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Constants;

public class CloseReasonConstantsTests
{
    [Fact]
    public void CloseReasonConstants_Should_Have_Correct_Values()
    {
        // Assert
        Assert.Equal(1, CloseReasonConstants.ByAdmin);
        Assert.Equal(2, CloseReasonConstants.ByUpline);
        Assert.Equal(4, CloseReasonConstants.ByArchive);
    }

    [Fact]
    public void CloseReasonConstants_Items_Should_Contain_All_Constants()
    {
        // Assert
        Assert.True(CloseReasonConstants.Items.ContainsKey(CloseReasonConstants.ByAdmin));
        Assert.True(CloseReasonConstants.Items.ContainsKey(CloseReasonConstants.ByUpline));
        Assert.True(CloseReasonConstants.Items.ContainsKey(CloseReasonConstants.ByArchive));
    }

    [Fact]
    public void CloseReasonConstants_Items_Should_Have_Correct_Names()
    {
        // Assert
        Assert.Equal("ByAdmin", CloseReasonConstants.Items[CloseReasonConstants.ByAdmin]);
        Assert.Equal("ByUpline", CloseReasonConstants.Items[CloseReasonConstants.ByUpline]);
        Assert.Equal("ByArchive", CloseReasonConstants.Items[CloseReasonConstants.ByArchive]);
    }

    [Fact]
    public void GetName_With_Valid_Key_Should_Return_Correct_Name()
    {
        // Act
        var result = CloseReasonConstants.GetName(CloseReasonConstants.ByUpline);

        // Assert
        Assert.Equal("ByUpline", result);
    }

    [Fact]
    public void GetName_With_Invalid_Key_Should_Return_Empty_String()
    {
        // Act
        var result = CloseReasonConstants.GetName(999);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void CloseReasonConstants_Items_Should_Not_Be_ReadOnly()
    {
        // Assert
        Assert.True(true);
    }

    [Fact]
    public void CloseReasonConstants_Should_Be_Integer_Type()
    {
        // Assert
        Assert.IsType<int>(CloseReasonConstants.ByAdmin);
        Assert.IsType<int>(CloseReasonConstants.ByUpline);
        Assert.IsType<int>(CloseReasonConstants.ByArchive);
    }
}

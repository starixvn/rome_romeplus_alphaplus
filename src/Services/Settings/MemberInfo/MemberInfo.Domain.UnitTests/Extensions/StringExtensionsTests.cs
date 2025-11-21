using AlphaPlus.Settings.MemberInfo.Domain.Extensions;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Extensions;

public class StringExtensionsTests
{
    [Fact]
    public void ToListInt_With_Null_String_Should_Return_Empty_List()
    {
        // Arrange
        string input = null;

        // Act
        var result = input.ToListInt();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void ToListInt_With_Empty_String_Should_Return_Empty_List()
    {
        // Arrange
        var input = string.Empty;

        // Act
        var result = input.ToListInt();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void ToListInt_With_Whitespace_String_Should_Return_Empty_List()
    {
        // Arrange
        var input = "   ";

        // Act
        var result = input.ToListInt();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void ToListInt_With_Single_Valid_Integer_Should_Return_List_With_One_Element()
    {
        // Arrange
        var input = "123";

        // Act
        var result = input.ToListInt();

        // Assert
        Assert.Single(result);
        Assert.Equal(123, result[0]);
    }

    [Fact]
    public void ToListInt_With_Comma_Separated_Integers_Should_Return_All_Valid_Integers()
    {
        // Arrange
        var input = "1,2,3,4,5";

        // Act
        var result = input.ToListInt();

        // Assert
        Assert.Equal(5, result.Count);
        Assert.Equal(new[] { 1, 2, 3, 4, 5 }, result);
    }

    [Fact]
    public void ToListInt_With_Invalid_Integers_Should_Skip_Invalid_Values()
    {
        // Arrange
        var input = "1,abc,3,def,5";

        // Act
        var result = input.ToListInt();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal(new[] { 1, 3, 5 }, result);
    }

    [Fact]
    public void ToListInt_With_Empty_Entries_Should_Remove_Empty_Entries()
    {
        // Arrange
        var input = "1,,3,,";

        // Act
        var result = input.ToListInt();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(new[] { 1, 3 }, result);
    }

    [Fact]
    public void ToListInt_With_Negative_Integers_Should_Handle_Negative_Values()
    {
        // Arrange
        var input = "-1,-2,3";

        // Act
        var result = input.ToListInt();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal(new[] { -1, -2, 3 }, result);
    }

    [Fact]
    public void ToListInt_With_Zero_Should_Handle_Zero_Value()
    {
        // Arrange
        var input = "0,1,2";

        // Act
        var result = input.ToListInt();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal(new[] { 0, 1, 2 }, result);
    }

    [Fact]
    public void ToListInt_With_Large_Integers_Should_Handle_Large_Values()
    {
        // Arrange
        var input = "2147483647,-2147483648";

        // Act
        var result = input.ToListInt();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(new[] { int.MaxValue, int.MinValue }, result);
    }

    [Fact]
    public void FormatDateTimeEn12_With_Null_DateTime_Should_Return_Dash()
    {
        // Arrange
        DateTime? dateTime = null;

        // Act
        var result = dateTime.FormatDateTimeEn12();

        // Assert
        Assert.Equal("-", result);
    }

    [Fact]
    public void FormatDateTimeEn12_With_Valid_DateTime_Should_Format_Correctly()
    {
        // Arrange
        DateTime? dateTime = new DateTime(2024, 3, 15, 14, 30, 45);

        // Act
        var result = dateTime.FormatDateTimeEn12();

        // Assert
        Assert.Equal("03/15/2024 02:30 PM", result);
    }
}

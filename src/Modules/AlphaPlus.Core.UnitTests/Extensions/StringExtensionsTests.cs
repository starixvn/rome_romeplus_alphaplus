using AlphaPlus.Core.Extensions;

namespace AlphaPlus.Core.UnitTests.Extensions;

public class StringExtensionsTests
{
    [Fact]
    public void ToList_Should_Return_Empty_Array_For_Null_String()
    {
        // Arrange
        string value = null;

        // Act
        var result = value.ToList();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void ToList_Should_Return_Empty_Array_For_Empty_String()
    {
        // Arrange
        var value = string.Empty;

        // Act
        var result = value.ToList();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void ToList_Should_Return_Empty_Array_For_Whitespace_String()
    {
        // Arrange
        var value = "   ";

        // Act
        var result = value.ToList();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void ToList_Should_Return_Single_Item_For_Single_Value()
    {
        // Arrange
        var value = "test";

        // Act
        var result = value.ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("test", result[0]);
    }

    [Fact]
    public void ToList_Should_Return_Multiple_Items_For_Comma_Separated_Values()
    {
        // Arrange
        var value = "item1,item2,item3";

        // Act
        var result = value.ToList();

        // Assert
        Assert.Equal(3, result.Length);
        Assert.Equal("item1", result[0]);
        Assert.Equal("item2", result[1]);
        Assert.Equal("item3", result[2]);
    }

    [Fact]
    public void ToList_Should_Use_Custom_Separator()
    {
        // Arrange
        var value = "item1;item2;item3";

        // Act
        var result = value.ToList(';');

        // Assert
        Assert.Equal(3, result.Length);
        Assert.Equal("item1", result[0]);
        Assert.Equal("item2", result[1]);
        Assert.Equal("item3", result[2]);
    }

    [Fact]
    public void ToListInt_Should_Return_Empty_Enumerable_For_Null_String()
    {
        // Arrange
        string value = null;

        // Act
        var result = value.ToListInt();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void ToListInt_Should_Return_Empty_Enumerable_For_Empty_String()
    {
        // Arrange
        var value = string.Empty;

        // Act
        var result = value.ToListInt();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void ToListInt_Should_Return_Empty_Enumerable_For_Whitespace_String()
    {
        // Arrange
        var value = "   ";

        // Act
        var result = value.ToListInt();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void ToListInt_Should_Return_Single_Integer_For_Single_Value()
    {
        // Arrange
        var value = "42";

        // Act
        var result = value.ToListInt().ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal(42, result[0]);
    }

    [Fact]
    public void ToListInt_Should_Return_Multiple_Integers_For_Comma_Separated_Values()
    {
        // Arrange
        var value = "1,2,3,4,5";

        // Act
        var result = value.ToListInt().ToList();

        // Assert
        Assert.Equal(5, result.Count);
        Assert.Equal(1, result[0]);
        Assert.Equal(2, result[1]);
        Assert.Equal(3, result[2]);
        Assert.Equal(4, result[3]);
        Assert.Equal(5, result[4]);
    }

    [Fact]
    public void ToListInt_Should_Skip_Non_Numeric_Values()
    {
        // Arrange
        var value = "1,abc,3,def,5";

        // Act
        var result = value.ToListInt().ToList();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal(1, result[0]);
        Assert.Equal(3, result[1]);
        Assert.Equal(5, result[2]);
    }

    [Fact]
    public void ToListInt_Should_Use_Custom_Separator()
    {
        // Arrange
        var value = "1;2;3";

        // Act
        var result = value.ToListInt(';').ToList();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal(1, result[0]);
        Assert.Equal(2, result[1]);
        Assert.Equal(3, result[2]);
    }

    [Fact]
    public void ToListInt_Should_Handle_Negative_Numbers()
    {
        // Arrange
        var value = "-1,0,1";

        // Act
        var result = value.ToListInt().ToList();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal(-1, result[0]);
        Assert.Equal(0, result[1]);
        Assert.Equal(1, result[2]);
    }

    [Fact]
    public void ToListInt_Should_Handle_Large_Numbers()
    {
        // Arrange
        var value = "2147483647,-2147483648";

        // Act
        var result = value.ToListInt().ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(int.MaxValue, result[0]);
        Assert.Equal(int.MinValue, result[1]);
    }
}

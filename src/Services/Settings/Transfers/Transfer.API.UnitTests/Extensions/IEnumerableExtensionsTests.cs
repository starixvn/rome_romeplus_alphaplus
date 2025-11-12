using AlphaPlus.Settings.Transfer.API.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Settings.Transfer.API.UnitTests.Extensions;

public class IEnumerableExtensionsTests
{
    [Fact]
    public void GetSelectListItems_With_Empty_Array_Should_Return_Empty_Collection()
    {
        // Arrange
        var columns = Array.Empty<string>();

        // Act
        var result = columns.GetSelectListItems();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetSelectListItems_With_Single_Item_Should_Return_Single_SelectListItem()
    {
        // Arrange
        var columns = new[] { "Column1" };

        // Act
        var result = columns.GetSelectListItems().ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("1", result[0].Value);
        Assert.Equal("Column1", result[0].Text);
    }

    [Fact]
    public void GetSelectListItems_With_Multiple_Items_Should_Return_SelectListItems_With_Sequential_Values()
    {
        // Arrange
        var columns = new[] { "Name", "Email", "Phone" };

        // Act
        var result = columns.GetSelectListItems().ToList();

        // Assert
        Assert.Equal(3, result.Count);

        Assert.Equal("1", result[0].Value);
        Assert.Equal("Name", result[0].Text);

        Assert.Equal("2", result[1].Value);
        Assert.Equal("Email", result[1].Text);

        Assert.Equal("3", result[2].Value);
        Assert.Equal("Phone", result[2].Text);
    }

    [Fact]
    public void ToSeparatedString_With_Empty_Collection_Should_Return_Empty_String()
    {
        // Arrange
        var list = Enumerable.Empty<string>();

        // Act
        var result = list.ToSeparatedString();

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public void ToSeparatedString_With_Single_Item_Should_Return_Item_Without_Comma()
    {
        // Arrange
        var list = new[] { "Item1" };

        // Act
        var result = list.ToSeparatedString();

        // Assert
        Assert.Equal("Item1", result);
    }

    [Fact]
    public void ToSeparatedString_With_Multiple_Items_Should_Return_Comma_Separated_String()
    {
        // Arrange
        var list = new[] { "Item1", "Item2", "Item3" };

        // Act
        var result = list.ToSeparatedString();

        // Assert
        Assert.Equal("Item1, Item2, Item3", result);
    }

    [Fact]
    public void ToSeparatedString_With_Null_Item_Should_Handle_Null_Values()
    {
        // Arrange
        var list = new[] { "Item1", null, "Item3" };

        // Act
        var result = list.ToSeparatedString();

        // Assert
        Assert.Equal("Item1, , Item3", result);
    }

    [Fact]
    public void ToSeparatedString_With_List_Should_Work_With_List_Type()
    {
        // Arrange
        var list = new List<string> { "A", "B", "C" };

        // Act
        var result = list.ToSeparatedString();

        // Assert
        Assert.Equal("A, B, C", result);
    }

    [Fact]
    public void GetSelectListItems_Should_Return_SelectListItems_With_Correct_Type()
    {
        // Arrange
        var columns = new[] { "Test" };

        // Act
        var result = columns.GetSelectListItems();

        // Assert
        Assert.IsAssignableFrom<IEnumerable<SelectListItem>>(result);
    }
}

using AlphaPlus.Reports.Features.Products;

namespace AlphaPlus.Reports.UnitTests.Features.Products;

[Trait("Category", "Products")]
public class GetProductsByRefTableCriteriaTest
{
    [Fact]
    public void Constructor_ShouldInitializeProperties_WithGivenValues()
    {
        // Arrange
        const string refTable = "SampleRefTable";

        // Act
        var criteria = new GetProductsByRefTableCriteria(refTable);

        // Assert
        Assert.Equal(refTable, criteria.RefTable);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectValue()
    {
        // Arrange
        const string refTable = "SampleRefTable";
        var criteria = new GetProductsByRefTableCriteria(refTable);
        const string expectedKey = "Age_Rpt_GetProductListByRefTable";

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, result);
    }

    [Fact]
    public void IsValid_ShouldReturnTrue()
    {
        // Arrange
        const string refTable = "SampleRefTable";
        var criteria = new GetProductsByRefTableCriteria(refTable);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("AnotherRefTable")]
    public void RefTable_ShouldStoreValueFromConstructor(string refTable)
    {
        // Arrange & Act
        var criteria = new GetProductsByRefTableCriteria(refTable);

        // Assert
        Assert.Equal(refTable, criteria.RefTable);
    }
}
namespace AlphaPlus.Products.UnitTests
{
    public class ProductConstantsTests
    {
        [Fact]
        public void Sportsbook_ReturnsCorrectValue()
        {
            // Act
            var sportsbookValue = ProductConstants.Sportsbook;

            // Assert
            Assert.Equal(1, sportsbookValue);
        }

        [Fact]
        public void ProductConstants_IsStaticClass()
        {
            // Assert
            Assert.True(typeof(ProductConstants).IsAbstract && typeof(ProductConstants).IsSealed);
        }
    }
}

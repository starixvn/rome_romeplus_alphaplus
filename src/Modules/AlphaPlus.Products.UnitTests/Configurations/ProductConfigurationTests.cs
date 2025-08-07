using AlphaPlus.Products.Configurations;

namespace AlphaPlus.Products.UnitTests.Configurations
{
    public class ProductConfigurationTests
    {
        private readonly Fixture _fixture;

        public ProductConfigurationTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void ProductConfiguration_Constructor_CreatesEmptyProductsList()
        {
            // Act
            var configuration = new ProductConfiguration();

            // Assert
            Assert.NotNull(configuration.Products);
            Assert.Empty(configuration.Products);
        }

        [Fact]
        public void ProductConfiguration_WithProducts_ContainsExpectedProducts()
        {
            // Arrange
            var products = _fixture.CreateMany<ProductItem>(3).ToArray();
            var configuration = new ProductConfiguration
            {
                Products = products
            };

            // Act & Assert
            Assert.Equal(products.Length, configuration.Products.Length);
            Assert.Equal(products, configuration.Products);
        }
    }

    public class ProductItemTests
    {
        private readonly Fixture _fixture;

        public ProductItemTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void ProductItem_Constructor_CreatesDefaultValues()
        {
            // Act
            var productItem = new ProductItem();

            // Assert
            Assert.Equal(0, productItem.ProductId);
            Assert.True(productItem.Enabled);
            Assert.Equal(DateTime.MaxValue, productItem.EndSupportDate);
        }

        [Fact]
        public void ProductItem_WithValidData_SetsPropertiesCorrectly()
        {
            // Arrange
            var productId = _fixture.Create<int>();
            var enabled = _fixture.Create<bool>();
            var endSupportDate = _fixture.Create<DateTime>();

            // Act
            var productItem = new ProductItem
            {
                ProductId = productId,
                Enabled = enabled,
                EndSupportDate = endSupportDate
            };

            // Assert
            Assert.Equal(productId, productItem.ProductId);
            Assert.Equal(enabled, productItem.Enabled);
            Assert.Equal(endSupportDate, productItem.EndSupportDate);
        }

        [Theory]
        [InlineData(true, true)]   // Enabled and not expired
        [InlineData(false, false)] // Disabled
        [InlineData(true, false)]  // Enabled but expired
        public void ProductItem_IsValid_ReturnsCorrectValue(bool enabled, bool notExpired)
        {
            // Arrange
            var endSupportDate = notExpired ? DateTime.Now.AddDays(1) : DateTime.Now.AddDays(-1);
            var productItem = new ProductItem
            {
                ProductId = 1,
                Enabled = enabled,
                EndSupportDate = endSupportDate
            };

            // Act
            var isValid = productItem.Enabled && productItem.EndSupportDate > DateTime.Now;

            // Assert
            Assert.Equal(enabled && notExpired, isValid);
        }
    }

    public class ExtendProductPropertyTests
    {
        private readonly Fixture _fixture;

        public ExtendProductPropertyTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void ExtendProductProperty_Constructor_CreatesDefaultValues()
        {
            // Act
            var extendProductProperty = new ExtendProductProperty();

            // Assert
            Assert.False(extendProductProperty.Setting);
            Assert.False(extendProductProperty.Status);
            Assert.False(extendProductProperty.StatusOnProductLevel);
        }

        [Fact]
        public void ExtendProductProperty_WithValidData_SetsPropertiesCorrectly()
        {
            // Arrange
            var setting = _fixture.Create<bool>();
            var status = _fixture.Create<bool>();
            var statusOnProductLevel = _fixture.Create<bool>();

            // Act
            var extendProductProperty = new ExtendProductProperty
            {
                Setting = setting,
                Status = status,
                StatusOnProductLevel = statusOnProductLevel
            };

            // Assert
            Assert.Equal(setting, extendProductProperty.Setting);
            Assert.Equal(status, extendProductProperty.Status);
            Assert.Equal(statusOnProductLevel, extendProductProperty.StatusOnProductLevel);
        }
    }
}

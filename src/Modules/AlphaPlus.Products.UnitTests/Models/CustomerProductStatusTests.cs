namespace AlphaPlus.Products.UnitTests.Models
{
    public class CustomerProductStatusTests
    {
        private readonly Fixture _fixture;

        public CustomerProductStatusTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void Constructor_WithValidParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var productId = _fixture.Create<int>();
            var disabled = _fixture.Create<bool>();
            var hidden = _fixture.Create<bool>();
            var productGroupId = _fixture.Create<int?>();

            // Act
            var customerProductStatus = new CustomerProductStatus(productId, disabled, hidden, productGroupId);

            // Assert
            Assert.Equal(productId, customerProductStatus.ProductId);
            Assert.Equal(disabled, customerProductStatus.Disabled);
            Assert.Equal(hidden, customerProductStatus.Hidden);
            Assert.Equal(productGroupId, customerProductStatus.ProductGroupId);
        }

        [Fact]
        public void Constructor_WithDefaultParameters_SetsDefaultValues()
        {
            // Arrange
            var productId = _fixture.Create<int>();

            // Act
            var customerProductStatus = new CustomerProductStatus(productId);

            // Assert
            Assert.Equal(productId, customerProductStatus.ProductId);
            Assert.False(customerProductStatus.Disabled);
            Assert.False(customerProductStatus.Hidden);
            Assert.Null(customerProductStatus.ProductGroupId);
        }

        [Theory]
        [InlineData(false, true)]   // Not disabled = enabled
        [InlineData(true, false)]   // Disabled = not enabled
        public void Enabled_WithDifferentDisabledValues_ReturnsCorrectValue(bool disabled, bool expectedEnabled)
        {
            // Arrange
            var productId = _fixture.Create<int>();
            var customerProductStatus = new CustomerProductStatus(productId, disabled);

            // Act
            var result = customerProductStatus.Enabled;

            // Assert
            Assert.Equal(expectedEnabled, result);
        }

        [Theory]
        [InlineData(false, true)]   // Not hidden = can be seen
        [InlineData(true, false)]   // Hidden = cannot be seen
        public void CanBeSeen_WithDifferentHiddenValues_ReturnsCorrectValue(bool hidden, bool expectedCanBeSeen)
        {
            // Arrange
            var productId = _fixture.Create<int>();
            var customerProductStatus = new CustomerProductStatus(productId, false, hidden);

            // Act
            var result = customerProductStatus.CanBeSeen;

            // Assert
            Assert.Equal(expectedCanBeSeen, result);
        }

        [Fact]
        public void ProductGroupId_WhenProvided_SetsValueCorrectly()
        {
            // Arrange
            var productId = _fixture.Create<int>();
            var productGroupId = _fixture.Create<int>();

            // Act
            var customerProductStatus = new CustomerProductStatus(productId, false, false, productGroupId);

            // Assert
            Assert.Equal(productGroupId, customerProductStatus.ProductGroupId);
        }

        [Fact]
        public void ProductGroupId_WhenNotProvided_IsNull()
        {
            // Arrange
            var productId = _fixture.Create<int>();

            // Act
            var customerProductStatus = new CustomerProductStatus(productId);

            // Assert
            Assert.Null(customerProductStatus.ProductGroupId);
        }

        [Fact]
        public void CustomerProductStatus_ImplementsICustomerProductStatus()
        {
            // Arrange
            var productId = _fixture.Create<int>();

            // Act
            var customerProductStatus = new CustomerProductStatus(productId);

            // Assert
            Assert.IsAssignableFrom<ICustomerProductStatus>(customerProductStatus);
        }

        [Fact]
        public void CustomerProductStatus_PropertiesAreReadOnly()
        {
            // Arrange
            var productId = _fixture.Create<int>();
            var customerProductStatus = new CustomerProductStatus(productId);

            // Act & Assert
            var type = customerProductStatus.GetType();
            var productIdProperty = type.GetProperty(nameof(CustomerProductStatus.ProductId));
            var disabledProperty = type.GetProperty(nameof(CustomerProductStatus.Disabled));
            var hiddenProperty = type.GetProperty(nameof(CustomerProductStatus.Hidden));
            var productGroupIdProperty = type.GetProperty(nameof(CustomerProductStatus.ProductGroupId));

            Assert.True(productIdProperty?.CanWrite == false);
            Assert.True(disabledProperty?.CanWrite == false);
            Assert.True(hiddenProperty?.CanWrite == false);
            Assert.True(productGroupIdProperty?.CanWrite == false);
        }
    }
}

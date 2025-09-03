using AlphaPlus.Features.Messages.API.Models.Product;
using Xunit;

namespace AlphaPlus.Features.Messages.API.UnitTests.Models.Product
{
    public class ProductTests
    {
        [Fact]
        public void DefaultConstructor_InitializesWithDefaults()
        {
            // Act
            var product = new AlphaPlus.Features.Messages.API.Models.Product.Product();

            // Assert
            Assert.Equal(0, product.Id);
            Assert.Equal(string.Empty, product.Name);
            Assert.False(product.IsHidden);
        }

        [Fact]
        public void ParameterizedConstructor_SetsPropertiesCorrectly()
        {
            // Arrange
            var id = 123;
            var name = "Test Product";
            var isHidden = true;

            // Act
            var product = new AlphaPlus.Features.Messages.API.Models.Product.Product(id, name, isHidden);

            // Assert
            Assert.Equal(id, product.Id);
            Assert.Equal(name, product.Name);
            Assert.Equal(isHidden, product.IsHidden);
        }

        [Fact]
        public void Id_CanBeSetAndRetrieved()
        {
            // Arrange
            var product = new AlphaPlus.Features.Messages.API.Models.Product.Product();
            var id = 456;

            // Act
            product.Id = id;

            // Assert
            Assert.Equal(id, product.Id);
        }

        [Fact]
        public void Name_CanBeSetAndRetrieved()
        {
            // Arrange
            var product = new AlphaPlus.Features.Messages.API.Models.Product.Product();
            var name = "New Product Name";

            // Act
            product.Name = name;

            // Assert
            Assert.Equal(name, product.Name);
        }

        [Fact]
        public void IsHidden_CanBeSetAndRetrieved()
        {
            // Arrange
            var product = new AlphaPlus.Features.Messages.API.Models.Product.Product();

            // Act
            product.IsHidden = true;

            // Assert
            Assert.True(product.IsHidden);

            // Act
            product.IsHidden = false;

            // Assert
            Assert.False(product.IsHidden);
        }

        [Theory]
        [InlineData(1, "Product 1", true)]
        [InlineData(2, "Product 2", false)]
        [InlineData(0, "", true)]
        [InlineData(-1, "Negative ID Product", false)]
        public void Constructor_WithVariousInputs_SetsPropertiesCorrectly(int id, string name, bool isHidden)
        {
            // Act
            var product = new AlphaPlus.Features.Messages.API.Models.Product.Product(id, name, isHidden);

            // Assert
            Assert.Equal(id, product.Id);
            Assert.Equal(name, product.Name);
            Assert.Equal(isHidden, product.IsHidden);
        }

        [Fact]
        public void Constructor_WithNullName_SetsNameCorrectly()
        {
            // Act
            var product = new AlphaPlus.Features.Messages.API.Models.Product.Product(1, null, false);

            // Assert
            Assert.Null(product.Name);
        }

        [Fact]
        public void Name_CanBeSetToNull()
        {
            // Arrange
            var product = new AlphaPlus.Features.Messages.API.Models.Product.Product();

            // Act
            product.Name = null;

            // Assert
            Assert.Null(product.Name);
        }

        [Fact]
        public void Name_CanBeSetToEmptyString()
        {
            // Arrange
            var product = new AlphaPlus.Features.Messages.API.Models.Product.Product();

            // Act
            product.Name = string.Empty;

            // Assert
            Assert.Equal(string.Empty, product.Name);
        }

        [Fact]
        public void Name_CanBeSetToWhitespace()
        {
            // Arrange
            var product = new AlphaPlus.Features.Messages.API.Models.Product.Product();
            var whitespace = "   ";

            // Act
            product.Name = whitespace;

            // Assert
            Assert.Equal(whitespace, product.Name);
        }

        [Fact]
        public void Properties_AreIndependent()
        {
            // Arrange
            var product = new AlphaPlus.Features.Messages.API.Models.Product.Product();

            // Act
            product.Id = 100;
            product.Name = "Independent Test";
            product.IsHidden = true;

            // Assert
            Assert.Equal(100, product.Id);
            Assert.Equal("Independent Test", product.Name);
            Assert.True(product.IsHidden);

            // Act - Change one property
            product.Id = 200;

            // Assert - Other properties remain unchanged
            Assert.Equal(200, product.Id);
            Assert.Equal("Independent Test", product.Name);
            Assert.True(product.IsHidden);
        }

        [Fact]
        public void Product_CanBeModifiedAfterConstruction()
        {
            // Arrange
            var product = new AlphaPlus.Features.Messages.API.Models.Product.Product(1, "Original", false);

            // Act
            product.Id = 999;
            product.Name = "Modified";
            product.IsHidden = true;

            // Assert
            Assert.Equal(999, product.Id);
            Assert.Equal("Modified", product.Name);
            Assert.True(product.IsHidden);
        }

        [Fact]
        public void Properties_HaveCorrectTypes()
        {
            // Arrange
            var product = new AlphaPlus.Features.Messages.API.Models.Product.Product();

            // Assert
            Assert.IsType<int>(product.Id);
            Assert.IsType<string>(product.Name);
            Assert.IsType<bool>(product.IsHidden);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData(0)]
        public void Id_AcceptsExtremeValues(int extremeValue)
        {
            // Arrange
            var product = new AlphaPlus.Features.Messages.API.Models.Product.Product();

            // Act
            product.Id = extremeValue;

            // Assert
            Assert.Equal(extremeValue, product.Id);
        }

        [Fact]
        public void Name_AcceptsLongString()
        {
            // Arrange
            var product = new AlphaPlus.Features.Messages.API.Models.Product.Product();
            var longName = new string('A', 1000);

            // Act
            product.Name = longName;

            // Assert
            Assert.Equal(longName, product.Name);
            Assert.Equal(1000, product.Name.Length);
        }

        [Fact]
        public void Name_AcceptsSpecialCharacters()
        {
            // Arrange
            var product = new AlphaPlus.Features.Messages.API.Models.Product.Product();
            var specialName = "Product!@#$%^&*()_+-={}[]|\\:;\"'<>,.?/";

            // Act
            product.Name = specialName;

            // Assert
            Assert.Equal(specialName, product.Name);
        }

        [Fact]
        public void Name_AcceptsUnicodeCharacters()
        {
            // Arrange
            var product = new AlphaPlus.Features.Messages.API.Models.Product.Product();
            var unicodeName = "产品名称 商品 🛍️";

            // Act
            product.Name = unicodeName;

            // Assert
            Assert.Equal(unicodeName, product.Name);
        }

        [Fact]
        public void Product_CanBeCreatedWithAllDefaultValues()
        {
            // Act
            var product1 = new AlphaPlus.Features.Messages.API.Models.Product.Product();
            var product2 = new AlphaPlus.Features.Messages.API.Models.Product.Product(0, string.Empty, false);

            // Assert
            Assert.Equal(product1.Id, product2.Id);
            Assert.Equal(product1.Name, product2.Name);
            Assert.Equal(product1.IsHidden, product2.IsHidden);
        }
    }
}

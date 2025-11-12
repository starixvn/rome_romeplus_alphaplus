using AlphaPlus.Features.Handbook.API.Models;
using Xunit;

namespace AlphaPlus.Features.Handbook.API.UnitTests.Models
{
    public class MarketModelTests
    {
        [Fact]
        public void DefaultConstructor_InitializesWithDefaults()
        {
            // Act
            var market = new MarketModel();

            // Assert
            Assert.Null(market.Name);
        }

        [Fact]
        public void Name_CanBeSetAndRetrieved()
        {
            // Arrange
            var market = new MarketModel();
            var testName = "Test Market";

            // Act
            market.Name = testName;

            // Assert
            Assert.Equal(testName, market.Name);
        }

        [Theory]
        [InlineData("Stock Market")]
        [InlineData("Forex Market")]
        [InlineData("Crypto Market")]
        [InlineData("Commodity Market")]
        [InlineData("")]
        public void Name_WithDifferentValues_SetCorrectly(string name)
        {
            // Arrange
            var market = new MarketModel();

            // Act
            market.Name = name;

            // Assert
            Assert.Equal(name, market.Name);
        }

        [Fact]
        public void Name_CanBeSetToNull()
        {
            // Arrange
            var market = new MarketModel();

            // Act
            market.Name = null;

            // Assert
            Assert.Null(market.Name);
        }

        [Fact]
        public void Name_CanBeSetToEmptyString()
        {
            // Arrange
            var market = new MarketModel();

            // Act
            market.Name = string.Empty;

            // Assert
            Assert.Equal(string.Empty, market.Name);
        }

        [Fact]
        public void Name_HasCorrectType()
        {
            // Arrange
            var market = new MarketModel();

            // Act
            market.Name = "test";

            // Assert
            Assert.IsType<string>(market.Name);
        }

        [Fact]
        public void Name_AcceptsSpecialCharacters()
        {
            // Arrange
            var market = new MarketModel();
            var specialName = "Market @#$%^&*()_+-={}[]|\\:;\"'<>,.?/";

            // Act
            market.Name = specialName;

            // Assert
            Assert.Equal(specialName, market.Name);
        }

        [Fact]
        public void Name_AcceptsUnicodeCharacters()
        {
            // Arrange
            var market = new MarketModel();
            var unicodeName = "Market 市场 📈 Marché";

            // Act
            market.Name = unicodeName;

            // Assert
            Assert.Equal(unicodeName, market.Name);
        }

        [Fact]
        public void Name_AcceptsLongString()
        {
            // Arrange
            var market = new MarketModel();
            var longName = new string('M', 1000);

            // Act
            market.Name = longName;

            // Assert
            Assert.Equal(longName, market.Name);
            Assert.Equal(1000, market.Name.Length);
        }

        [Fact]
        public void Name_AcceptsWhitespaceString()
        {
            // Arrange
            var market = new MarketModel();
            var whitespaceName = "   Market with spaces   ";

            // Act
            market.Name = whitespaceName;

            // Assert
            Assert.Equal(whitespaceName, market.Name);
        }

        [Fact]
        public void Name_AcceptsOnlyWhitespace()
        {
            // Arrange
            var market = new MarketModel();
            var onlyWhitespace = "   \t\n\r   ";

            // Act
            market.Name = onlyWhitespace;

            // Assert
            Assert.Equal(onlyWhitespace, market.Name);
        }

        [Fact]
        public void Model_CanBeUsedAsCollectionItem()
        {
            // Arrange
            var markets = new List<MarketModel>();

            // Act
            markets.Add(new MarketModel { Name = "NYSE" });
            markets.Add(new MarketModel { Name = "NASDAQ" });
            markets.Add(new MarketModel { Name = "LSE" });

            // Assert
            Assert.Equal(3, markets.Count);
            Assert.Equal("NYSE", markets[0].Name);
            Assert.Equal("NASDAQ", markets[1].Name);
            Assert.Equal("LSE", markets[2].Name);
        }

        [Fact]
        public void Model_SupportsObjectInitializerSyntax()
        {
            // Act
            var market = new MarketModel
            {
                Name = "Tokyo Stock Exchange"
            };

            // Assert
            Assert.Equal("Tokyo Stock Exchange", market.Name);
        }

        [Fact]
        public void Name_CanBeModifiedAfterInitialization()
        {
            // Arrange
            var market = new MarketModel
            {
                Name = "Original Market"
            };

            // Act
            market.Name = "Modified Market";

            // Assert
            Assert.Equal("Modified Market", market.Name);
        }

        [Fact]
        public void Model_HasExpectedPropertyCount()
        {
            // Assert
            Assert.Contains(typeof(MarketModel).GetProperty(nameof(MarketModel.Name)),
                typeof(MarketModel).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance));
        }

        [Fact]
        public void Name_IsPublicAndReadWrite()
        {
            // Arrange
            var type = typeof(MarketModel);
            var nameProperty = type.GetProperty(nameof(MarketModel.Name));

            // Assert
            Assert.NotNull(nameProperty);
            Assert.True(nameProperty.CanRead);
            Assert.True(nameProperty.CanWrite);
            Assert.Equal(typeof(string), nameProperty.PropertyType);
        }

        [Fact]
        public void Model_AllowsMultipleInstancesWithDifferentNames()
        {
            // Arrange & Act
            var market1 = new MarketModel { Name = "Market 1" };
            var market2 = new MarketModel { Name = "Market 2" };

            // Assert
            Assert.NotEqual(market1.Name, market2.Name);
            Assert.Equal("Market 1", market1.Name);
            Assert.Equal("Market 2", market2.Name);
        }

        [Fact]
        public void Name_CanStoreNumericStrings()
        {
            // Arrange
            var market = new MarketModel();
            var numericName = "12345";

            // Act
            market.Name = numericName;

            // Assert
            Assert.Equal(numericName, market.Name);
            Assert.IsType<string>(market.Name);
        }

        [Fact]
        public void Name_CanStoreJsonLikeStrings()
        {
            // Arrange
            var market = new MarketModel();
            var jsonLikeName = "{\"market\": \"NYSE\", \"type\": \"stock\"}";

            // Act
            market.Name = jsonLikeName;

            // Assert
            Assert.Equal(jsonLikeName, market.Name);
        }

        [Fact]
        public void Name_CanStoreMultilineStrings()
        {
            // Arrange
            var market = new MarketModel();
            var multilineName = "Market\nWith\nMultiple\nLines";

            // Act
            market.Name = multilineName;

            // Assert
            Assert.Equal(multilineName, market.Name);
            Assert.Contains("\n", market.Name);
        }

        [Fact]
        public void Name_PreservesLeadingAndTrailingSpaces()
        {
            // Arrange
            var market = new MarketModel();
            var nameWithSpaces = "  Market Name  ";

            // Act
            market.Name = nameWithSpaces;

            // Assert
            Assert.Equal(nameWithSpaces, market.Name);
            Assert.StartsWith("  ", market.Name);
            Assert.EndsWith("  ", market.Name);
        }
    }
}
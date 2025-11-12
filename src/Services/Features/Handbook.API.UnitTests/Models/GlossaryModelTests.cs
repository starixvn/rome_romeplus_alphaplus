using AlphaPlus.Features.Handbook.API.Models;
using Xunit;

namespace AlphaPlus.Features.Handbook.API.UnitTests.Models
{
    public class GlossaryModelTests
    {
        [Fact]
        public void DefaultConstructor_InitializesWithDefaults()
        {
            // Act
            var glossary = new GlossaryModel();

            // Assert
            Assert.Null(glossary.Name);
            Assert.Null(glossary.Content);
            Assert.Null(glossary.Title);
        }

        [Fact]
        public void Name_CanBeSetAndRetrieved()
        {
            // Arrange
            var glossary = new GlossaryModel();
            var testName = "Test Glossary";

            // Act
            glossary.Name = testName;

            // Assert
            Assert.Equal(testName, glossary.Name);
        }

        [Fact]
        public void Content_CanBeSetAndRetrieved()
        {
            // Arrange
            var glossary = new GlossaryModel();
            var testContent = "This is test content for the glossary item.";

            // Act
            glossary.Content = testContent;

            // Assert
            Assert.Equal(testContent, glossary.Content);
        }

        [Fact]
        public void Title_CanBeSetAndRetrieved()
        {
            // Arrange
            var glossary = new GlossaryModel();
            var testTitle = "Test Title";

            // Act
            glossary.Title = testTitle;

            // Assert
            Assert.Equal(testTitle, glossary.Title);
        }

        [Fact]
        public void Group_WithNullTitle_ThrowsNullReferenceException()
        {
            // Arrange
            var glossary = new GlossaryModel();
            glossary.Title = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => glossary.Group);
        }

        [Fact]
        public void Group_WithEmptyTitle_ReturnsHash()
        {
            // Arrange
            var glossary = new GlossaryModel();
            glossary.Title = string.Empty;

            // Act
            var result = glossary.Group;

            // Assert
            Assert.Equal("#", result);
        }

        [Theory]
        [InlineData("Apple", "A")]
        [InlineData("Book", "B")]
        [InlineData("Cat", "C")]
        [InlineData("Dog", "D")]
        [InlineData("Elephant", "E")]
        [InlineData("Fish", "F")]
        [InlineData("Giraffe", "G")]
        [InlineData("Horse", "H")]
        [InlineData("Ice", "I")]
        [InlineData("Jungle", "J")]
        [InlineData("Kite", "K")]
        [InlineData("Lion", "L")]
        [InlineData("Mouse", "M")]
        [InlineData("Nest", "N")]
        [InlineData("Ocean", "O")]
        [InlineData("Penguin", "P")]
        [InlineData("Queen", "Q")]
        [InlineData("Rabbit", "R")]
        [InlineData("Snake", "S")]
        [InlineData("Tiger", "T")]
        [InlineData("Umbrella", "U")]
        [InlineData("Violin", "V")]
        [InlineData("Whale", "W")]
        [InlineData("Xylophone", "X")]
        [InlineData("Yacht", "Y")]
        [InlineData("Zebra", "Z")]
        public void Group_WithAlphabeticalTitles_ReturnsFirstLetter(string title, string expectedGroup)
        {
            // Arrange
            var glossary = new GlossaryModel();
            glossary.Title = title;

            // Act
            var result = glossary.Group;

            // Assert
            Assert.Equal(expectedGroup, result);
        }

        [Theory]
        [InlineData("apple", "a")]
        [InlineData("book", "b")]
        [InlineData("cat", "c")]
        [InlineData("dog", "d")]
        public void Group_WithLowercaseTitles_ReturnsLowercaseFirstLetter(string title, string expectedGroup)
        {
            // Arrange
            var glossary = new GlossaryModel();
            glossary.Title = title;

            // Act
            var result = glossary.Group;

            // Assert
            Assert.Equal(expectedGroup, result);
        }

        [Theory]
        [InlineData("APPLE", "A")]
        [InlineData("BOOK", "B")]
        [InlineData("CAT", "C")]
        public void Group_WithUppercaseTitles_ReturnsUppercaseFirstLetter(string title, string expectedGroup)
        {
            // Arrange
            var glossary = new GlossaryModel();
            glossary.Title = title;

            // Act
            var result = glossary.Group;

            // Assert
            Assert.Equal(expectedGroup, result);
        }

        [Theory]
        [InlineData("123", "#")]
        [InlineData("456 Numbers", "#")]
        [InlineData("789-Test", "#")]
        [InlineData("!@#$%", "#")]
        [InlineData("&*()_", "#")]
        [InlineData("[]{}|", "#")]
        [InlineData("+=<>?", "#")]
        public void Group_WithNonAlphabeticalStart_ReturnsHash(string title, string expectedGroup)
        {
            // Arrange
            var glossary = new GlossaryModel();
            glossary.Title = title;

            // Act
            var result = glossary.Group;

            // Assert
            Assert.Equal(expectedGroup, result);
        }

        [Fact]
        public void Group_WithHashStartingTitle_ReturnsHash()
        {
            // Arrange
            var glossary = new GlossaryModel();
            glossary.Title = "#hashtag";

            // Act
            var result = glossary.Group;

            // Assert
            Assert.Equal("#", result);
        }

        [Theory]
        [InlineData("中文", "#")]
        [InlineData("日本語", "#")]
        [InlineData("한국어", "#")]
        [InlineData("العربية", "#")]
        [InlineData("русский", "#")]
        public void Group_WithUnicodeStart_ReturnsHash(string title, string expectedGroup)
        {
            // Arrange
            var glossary = new GlossaryModel();
            glossary.Title = title;

            // Act
            var result = glossary.Group;

            // Assert
            Assert.Equal(expectedGroup, result);
        }

        [Theory]
        [InlineData("  Apple", "#")]
        [InlineData("\tBook", "#")]
        [InlineData("\nCat", "#")]
        public void Group_WithWhitespaceStart_ReturnsHash(string title, string expectedGroup)
        {
            // Arrange
            var glossary = new GlossaryModel();
            glossary.Title = title;

            // Act
            var result = glossary.Group;

            // Assert
            Assert.Equal(expectedGroup, result);
        }

        [Fact]
        public void Group_WithMixedCaseAlphabetStart_WorksCorrectly()
        {
            // Arrange
            var testCases = new[]
            {
                ("aPPLE", "a"),
                ("BooK", "B"),
                ("cAT", "c"),
                ("DoG", "D")
            };

            foreach (var (title, expected) in testCases)
            {
                var glossary = new GlossaryModel { Title = title };

                // Act
                var result = glossary.Group;

                // Assert
                Assert.Equal(expected, result);
            }
        }

        [Fact]
        public void Properties_AreIndependent()
        {
            // Arrange
            var glossary = new GlossaryModel();

            // Act
            glossary.Name = "Test Name";
            glossary.Content = "Test Content";
            glossary.Title = "Test Title";

            // Assert
            Assert.Equal("Test Name", glossary.Name);
            Assert.Equal("Test Content", glossary.Content);
            Assert.Equal("Test Title", glossary.Title);
            Assert.Equal("T", glossary.Group);

            // Act - Change one property
            glossary.Name = "New Name";

            // Assert - Other properties remain unchanged
            Assert.Equal("New Name", glossary.Name);
            Assert.Equal("Test Content", glossary.Content);
            Assert.Equal("Test Title", glossary.Title);
            Assert.Equal("T", glossary.Group);
        }

        [Fact]
        public void Properties_CanBeSetToNull()
        {
            // Arrange
            var glossary = new GlossaryModel();

            // Act
            glossary.Name = null;
            glossary.Content = null;
            glossary.Title = null;

            // Assert
            Assert.Null(glossary.Name);
            Assert.Null(glossary.Content);
            Assert.Null(glossary.Title);
            // Note: Group will throw NullReferenceException when Title is null
        }

        [Fact]
        public void Properties_CanBeSetToEmptyString()
        {
            // Arrange
            var glossary = new GlossaryModel();

            // Act
            glossary.Name = string.Empty;
            glossary.Content = string.Empty;
            glossary.Title = string.Empty;

            // Assert
            Assert.Equal(string.Empty, glossary.Name);
            Assert.Equal(string.Empty, glossary.Content);
            Assert.Equal(string.Empty, glossary.Title);
            Assert.Equal("#", glossary.Group);
        }

        [Fact]
        public void Properties_HaveCorrectTypes()
        {
            // Arrange
            var glossary = new GlossaryModel();

            // Act
            glossary.Name = "test";
            glossary.Content = "test";
            glossary.Title = "test";

            // Assert
            Assert.IsType<string>(glossary.Name);
            Assert.IsType<string>(glossary.Content);
            Assert.IsType<string>(glossary.Title);
            Assert.IsType<string>(glossary.Group);
        }

        [Fact]
        public void Model_SupportsObjectInitializerSyntax()
        {
            // Act
            var glossary = new GlossaryModel
            {
                Name = "Init Name",
                Content = "Init Content",
                Title = "Init Title"
            };

            // Assert
            Assert.Equal("Init Name", glossary.Name);
            Assert.Equal("Init Content", glossary.Content);
            Assert.Equal("Init Title", glossary.Title);
            Assert.Equal("I", glossary.Group);
        }

        [Fact]
        public void Model_CanBeUsedAsCollectionItem()
        {
            // Arrange
            var glossaries = new List<GlossaryModel>();

            // Act
            glossaries.Add(new GlossaryModel { Name = "Term1", Content = "Content1", Title = "Apple" });
            glossaries.Add(new GlossaryModel { Name = "Term2", Content = "Content2", Title = "Banana" });

            // Assert
            Assert.Equal(2, glossaries.Count);
            Assert.Equal("Term1", glossaries[0].Name);
            Assert.Equal("A", glossaries[0].Group);
            Assert.Equal("Term2", glossaries[1].Name);
            Assert.Equal("B", glossaries[1].Group);
        }

        [Fact]
        public void Model_HasExpectedPropertyCount()
        {
            // Arrange
            var propertyCount = typeof(GlossaryModel)
                .GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                .Length;

            // Assert
            Assert.Equal(4, propertyCount); // Name, Content, Title, Group
        }

        [Fact]
        public void Group_IsReadOnlyProperty()
        {
            // Arrange
            var type = typeof(GlossaryModel);
            var groupProperty = type.GetProperty(nameof(GlossaryModel.Group));

            // Assert
            Assert.NotNull(groupProperty);
            Assert.True(groupProperty.CanRead);
            Assert.False(groupProperty.CanWrite); // Should be read-only (computed property)
        }

        [Fact]
        public void Group_ChangesWhenTitleChanges()
        {
            // Arrange
            var glossary = new GlossaryModel();
            glossary.Title = "Apple";

            // Act & Assert - First value
            Assert.Equal("A", glossary.Group);

            // Act - Change title
            glossary.Title = "Banana";

            // Assert - Group changes
            Assert.Equal("B", glossary.Group);

            // Act - Change to non-alphabetical
            glossary.Title = "123";

            // Assert - Group changes to hash
            Assert.Equal("#", glossary.Group);
        }

        [Fact]
        public void Group_WithComplexTitles_WorksCorrectly()
        {
            // Arrange & Act & Assert
            var testCases = new[]
            {
                ("A1B2C3", "A"),
                ("Z9Y8X7", "Z"),
                ("Apple Pie", "A"),
                ("123 Apple", "#"),
                ("!@# Test", "#"),
                ("   Leading Space", "#"),
                ("m-dash", "m"),
                ("X-ray", "X")
            };

            foreach (var (title, expectedGroup) in testCases)
            {
                var glossary = new GlossaryModel { Title = title };
                Assert.Equal(expectedGroup, glossary.Group);
            }
        }

        [Fact]
        public void Properties_AcceptLongStrings()
        {
            // Arrange
            var glossary = new GlossaryModel();
            var longName = new string('N', 1000);
            var longContent = new string('C', 2000);
            var longTitle = new string('T', 500);

            // Act
            glossary.Name = longName;
            glossary.Content = longContent;
            glossary.Title = longTitle;

            // Assert
            Assert.Equal(1000, glossary.Name.Length);
            Assert.Equal(2000, glossary.Content.Length);
            Assert.Equal(500, glossary.Title.Length);
            Assert.Equal("T", glossary.Group); // First character
        }
    }
}

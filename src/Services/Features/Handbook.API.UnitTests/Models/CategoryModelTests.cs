using AlphaPlus.Features.Handbook.API.Models;
using Xunit;

namespace AlphaPlus.Features.Handbook.API.UnitTests.Models
{
    public class CategoryModelTests
    {
        [Fact]
        public void GetUrl_ReturnsCorrectHandbookCategoryUrl()
        {
            // Arrange
            var category = new CategoryModel
            {
                Slug = "test-category"
            };

            // Act
            var result = category.GetUrl();

            // Assert
            Assert.Equal("HandbookCategory/GetBySlug?slug=test-category", result);
        }

        [Fact]
        public void GetUrl_WithRelativePath_PrependRelativePath()
        {
            // Arrange
            var category = new CategoryModel
            {
                Slug = "test-category"
            };
            var relativePath = "/features/";

            // Act
            var result = category.GetUrl(relativePath);

            // Assert
            Assert.Equal("/features/HandbookCategory/GetBySlug?slug=test-category", result);
        }

        [Fact]
        public void GetUrl_WithEmptySlug_ReturnsUrlWithEmptySlug()
        {
            // Arrange
            var category = new CategoryModel
            {
                Slug = ""
            };

            // Act
            var result = category.GetUrl();

            // Assert
            Assert.Equal("HandbookCategory/GetBySlug?slug=", result);
        }

        [Fact]
        public void GetUrl_WithNullSlug_ReturnsUrlWithNullSlug()
        {
            // Arrange
            var category = new CategoryModel
            {
                Slug = null
            };

            // Act
            var result = category.GetUrl();

            // Assert
            Assert.Equal("HandbookCategory/GetBySlug?slug=", result);
        }

        [Fact]
        public void Properties_CanBeSetAndRetrieved()
        {
            // Arrange
            var parentCategories = new List<CategoryModel> { new CategoryModel { Name = "Parent" } };
            var subCategories = new List<CategoryModel> { new CategoryModel { Name = "Sub" } };
            var markets = new List<MarketModel> { new MarketModel() };
            var articles = new List<ArticleModel> { new ArticleModel { Name = "Article" } };

            var category = new CategoryModel
            {
                Name = "Test Category",
                Slug = "test-category",
                ShortName = "Test",
                Order = 5,
                CategoriesParent = parentCategories,
                Categories = subCategories,
                Markets = markets,
                Articles = articles
            };

            // Act & Assert
            Assert.Equal("Test Category", category.Name);
            Assert.Equal("test-category", category.Slug);
            Assert.Equal("Test", category.ShortName);
            Assert.Equal(5, category.Order);
            Assert.Equal(parentCategories, category.CategoriesParent);
            Assert.Equal(subCategories, category.Categories);
            Assert.Equal(markets, category.Markets);
            Assert.Equal(articles, category.Articles);
        }

        [Fact]
        public void DefaultInitialization_CollectionsAreEmptyLists()
        {
            // Arrange & Act
            var category = new CategoryModel();

            // Assert
            Assert.NotNull(category.CategoriesParent);
            Assert.Empty(category.CategoriesParent);
            Assert.NotNull(category.Categories);
            Assert.Empty(category.Categories);
            Assert.NotNull(category.Markets);
            Assert.Empty(category.Markets);
            Assert.NotNull(category.Articles);
            Assert.Empty(category.Articles);
        }

        [Fact]
        public void CategoriesParent_CanBeModified()
        {
            // Arrange
            var category = new CategoryModel();
            var parentCategory = new CategoryModel { Name = "Parent Category" };

            // Act
            category.CategoriesParent = new List<CategoryModel> { parentCategory };

            // Assert
            Assert.Single(category.CategoriesParent);
            Assert.Equal("Parent Category", category.CategoriesParent.First().Name);
        }

        [Fact]
        public void Categories_CanBeModified()
        {
            // Arrange
            var category = new CategoryModel();
            var subCategory = new CategoryModel { Name = "Sub Category" };

            // Act
            category.Categories = new List<CategoryModel> { subCategory };

            // Assert
            Assert.Single(category.Categories);
            Assert.Equal("Sub Category", category.Categories.First().Name);
        }

        [Fact]
        public void Markets_CanBeModified()
        {
            // Arrange
            var category = new CategoryModel();
            var market = new MarketModel();

            // Act
            category.Markets = new List<MarketModel> { market };

            // Assert
            Assert.Single(category.Markets);
        }

        [Fact]
        public void Articles_CanBeModified()
        {
            // Arrange
            var category = new CategoryModel();
            var article = new ArticleModel { Name = "Test Article" };

            // Act
            category.Articles = new List<ArticleModel> { article };

            // Assert
            Assert.Single(category.Articles);
            Assert.Equal("Test Article", category.Articles.First().Name);
        }

        [Theory]
        [InlineData("category-with-dash")]
        [InlineData("category_with_underscore")]
        [InlineData("category123")]
        [InlineData("CATEGORY")]
        [InlineData("Category")]
        public void GetUrl_WithVariousSlugFormats_ReturnsCorrectUrl(string slug)
        {
            // Arrange
            var category = new CategoryModel
            {
                Slug = slug
            };

            // Act
            var result = category.GetUrl();

            // Assert
            Assert.Equal($"HandbookCategory/GetBySlug?slug={slug}", result);
        }
    }
}

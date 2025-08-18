namespace AlphaPlus.Products.UnitTests.ViewModels
{
    public class ViewModelTests
    {
        private readonly Fixture _fixture;

        public ViewModelTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void IViewModel_Interface_DefinesDisplayOrderProperty()
        {
            // Arrange
            var type = typeof(IViewModel);

            // Act
            var displayOrderProperty = type.GetProperty(nameof(IViewModel.DisplayOrder));

            // Assert
            Assert.NotNull(displayOrderProperty);
            Assert.Equal(typeof(int), displayOrderProperty.PropertyType);
        }

        [Fact]
        public void ProductViewModel_Constructor_CreatesDefaultValues()
        {
            // Arrange
            var id = 1;
            var name = "Test Product";

            // Act
            var productViewModel = new ProductViewModel(id, name);

            // Assert
            Assert.Equal(id, productViewModel.DisplayOrder);
            Assert.Equal(name, productViewModel.Name);
            Assert.True(productViewModel.Visible);
        }

        [Fact]
        public void ProductViewModel_WithValidData_SetsPropertiesCorrectly()
        {
            // Arrange
            var id = _fixture.Create<int>();
            var name = _fixture.Create<string>();

            // Act
            var productViewModel = new ProductViewModel(id, name);

            // Assert
            Assert.Equal(id, productViewModel.DisplayOrder);
            Assert.Equal(name, productViewModel.Name);
            Assert.True(productViewModel.Visible);
        }

        [Fact]
        public void ProductViewModel_ImplementsIViewModel()
        {
            // Arrange
            var productViewModel = new ProductViewModel(1, "Test");

            // Act & Assert
            Assert.IsAssignableFrom<IViewModel>(productViewModel);
        }

        [Fact]
        public void SettingProductViewModel_Constructor_CreatesDefaultValues()
        {
            // Act
            var settingProductViewModel = new SettingProductViewModel();

            // Assert
            Assert.Null(settingProductViewModel.Products);
        }

        [Fact]
        public void SettingProductViewModel_WithValidData_SetsPropertiesCorrectly()
        {
            // Arrange
            var products = new List<SettingProduct>
            {
                new SettingProduct { Id = 1, Name = "Product 1", Active = true },
                new SettingProduct { Id = 2, Name = "Product 2", Active = false }
            };

            // Act
            var settingProductViewModel = new SettingProductViewModel
            {
                Products = products
            };

            // Assert
            Assert.Equal(products, settingProductViewModel.Products);
        }

        [Fact]
        public void IRequest_Interface_DefinesRequiredProperties()
        {
            // Arrange
            var type = typeof(IRequest);

            // Act
            var isValidProperty = type.GetProperty(nameof(IRequest.IsValid));
            var actorProperty = type.GetProperty(nameof(IRequest.Actor));
            var predicateProperty = type.GetProperty(nameof(IRequest.Predicate));
            var getProductGroupNameProperty = type.GetProperty(nameof(IRequest.GetProductGroupName));
            var getProductNameProperty = type.GetProperty(nameof(IRequest.GetProductName));
            var getResourceProperty = type.GetProperty(nameof(IRequest.GetResource));

            // Assert
            Assert.NotNull(isValidProperty);
            Assert.Equal(typeof(bool), isValidProperty.PropertyType);

            Assert.NotNull(actorProperty);
            Assert.Equal(typeof(ICustomer), actorProperty.PropertyType);

            Assert.NotNull(predicateProperty);
            Assert.Equal(typeof(Func<ProductItem, bool>), predicateProperty.PropertyType);

            Assert.NotNull(getProductGroupNameProperty);
            Assert.Equal(typeof(Func<int, string>), getProductGroupNameProperty.PropertyType);

            Assert.NotNull(getProductNameProperty);
            Assert.Equal(typeof(Func<int, string>), getProductNameProperty.PropertyType);

            Assert.NotNull(getResourceProperty);
            Assert.Equal(typeof(Func<string, string>), getResourceProperty.PropertyType);
        }
    }
}

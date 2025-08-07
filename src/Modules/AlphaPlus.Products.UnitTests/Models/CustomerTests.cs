namespace AlphaPlus.Products.UnitTests.Models
{
    public class CustomerTests
    {
        private readonly Fixture _fixture;

        public CustomerTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void Constructor_WithValidParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var id = _fixture.Create<int>();
            var subAccountId = _fixture.Create<int>();
            var username = _fixture.Create<string>();
            var customerLevelValue = (byte)_fixture.Create<int>();
            var customerProductStatus = _fixture.CreateMany<CustomerProductStatus>();

            // Act
            var customer = new Customer(id, subAccountId, username, customerLevelValue, customerProductStatus);

            // Assert
            Assert.Equal(id, customer.Id);
            Assert.Equal(subAccountId, customer.SubAccountId);
            Assert.Equal(username, customer.Username);
            Assert.Equal(customerLevelValue, customer.CustomerLevelValue);
        }

        [Fact]
        public void Constructor_WithUplineParameters_SetsUplinePropertiesCorrectly()
        {
            // Arrange
            var id = _fixture.Create<int>();
            var subAccountId = _fixture.Create<int>();
            var username = _fixture.Create<string>();
            var customerLevelValue = (byte)_fixture.Create<int>();
            var agentId = _fixture.Create<int>();
            var isCashMember = _fixture.Create<bool>();
            var customerProductStatus = _fixture.CreateMany<CustomerProductStatus>();
            var mockUpline = new Mock<ICustomer>();

            // Act
            var customer = new Customer(id, subAccountId, username, customerLevelValue, agentId, isCashMember, customerProductStatus, mockUpline.Object);

            // Assert
            Assert.Equal(agentId, customer.AgentId);
            Assert.Equal(isCashMember, customer.IsCashMember);
            Assert.Equal(mockUpline.Object, customer.Upline);
        }

        [Theory]
        [InlineData(1, true)]   // Member
        [InlineData(2, true)]   // Agent
        [InlineData(3, true)]   // Master
        [InlineData(4, true)]   // Super
        public void CustomerLevel_WithDifferentValues_ReturnsCorrectEnumeration(byte levelValue, bool shouldBeValid)
        {
            // Arrange
            var customer = new Customer(1, 0, "test", levelValue, Enumerable.Empty<CustomerProductStatus>());

            // Act
            var customerLevel = customer.CustomerLevel;

            // Assert
            if (shouldBeValid)
            {
                Assert.NotNull(customerLevel);
                Assert.Equal(levelValue, customerLevel.Value);
            }
            else
            {
                Assert.Null(customerLevel);
            }
        }

        [Theory]
        [InlineData(0, false)]  // Main account
        [InlineData(1, true)]   // Sub account
        [InlineData(100, true)] // Sub account
        public void IsSubAccount_WithDifferentSubAccountIds_ReturnsCorrectValue(int subAccountId, bool expectedIsSubAccount)
        {
            // Arrange
            var customer = new Customer(1, subAccountId, "test", 1, Enumerable.Empty<CustomerProductStatus>());

            // Act
            var result = customer.IsSubAccount;

            // Assert
            Assert.Equal(expectedIsSubAccount, result);
        }

        [Theory]
        [InlineData(0, true)]   // Main account
        [InlineData(1, false)]  // Sub account
        public void IsMainAccount_WithDifferentSubAccountIds_ReturnsCorrectValue(int subAccountId, bool expectedIsMainAccount)
        {
            // Arrange
            var customer = new Customer(1, subAccountId, "test", 1, Enumerable.Empty<CustomerProductStatus>());

            // Act
            var result = customer.IsMainAccount;

            // Assert
            Assert.Equal(expectedIsMainAccount, result);
        }

        [Theory]
        [InlineData(1, false)] // Member
        [InlineData(2, true)]  // Agent
        [InlineData(3, false)] // Master
        [InlineData(4, false)] // Super
        public void IsAgent_WithDifferentLevels_ReturnsCorrectValue(byte levelValue, bool expectedIsAgent)
        {
            // Arrange
            var customer = new Customer(1, 0, "test", levelValue, Enumerable.Empty<CustomerProductStatus>());

            // Act
            var result = customer.IsAgent;

            // Assert
            Assert.Equal(expectedIsAgent, result);
        }

        [Theory]
        [InlineData(1, true)]  // Member
        [InlineData(2, false)] // Agent
        [InlineData(3, true)]  // Master
        [InlineData(4, true)]  // Super
        public void IsNotAgent_WithDifferentLevels_ReturnsCorrectValue(byte levelValue, bool expectedIsNotAgent)
        {
            // Arrange
            var customer = new Customer(1, 0, "test", levelValue, Enumerable.Empty<CustomerProductStatus>());

            // Act
            var result = customer.IsNotAgent;

            // Assert
            Assert.Equal(expectedIsNotAgent, result);
        }

        [Theory]
        [InlineData(1, false)] // Member
        [InlineData(2, false)] // Agent
        [InlineData(3, true)]  // Master
        [InlineData(4, false)] // Super
        public void IsMaster_WithDifferentLevels_ReturnsCorrectValue(byte levelValue, bool expectedIsMaster)
        {
            // Arrange
            var customer = new Customer(1, 0, "test", levelValue, Enumerable.Empty<CustomerProductStatus>());

            // Act
            var result = customer.IsMaster;

            // Assert
            Assert.Equal(expectedIsMaster, result);
        }

        [Theory]
        [InlineData(1, false)] // Member
        [InlineData(2, false)] // Agent
        [InlineData(3, false)] // Master
        [InlineData(4, true)]  // Super
        public void IsSuper_WithDifferentLevels_ReturnsCorrectValue(byte levelValue, bool expectedIsSuper)
        {
            // Arrange
            var customer = new Customer(1, 0, "test", levelValue, Enumerable.Empty<CustomerProductStatus>());

            // Act
            var result = customer.IsSuper;

            // Assert
            Assert.Equal(expectedIsSuper, result);
        }

        [Theory]
        [InlineData(1, false)] // Member
        [InlineData(2, false)] // Agent
        [InlineData(3, true)]  // Master
        [InlineData(4, true)]  // Super
        public void IsMasterOrSuper_WithDifferentLevels_ReturnsCorrectValue(byte levelValue, bool expectedIsMasterOrSuper)
        {
            // Arrange
            var customer = new Customer(1, 0, "test", levelValue, Enumerable.Empty<CustomerProductStatus>());

            // Act
            var result = customer.IsMasterOrSuper;

            // Assert
            Assert.Equal(expectedIsMasterOrSuper, result);
        }

        [Theory]
        [InlineData(1, true)]  // Member
        [InlineData(2, true)]  // Agent
        [InlineData(3, true)]  // Master
        [InlineData(4, false)] // Super
        public void IsNotSuper_WithDifferentLevels_ReturnsCorrectValue(byte levelValue, bool expectedIsNotSuper)
        {
            // Arrange
            var customer = new Customer(1, 0, "test", levelValue, Enumerable.Empty<CustomerProductStatus>());

            // Act
            var result = customer.IsNotSuper;

            // Assert
            Assert.Equal(expectedIsNotSuper, result);
        }

        [Theory]
        [InlineData(1, true)]  // Member
        [InlineData(2, false)] // Agent
        [InlineData(3, false)] // Master
        [InlineData(4, false)] // Super
        public void IsMember_WithDifferentLevels_ReturnsCorrectValue(byte levelValue, bool expectedIsMember)
        {
            // Arrange
            var customer = new Customer(1, 0, "test", levelValue, Enumerable.Empty<CustomerProductStatus>());

            // Act
            var result = customer.IsMember;

            // Assert
            Assert.Equal(expectedIsMember, result);
        }

        [Theory]
        [InlineData(1, false)] // Member
        [InlineData(2, true)]  // Agent
        [InlineData(3, true)]  // Master
        [InlineData(4, true)]  // Super
        public void IsNotMember_WithDifferentLevels_ReturnsCorrectValue(byte levelValue, bool expectedIsNotMember)
        {
            // Arrange
            var customer = new Customer(1, 0, "test", levelValue, Enumerable.Empty<CustomerProductStatus>());

            // Act
            var result = customer.IsNotMember;

            // Assert
            Assert.Equal(expectedIsNotMember, result);
        }

        [Fact]
        public void GetProductStatus_ReturnsCustomerProductStatus()
        {
            // Arrange
            var expectedProductStatus = _fixture.CreateMany<CustomerProductStatus>();
            var customer = new Customer(1, 0, "test", 1, expectedProductStatus);

            // Act
            var result = customer.GetProductStatus();

            // Assert
            Assert.Equal(expectedProductStatus, result);
        }

        [Fact]
        public void CanSeeProduct_WhenProductExistsAndCanBeSeen_ReturnsTrue()
        {
            // Arrange
            var productId = _fixture.Create<int>();
            var productStatus = new CustomerProductStatus(productId, false, false, null);
            var customer = new Customer(1, 0, "test", 1, new[] { productStatus });

            // Act
            var result = customer.CanSeeProduct(productId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CanSeeProduct_WhenProductExistsButCannotBeSeen_ReturnsFalse()
        {
            // Arrange
            var productId = _fixture.Create<int>();
            var productStatus = new CustomerProductStatus(productId, false, true, null);
            var customer = new Customer(1, 0, "test", 1, new[] { productStatus });

            // Act
            var result = customer.CanSeeProduct(productId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CanSeeProduct_WhenProductDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var productId = _fixture.Create<int>();
            var customer = new Customer(1, 0, "test", 1, Enumerable.Empty<CustomerProductStatus>());

            // Act
            var result = customer.CanSeeProduct(productId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ProductStatusOf_WhenProductExists_ReturnsProductStatus()
        {
            // Arrange
            var productId = _fixture.Create<int>();
            var expectedProductStatus = new CustomerProductStatus(productId, false, false, null);
            var customer = new Customer(1, 0, "test", 1, new[] { expectedProductStatus });

            // Act
            var result = customer.ProductStatusOf(productId);

            // Assert
            Assert.Equal(expectedProductStatus, result);
        }

        [Fact]
        public void ProductStatusOf_WhenProductDoesNotExist_ReturnsNull()
        {
            // Arrange
            var productId = _fixture.Create<int>();
            var customer = new Customer(1, 0, "test", 1, Enumerable.Empty<CustomerProductStatus>());

            // Act
            var result = customer.ProductStatusOf(productId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void IsUplineOf_WhenCustomerIsUpline_ReturnsTrue()
        {
            // Arrange
            var uplineCustomer = new Customer(1, 0, "upline", 3, Enumerable.Empty<CustomerProductStatus>()); // Master
            var downlineCustomer = new Customer(2, 0, "downline", 1, Enumerable.Empty<CustomerProductStatus>()); // Member
            var customer = new Customer(3, 0, "test", 4, 0, false, Enumerable.Empty<CustomerProductStatus>(), uplineCustomer);

            // Act
            var result = customer.IsUplineOf(downlineCustomer);

            // Assert
            Assert.True(result);
        }
    }
}

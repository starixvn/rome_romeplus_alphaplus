using AlphaPlus.SmartPunter.Criteria;
using AutoFixture;

namespace AlphaPlus.SmartPunter.UnitTests.Criteria
{
    public class SmartPunterSMACheckerTests
    {
        private readonly Fixture _fixture;

        public SmartPunterSMACheckerTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void Constructor_WithCustomerId_ShouldSetCustomerIdAndNullUsername()
        {
            // Arrange
            int customerId = _fixture.Create<int>();

            // Act
            var checker = new SmartPunterSMAChecker(customerId);

            // Assert
            Assert.Equal(customerId, checker.CustId);
            Assert.Null(checker.Username);
        }

        [Fact]
        public void Constructor_WithUsername_ShouldSetUsernameAndNullCustomerId()
        {
            // Arrange
            string username = _fixture.Create<string>();

            // Act
            var checker = new SmartPunterSMAChecker(username);

            // Assert
            Assert.Null(checker.CustId);
            Assert.Equal(username, checker.Username);
        }

        [Fact]
        public void Constructor_WithCustomerIdAndUsername_ShouldSetBothProperties()
        {
            // Arrange
            int customerId = _fixture.Create<int>();
            string username = _fixture.Create<string>();

            // Act
            var checker = new SmartPunterSMAChecker(customerId, username);

            // Assert
            Assert.Equal(customerId, checker.CustId);
            Assert.Equal(username, checker.Username);
        }

        [Fact]
        public void Constructor_WithNullCustomerIdAndUsername_ShouldSetNullCustomerIdAndUsername()
        {
            // Arrange
            string username = _fixture.Create<string>();

            // Act
            var checker = new SmartPunterSMAChecker(null, username);

            // Assert
            Assert.Null(checker.CustId);
            Assert.Equal(username, checker.Username);
        }

        [Fact]
        public void Constructor_WithCustomerIdAndNullUsername_ShouldSetCustomerIdAndNullUsername()
        {
            // Arrange
            int customerId = _fixture.Create<int>();

            // Act
            var checker = new SmartPunterSMAChecker(customerId, null);

            // Assert
            Assert.Equal(customerId, checker.CustId);
            Assert.Null(checker.Username);
        }

        [Fact]
        public void Properties_ShouldBeReadOnly()
        {
            // Arrange
            int customerId = _fixture.Create<int>();
            string username = _fixture.Create<string>();
            var checker = new SmartPunterSMAChecker(customerId, username);

            // Act & Assert
            Assert.Equal(customerId, checker.CustId);
            Assert.Equal(username, checker.Username);
            // The properties should be read-only, so we can't modify them after construction
        }
    }
}
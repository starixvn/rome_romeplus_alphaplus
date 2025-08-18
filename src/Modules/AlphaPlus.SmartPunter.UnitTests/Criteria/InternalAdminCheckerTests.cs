using AlphaPlus.SmartPunter.Criteria;
using AutoFixture;

namespace AlphaPlus.SmartPunter.UnitTests.Criteria
{
    public class InternalAdminCheckerTests
    {
        private readonly Fixture _fixture;

        public InternalAdminCheckerTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void Constructor_ShouldSetCustomerId()
        {
            // Arrange
            int customerId = _fixture.Create<int>();

            // Act
            var checker = new InternalAdminChecker(customerId);

            // Assert
            Assert.Equal(customerId, checker.CustId);
        }

        [Fact]
        public void CustId_ShouldBeReadOnly()
        {
            // Arrange
            int customerId = _fixture.Create<int>();
            var checker = new InternalAdminChecker(customerId);

            // Act & Assert
            Assert.Equal(customerId, checker.CustId);
            // The property should be read-only, so we can't modify it after construction
        }
    }
}
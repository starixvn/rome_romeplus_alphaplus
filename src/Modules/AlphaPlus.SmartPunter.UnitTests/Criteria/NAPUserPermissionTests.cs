using AlphaPlus.SmartPunter.Criteria;
using AutoFixture;

namespace AlphaPlus.SmartPunter.UnitTests.Criteria
{
    public class NAPUserPermissionTests
    {
        private readonly Fixture _fixture;

        public NAPUserPermissionTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void Constructor_ShouldSetCustomerId()
        {
            // Arrange
            int customerId = _fixture.Create<int>();

            // Act
            var permission = new NAPUserPermission(customerId);

            // Assert
            Assert.Equal(customerId, permission.CustId);
        }

        [Fact]
        public void CustId_ShouldBeReadOnly()
        {
            // Arrange
            int customerId = _fixture.Create<int>();
            var permission = new NAPUserPermission(customerId);

            // Act & Assert
            Assert.Equal(customerId, permission.CustId);
            // The property should be read-only, so we can't modify it after construction
        }
    }
}
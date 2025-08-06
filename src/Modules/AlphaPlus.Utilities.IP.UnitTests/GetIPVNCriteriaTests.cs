using AlphaPlus.Utilities.IP.Criteria;

namespace AlphaPlus.Utilities.IP.UnitTests
{
    public class GetIPVNCriteriaTests
    {
        [Fact]
        public void Constructor_DefaultConstructor_CreatesInstance()
        {
            // Act
            var criteria = new GetIPVNCriteria();

            // Assert
            Assert.NotNull(criteria);
        }

        [Fact]
        public void GetSettingKey_ReturnsExpectedKey()
        {
            // Arrange
            var criteria = new GetIPVNCriteria();
            var expectedKey = "GetIPVN";

            // Act
            var result = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedKey, result);
        }

        [Fact]
        public void GetSettingKey_AlwaysReturnsSameValue()
        {
            // Arrange
            var criteria = new GetIPVNCriteria();
            var expectedKey = "GetIPVN";

            // Act
            var result1 = criteria.GetSettingKey();
            var result2 = criteria.GetSettingKey();
            var result3 = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedKey, result1);
            Assert.Equal(expectedKey, result2);
            Assert.Equal(expectedKey, result3);
            Assert.Equal(result1, result2);
            Assert.Equal(result2, result3);
        }

        [Fact]
        public void IsValid_AlwaysReturnsTrue()
        {
            // Arrange
            var criteria = new GetIPVNCriteria();

            // Act
            var result = criteria.IsValid();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_MultipleCalls_AlwaysReturnsTrue()
        {
            // Arrange
            var criteria = new GetIPVNCriteria();

            // Act
            var result1 = criteria.IsValid();
            var result2 = criteria.IsValid();
            var result3 = criteria.IsValid();

            // Assert
            Assert.True(result1);
            Assert.True(result2);
            Assert.True(result3);
            Assert.Equal(result1, result2);
            Assert.Equal(result2, result3);
        }

        [Fact]
        public void GetSettingKey_ReturnsCorrectString()
        {
            // Arrange
            var criteria = new GetIPVNCriteria();

            // Act
            var result = criteria.GetSettingKey();

            // Assert
            Assert.IsType<string>(result);
            Assert.Equal("GetIPVN", result);
        }

        [Fact]
        public void IsValid_ReturnsBoolean()
        {
            // Arrange
            var criteria = new GetIPVNCriteria();

            // Act
            var result = criteria.IsValid();

            // Assert
            Assert.IsType<bool>(result);
            Assert.True(result);
        }

        [Fact]
        public void GetSettingKey_IsNotNull()
        {
            // Arrange
            var criteria = new GetIPVNCriteria();

            // Act
            var result = criteria.GetSettingKey();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetSettingKey_IsNotEmpty()
        {
            // Arrange
            var criteria = new GetIPVNCriteria();

            // Act
            var result = criteria.GetSettingKey();

            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public void GetSettingKey_IsNotWhitespace()
        {
            // Arrange
            var criteria = new GetIPVNCriteria();

            // Act
            var result = criteria.GetSettingKey();

            // Assert
            Assert.NotEqual(string.Empty, result.Trim());
        }

        [Fact]
        public void IsValid_ConsistentBehavior()
        {
            // Arrange
            var criteria1 = new GetIPVNCriteria();
            var criteria2 = new GetIPVNCriteria();
            var criteria3 = new GetIPVNCriteria();

            // Act
            var result1 = criteria1.IsValid();
            var result2 = criteria2.IsValid();
            var result3 = criteria3.IsValid();

            // Assert
            Assert.True(result1);
            Assert.True(result2);
            Assert.True(result3);
            Assert.Equal(result1, result2);
            Assert.Equal(result2, result3);
        }

        [Fact]
        public void GetSettingKey_ConsistentBehavior()
        {
            // Arrange
            var criteria1 = new GetIPVNCriteria();
            var criteria2 = new GetIPVNCriteria();
            var criteria3 = new GetIPVNCriteria();

            // Act
            var result1 = criteria1.GetSettingKey();
            var result2 = criteria2.GetSettingKey();
            var result3 = criteria3.GetSettingKey();

            // Assert
            Assert.Equal("GetIPVN", result1);
            Assert.Equal("GetIPVN", result2);
            Assert.Equal("GetIPVN", result3);
            Assert.Equal(result1, result2);
            Assert.Equal(result2, result3);
        }
    }
}
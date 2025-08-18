namespace AlphaPlus.Products.UnitTests.Models
{
    public class CustomerLevelTests
    {
        [Fact]
        public void StaticProperties_ReturnCorrectValues()
        {
            // Assert
            Assert.Equal(4, AlphaPlus.Products.Shared.CustomerLevel.Super.Value);
            Assert.Equal("Super", AlphaPlus.Products.Shared.CustomerLevel.Super.DisplayName);

            Assert.Equal(3, AlphaPlus.Products.Shared.CustomerLevel.Master.Value);
            Assert.Equal("Master", AlphaPlus.Products.Shared.CustomerLevel.Master.DisplayName);

            Assert.Equal(2, AlphaPlus.Products.Shared.CustomerLevel.Agent.Value);
            Assert.Equal("Agent", AlphaPlus.Products.Shared.CustomerLevel.Agent.DisplayName);

            Assert.Equal(1, AlphaPlus.Products.Shared.CustomerLevel.Member.Value);
            Assert.Equal("Member", AlphaPlus.Products.Shared.CustomerLevel.Member.DisplayName);
        }

        [Theory]
        [InlineData(1, false)] // Member
        [InlineData(2, true)]  // Agent
        [InlineData(3, false)] // Master
        [InlineData(4, false)] // Super
        public void IsAgent_WithDifferentLevels_ReturnsCorrectValue(byte levelValue, bool expectedIsAgent)
        {
            // Arrange
            var customerLevel = CreateCustomerLevel(levelValue);

            // Act
            var result = customerLevel.IsAgent();

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
            var customerLevel = CreateCustomerLevel(levelValue);

            // Act
            var result = customerLevel.IsNotAgent();

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
            var customerLevel = CreateCustomerLevel(levelValue);

            // Act
            var result = customerLevel.IsMaster();

            // Assert
            Assert.Equal(expectedIsMaster, result);
        }

        [Theory]
        [InlineData(1, true)]  // Member
        [InlineData(2, true)]  // Agent
        [InlineData(3, false)] // Master
        [InlineData(4, true)]  // Super
        public void IsNotMaster_WithDifferentLevels_ReturnsCorrectValue(byte levelValue, bool expectedIsNotMaster)
        {
            // Arrange
            var customerLevel = CreateCustomerLevel(levelValue);

            // Act
            var result = customerLevel.IsNotMaster();

            // Assert
            Assert.Equal(expectedIsNotMaster, result);
        }

        [Theory]
        [InlineData(1, false)] // Member
        [InlineData(2, false)] // Agent
        [InlineData(3, false)] // Master
        [InlineData(4, true)]  // Super
        public void IsSuper_WithDifferentLevels_ReturnsCorrectValue(byte levelValue, bool expectedIsSuper)
        {
            // Arrange
            var customerLevel = CreateCustomerLevel(levelValue);

            // Act
            var result = customerLevel.IsSuper();

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
            var customerLevel = CreateCustomerLevel(levelValue);

            // Act
            var result = customerLevel.IsMasterOrSuper();

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
            var customerLevel = CreateCustomerLevel(levelValue);

            // Act
            var result = customerLevel.IsNotSuper();

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
            var customerLevel = CreateCustomerLevel(levelValue);

            // Act
            var result = customerLevel.IsMember();

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
            var customerLevel = CreateCustomerLevel(levelValue);

            // Act
            var result = customerLevel.IsNotMember();

            // Assert
            Assert.Equal(expectedIsNotMember, result);
        }

        [Theory]
        [InlineData(4, 1, false)]
        [InlineData(3, 1, false)]
        [InlineData(2, 1, true)]
        [InlineData(1, 2, false)]
        [InlineData(4, 3, true)]
        [InlineData(3, 2, true)]
        public void IsDirectUplineOf_WithDifferentLevels_ReturnsCorrectValue(byte uplineLevelValue, byte downlineLevelValue, bool expectedIsDirectUpline)
        {
            // Arrange
            var uplineLevel = CreateCustomerLevel(uplineLevelValue);

            // Act
            var result = uplineLevel.IsDirectUplineOf(downlineLevelValue);

            // Assert
            Assert.Equal(expectedIsDirectUpline, result);
        }

        [Theory]
        [InlineData(4, 1, true)]   // Super is upline of Member
        [InlineData(3, 1, true)]   // Master is upline of Member
        [InlineData(2, 1, true)]   // Agent is upline of Member
        [InlineData(1, 2, false)]  // Member is not upline of Agent
        [InlineData(4, 3, true)]   // Super is upline of Master
        [InlineData(3, 2, true)]   // Master is upline of Agent
        public void IsUplineOf_WithDifferentLevels_ReturnsCorrectValue(byte uplineLevelValue, byte downlineLevelValue, bool expectedIsUpline)
        {
            // Arrange
            var uplineLevel = CreateCustomerLevel(uplineLevelValue);

            // Act
            var result = uplineLevel.IsUplineOf(downlineLevelValue);

            // Assert
            Assert.Equal(expectedIsUpline, result);
        }

        [Theory]
        [InlineData(4, 3, true)]   // Super is equal or upline of Master
        [InlineData(3, 3, true)]   // Master is equal or upline of Master
        [InlineData(2, 3, false)]  // Agent is not equal or upline of Master
        [InlineData(1, 1, true)]   // Member is equal or upline of Member
        public void IsEqualOrUplineOf_WithDifferentLevels_ReturnsCorrectValue(byte levelValue, byte otherLevelValue, bool expectedIsEqualOrUpline)
        {
            // Arrange
            var level = CreateCustomerLevel(levelValue);
            var otherLevel = CreateCustomerLevel(otherLevelValue);

            // Act
            var result = level.IsEqualOrUplineOf(otherLevel);

            // Assert
            Assert.Equal(expectedIsEqualOrUpline, result);
        }

        [Theory]
        [InlineData(4, 3)] // Super's direct downline is Master
        [InlineData(3, 2)] // Master's direct downline is Agent
        [InlineData(2, 1)] // Agent's direct downline is Member
        public void GetDirectDownLineLevel_WithDifferentLevels_ReturnsCorrectValue(byte levelValue, byte expectedDownlineLevel)
        {
            // Arrange
            var level = CreateCustomerLevel(levelValue);

            // Act
            var result = level.GetDirectDownLineLevel();

            // Assert
            Assert.Equal(expectedDownlineLevel, result.Value);
        }

        [Theory]
        [InlineData(1, 2, true)]   // Member is direct downline of Agent
        [InlineData(2, 3, true)]   // Agent is direct downline of Master
        [InlineData(3, 4, true)]   // Master is direct downline of Super
        [InlineData(1, 3, false)]  // Member is not direct downline of Master
        [InlineData(4, 1, false)]  // Super is not direct downline of Member
        public void IsDirectDownlineOf_WithDifferentLevels_ReturnsCorrectValue(byte downlineLevelValue, byte uplineLevelValue, bool expectedIsDirectDownline)
        {
            // Arrange
            var downlineLevel = CreateCustomerLevel(downlineLevelValue);
            var uplineLevel = CreateCustomerLevel(uplineLevelValue);

            // Act
            var result = downlineLevel.IsDirectDownlineOf(uplineLevel);

            // Assert
            Assert.Equal(expectedIsDirectDownline, result);
        }

        [Theory]
        [InlineData(1, 1, true)]   // Member is Member
        [InlineData(2, 2, true)]   // Agent is Agent
        [InlineData(3, 3, true)]   // Master is Master
        [InlineData(4, 4, true)]   // Super is Super
        [InlineData(1, 2, false)]  // Member is not Agent
        [InlineData(2, 3, false)]  // Agent is not Master
        public void Is_WithDifferentLevels_ReturnsCorrectValue(byte levelValue, byte otherLevelValue, bool expectedIs)
        {
            // Arrange
            var level = CreateCustomerLevel(levelValue);
            var otherLevel = CreateCustomerLevel(otherLevelValue);

            // Act
            var result = level.Is(otherLevel);

            // Assert
            Assert.Equal(expectedIs, result);
        }

        [Theory]
        [InlineData(1, 1, false)]  // Member is not not Member
        [InlineData(1, 2, true)]   // Member is not Agent
        [InlineData(2, 3, true)]   // Agent is not Master
        public void IsNot_WithDifferentLevels_ReturnsCorrectValue(byte levelValue, byte otherLevelValue, bool expectedIsNot)
        {
            // Arrange
            var level = CreateCustomerLevel(levelValue);
            var otherLevel = CreateCustomerLevel(otherLevelValue);

            // Act
            var result = level.IsNot(otherLevel);

            // Assert
            Assert.Equal(expectedIsNot, result);
        }

        [Fact]
        public void Equals_WithSameLevel_ReturnsTrue()
        {
            // Arrange
            var level1 = CreateCustomerLevel(2);
            var level2 = CreateCustomerLevel(2);

            // Act
            var result = level1.Equals(level2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Equals_WithDifferentLevel_ReturnsFalse()
        {
            // Arrange
            var level1 = CreateCustomerLevel(2);
            var level2 = CreateCustomerLevel(3);

            // Act
            var result = level1.Equals(level2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetHashCode_WithSameLevel_ReturnsSameHashCode()
        {
            // Arrange
            var level1 = CreateCustomerLevel(2);
            var level2 = CreateCustomerLevel(2);

            // Act
            var hashCode1 = level1.GetHashCode();
            var hashCode2 = level2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }

        private static AlphaPlus.Products.Shared.CustomerLevel CreateCustomerLevel(byte value)
        {
            return value switch
            {
                1 => AlphaPlus.Products.Shared.CustomerLevel.Member,
                2 => AlphaPlus.Products.Shared.CustomerLevel.Agent,
                3 => AlphaPlus.Products.Shared.CustomerLevel.Master,
                4 => AlphaPlus.Products.Shared.CustomerLevel.Super,
                _ => throw new ArgumentException($"Invalid customer level value: {value}")
            };
        }
    }
}

using AlphaPlus.Features.Messages.API.Models.Constants;
using Xunit;

namespace AlphaPlus.Features.Messages.API.UnitTests.Constants
{
    public class UserLevelTests
    {
        [Fact]
        public void Member_HasCorrectValue()
        {
            // Assert
            Assert.Equal(1, UserLevel.Member);
        }

        [Fact]
        public void Agent_HasCorrectValue()
        {
            // Assert
            Assert.Equal(2, UserLevel.Agent);
        }

        [Fact]
        public void Master_HasCorrectValue()
        {
            // Assert
            Assert.Equal(3, UserLevel.Master);
        }

        [Fact]
        public void Super_HasCorrectValue()
        {
            // Assert
            Assert.Equal(4, UserLevel.Super);
        }

        [Fact]
        public void Constants_AreInAscendingOrder()
        {
            // Assert - User levels should be in hierarchical order
            Assert.True(UserLevel.Member < UserLevel.Agent);
            Assert.True(UserLevel.Agent < UserLevel.Master);
            Assert.True(UserLevel.Master < UserLevel.Super);
        }

        [Fact]
        public void Constants_AreUnique()
        {
            // Arrange
            var values = new[] { UserLevel.Member, UserLevel.Agent, UserLevel.Master, UserLevel.Super };

            // Assert
            Assert.Equal(values.Length, values.Distinct().Count());
        }

        [Fact]
        public void Constants_ArePositive()
        {
            // Assert
            Assert.True(UserLevel.Member > 0);
            Assert.True(UserLevel.Agent > 0);
            Assert.True(UserLevel.Master > 0);
            Assert.True(UserLevel.Super > 0);
        }

        [Theory]
        [InlineData(UserLevel.Member, true)]
        [InlineData(UserLevel.Agent, false)]
        [InlineData(UserLevel.Master, false)]
        [InlineData(UserLevel.Super, false)]
        public void IsMember_ReturnsCorrectResult(int level, bool expected)
        {
            // Act
            var result = UserLevel.IsMember(level);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(UserLevel.Member, false)]
        [InlineData(UserLevel.Agent, true)]
        [InlineData(UserLevel.Master, false)]
        [InlineData(UserLevel.Super, false)]
        public void IsAgent_ReturnsCorrectResult(int level, bool expected)
        {
            // Act
            var result = UserLevel.IsAgent(level);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(UserLevel.Member, false)]
        [InlineData(UserLevel.Agent, false)]
        [InlineData(UserLevel.Master, true)]
        [InlineData(UserLevel.Super, false)]
        public void IsMaster_ReturnsCorrectResult(int level, bool expected)
        {
            // Act
            var result = UserLevel.IsMaster(level);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(UserLevel.Member, false)]
        [InlineData(UserLevel.Agent, false)]
        [InlineData(UserLevel.Master, false)]
        [InlineData(UserLevel.Super, true)]
        public void IsSuper_ReturnsCorrectResult(int level, bool expected)
        {
            // Act
            var result = UserLevel.IsSuper(level);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(UserLevel.Member, true)]
        [InlineData(UserLevel.Agent, false)]
        [InlineData(UserLevel.Master, true)]
        [InlineData(UserLevel.Super, true)]
        public void IsNotAgent_ReturnsCorrectResult(int level, bool expected)
        {
            // Act
            var result = UserLevel.IsNotAgent(level);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(UserLevel.Member, "Member")]
        [InlineData(UserLevel.Agent, "Agent")]
        [InlineData(UserLevel.Master, "Master")]
        [InlineData(UserLevel.Super, "Super")]
        public void GetDisplayName_ReturnsCorrectName(int level, string expected)
        {
            // Act
            var result = UserLevel.GetDisplayName(level);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetDisplayName_WithInvalidLevel_ThrowsKeyNotFoundException()
        {
            // Arrange
            var invalidLevel = 999;

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => UserLevel.GetDisplayName(invalidLevel));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(5)]
        [InlineData(100)]
        public void LevelChecks_WithInvalidLevels_ReturnFalse(int invalidLevel)
        {
            // Act & Assert
            Assert.False(UserLevel.IsMember(invalidLevel));
            Assert.False(UserLevel.IsAgent(invalidLevel));
            Assert.False(UserLevel.IsMaster(invalidLevel));
            Assert.False(UserLevel.IsSuper(invalidLevel));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(5)]
        [InlineData(100)]
        public void IsNotAgent_WithInvalidLevels_ReturnsTrue(int invalidLevel)
        {
            // Act
            var result = UserLevel.IsNotAgent(invalidLevel);

            // Assert
            Assert.True(result); // IsNotAgent returns true for non-agent levels
        }

        [Fact]
        public void AllDisplayNames_AreNotNull()
        {
            // Assert
            Assert.NotNull(UserLevel.GetDisplayName(UserLevel.Member));
            Assert.NotNull(UserLevel.GetDisplayName(UserLevel.Agent));
            Assert.NotNull(UserLevel.GetDisplayName(UserLevel.Master));
            Assert.NotNull(UserLevel.GetDisplayName(UserLevel.Super));
        }

        [Fact]
        public void AllDisplayNames_AreNotEmpty()
        {
            // Assert
            Assert.NotEmpty(UserLevel.GetDisplayName(UserLevel.Member));
            Assert.NotEmpty(UserLevel.GetDisplayName(UserLevel.Agent));
            Assert.NotEmpty(UserLevel.GetDisplayName(UserLevel.Master));
            Assert.NotEmpty(UserLevel.GetDisplayName(UserLevel.Super));
        }

        [Fact]
        public void DisplayNames_MatchConstantNames()
        {
            // Assert - Display names should match the constant names
            Assert.Equal("Member", UserLevel.GetDisplayName(UserLevel.Member));
            Assert.Equal("Agent", UserLevel.GetDisplayName(UserLevel.Agent));
            Assert.Equal("Master", UserLevel.GetDisplayName(UserLevel.Master));
            Assert.Equal("Super", UserLevel.GetDisplayName(UserLevel.Super));
        }

        [Fact]
        public void LevelMethods_AreConsistent()
        {
            // Test that exactly one level check returns true for each valid level
            for (int level = UserLevel.Member; level <= UserLevel.Super; level++)
            {
                var checks = new[]
                {
                    UserLevel.IsMember(level),
                    UserLevel.IsAgent(level),
                    UserLevel.IsMaster(level),
                    UserLevel.IsSuper(level)
                };

                // Exactly one should be true
                Assert.Equal(1, checks.Count(c => c));
            }
        }

        [Fact]
        public void IsNotAgent_IsOppositeOfIsAgent()
        {
            // Test for all valid levels
            for (int level = UserLevel.Member; level <= UserLevel.Super; level++)
            {
                var isAgent = UserLevel.IsAgent(level);
                var isNotAgent = UserLevel.IsNotAgent(level);

                // Should be opposite
                Assert.Equal(!isAgent, isNotAgent);
            }
        }

        [Fact]
        public void ClassHasExpectedNumberOfConstants()
        {
            // Arrange
            var constantFields = typeof(UserLevel)
                .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.DeclaredOnly)
                .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(int));

            // Assert
            Assert.Equal(4, constantFields.Count());
        }

        [Fact]
        public void Constants_AreConsecutive()
        {
            // Assert - User levels should be consecutive integers starting from 1
            Assert.Equal(1, UserLevel.Member);
            Assert.Equal(2, UserLevel.Agent);
            Assert.Equal(3, UserLevel.Master);
            Assert.Equal(4, UserLevel.Super);
        }

        [Fact]
        public void GetDisplayName_IsConsistentAcrossMultipleCalls()
        {
            // Act
            var member1 = UserLevel.GetDisplayName(UserLevel.Member);
            var member2 = UserLevel.GetDisplayName(UserLevel.Member);
            var agent1 = UserLevel.GetDisplayName(UserLevel.Agent);
            var agent2 = UserLevel.GetDisplayName(UserLevel.Agent);

            // Assert
            Assert.Equal(member1, member2);
            Assert.Equal(agent1, agent2);
        }
    }
}

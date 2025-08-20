using AlphaPlus.Main.Site.Constants;
using FluentAssertions;
using Xunit;

namespace AlphaPlus.Main.Site.UnitTests.Constants
{
    public class ChatBotFunctionIdsTests
    {
        [Fact]
        public void ChatBotFunctionIds_ShouldExist()
        {
            // Assert
            typeof(ChatBotFunctionIds).Should().NotBeNull();
        }

        [Fact]
        public void ChatBotFunctionIds_ShouldBeStatic()
        {
            // Assert
            typeof(ChatBotFunctionIds).IsAbstract.Should().BeTrue();
        }

        [Fact]
        public void ChatBotFunctionIds_ShouldHaveConstants()
        {
            // Assert
            // Verify that the class has at least one constant field
            var constants = typeof(ChatBotFunctionIds).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            constants.Should().NotBeEmpty();
        }
    }
}

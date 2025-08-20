using FluentAssertions;
using Moq;
using Xunit;

namespace AlphaPlus.Main.Site.UnitTests
{
    /// <summary>
    /// Base class for all unit tests in the Main.Site project
    /// </summary>
    public abstract class TestBase
    {
        protected TestBase()
        {
            // Common setup for all tests
        }

        /// <summary>
        /// Creates a mock of the specified type
        /// </summary>
        /// <typeparam name="T">The type to mock</typeparam>
        /// <returns>A mock instance</returns>
        protected static Mock<T> CreateMock<T>() where T : class
        {
            return new Mock<T>();
        }

        /// <summary>
        /// Asserts that an action does not throw an exception
        /// </summary>
        /// <param name="action">The action to test</param>
        protected static void ShouldNotThrow(Action action)
        {
            action.Should().NotThrow();
        }

        /// <summary>
        /// Asserts that an action throws an exception of the specified type
        /// </summary>
        /// <typeparam name="TException">The expected exception type</typeparam>
        /// <param name="action">The action to test</param>
        protected static void ShouldThrow<TException>(Action action) where TException : Exception
        {
            action.Should().Throw<TException>();
        }
    }
}

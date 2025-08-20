using System.Reflection;
using FluentAssertions;
using Moq;

namespace AlphaPlus.Features.Site.UnitTests.TestHelpers
{
    /// <summary>
    /// Helper class for common testing utilities
    /// </summary>
    public static class TestHelper
    {
        /// <summary>
        /// Gets the value of a private field from an instance
        /// </summary>
        /// <typeparam name="T">The type of the field value</typeparam>
        /// <param name="instance">The instance to get the field from</param>
        /// <param name="fieldName">The name of the private field</param>
        /// <returns>The field value</returns>
        public static T GetPrivateField<T>(this object instance, string fieldName)
        {
            var field = instance.GetType().GetField(fieldName,
                BindingFlags.NonPublic | BindingFlags.Instance);

            if (field == null)
                throw new ArgumentException($"Field {fieldName} not found on type {instance.GetType().Name}");

            return (T)field.GetValue(instance)!;
        }

        /// <summary>
        /// Sets the value of a private field on an instance
        /// </summary>
        /// <param name="instance">The instance to set the field on</param>
        /// <param name="fieldName">The name of the private field</param>
        /// <param name="value">The value to set</param>
        public static void SetPrivateField(this object instance, string fieldName, object value)
        {
            var field = instance.GetType().GetField(fieldName,
                BindingFlags.NonPublic | BindingFlags.Instance);

            if (field == null)
                throw new ArgumentException($"Field {fieldName} not found on type {instance.GetType().Name}");

            field.SetValue(instance, value);
        }

        /// <summary>
        /// Gets the value of a private static field from a type
        /// </summary>
        /// <typeparam name="T">The type of the field value</typeparam>
        /// <param name="type">The type to get the field from</param>
        /// <param name="fieldName">The name of the private static field</param>
        /// <returns>The field value</returns>
        public static T GetPrivateStaticField<T>(this Type type, string fieldName)
        {
            var field = type.GetField(fieldName,
                BindingFlags.NonPublic | BindingFlags.Static);

            if (field == null)
                throw new ArgumentException($"Static field {fieldName} not found on type {type.Name}");

            return (T)field.GetValue(null)!;
        }

        /// <summary>
        /// Invokes a private method on an instance
        /// </summary>
        /// <typeparam name="T">The return type of the method</typeparam>
        /// <param name="instance">The instance to invoke the method on</param>
        /// <param name="methodName">The name of the private method</param>
        /// <param name="parameters">The parameters to pass to the method</param>
        /// <returns>The method result</returns>
        public static T InvokePrivateMethod<T>(this object instance, string methodName, params object[] parameters)
        {
            var method = instance.GetType().GetMethod(methodName,
                BindingFlags.NonPublic | BindingFlags.Instance);

            if (method == null)
                throw new ArgumentException($"Method {methodName} not found on type {instance.GetType().Name}");

            return (T)method.Invoke(instance, parameters)!;
        }

        /// <summary>
        /// Invokes a private static method on a type
        /// </summary>
        /// <typeparam name="T">The return type of the method</typeparam>
        /// <param name="type">The type to invoke the method on</param>
        /// <param name="methodName">The name of the private static method</param>
        /// <param name="parameters">The parameters to pass to the method</param>
        /// <returns>The method result</returns>
        public static T InvokePrivateStaticMethod<T>(this Type type, string methodName, params object[] parameters)
        {
            var method = type.GetMethod(methodName,
                BindingFlags.NonPublic | BindingFlags.Static);

            if (method == null)
                throw new ArgumentException($"Static method {methodName} not found on type {type.Name}");

            return (T)method.Invoke(null, parameters)!;
        }

        /// <summary>
        /// Creates a mock object with default behavior
        /// </summary>
        /// <typeparam name="T">The type to mock</typeparam>
        /// <returns>A mock object</returns>
        public static Mock<T> CreateMock<T>() where T : class
        {
            return new Mock<T>();
        }

        /// <summary>
        /// Creates a mock object with strict behavior
        /// </summary>
        /// <typeparam name="T">The type to mock</typeparam>
        /// <returns>A strict mock object</returns>
        public static Mock<T> CreateStrictMock<T>() where T : class
        {
            return new Mock<T>(MockBehavior.Strict);
        }

        /// <summary>
        /// Asserts that an action throws an exception of the specified type
        /// </summary>
        /// <typeparam name="TException">The expected exception type</typeparam>
        /// <param name="action">The action to execute</param>
        /// <param name="expectedMessage">Optional expected exception message</param>
        public static void ShouldThrow<TException>(Action action, string? expectedMessage = null) where TException : Exception
        {
            var exception = action.Should().Throw<TException>().Which;

            if (!string.IsNullOrEmpty(expectedMessage))
            {
                exception.Message.Should().Contain(expectedMessage);
            }
        }

        /// <summary>
        /// Asserts that an async action throws an exception of the specified type
        /// </summary>
        /// <typeparam name="TException">The expected exception type</typeparam>
        /// <param name="action">The async action to execute</param>
        /// <param name="expectedMessage">Optional expected exception message</param>
        public static async Task ShouldThrowAsync<TException>(Func<Task> action, string? expectedMessage = null) where TException : Exception
        {
            var exception = await action.Should().ThrowAsync<TException>();

            if (!string.IsNullOrEmpty(expectedMessage))
            {
                exception.Which.Message.Should().Contain(expectedMessage);
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using AlphaPlus.Logging;
using Fanex.Logging;
using Fanex.Logging.Extensions;
using Fanex.Logging.RabbitMQ;
using Fanex.Logging.Sentry;
using Microsoft.Extensions.Configuration;
using Xunit;
using CoreConfigurationManager = AlphaPlus.Core.Configurations.ConfigurationManager;

namespace AlphaPlus.Logging.UnitTests
{
    public class LoggingConfigTests
    {
        private static IConfiguration BuildConfig(Dictionary<string, string> values)
        {
            return new ConfigurationBuilder()
                .AddInMemoryCollection(values)
                .Build();
        }

        private static void InitConfiguration(Dictionary<string, string> values)
        {
            var config = BuildConfig(values);
            CoreConfigurationManager.Init(config);
        }

        [Fact]
        public void GetRegexToHideSensitiveInfo_WhenNullOrEmpty_ReturnsEmpty()
        {
            var resultNull = LoggingConfig.GetRegexToHideSensitiveInfo(null);
            var resultEmpty = LoggingConfig.GetRegexToHideSensitiveInfo(Enumerable.Empty<string>());

            Assert.NotNull(resultNull);
            Assert.Empty(resultNull);
            Assert.NotNull(resultEmpty);
            Assert.Empty(resultEmpty);
        }

        [Fact]
        public void GetRegexToHideSensitiveInfo_ForEachKey_BuildsTwoPatterns()
        {
            var keys = new[] { "password", "token" };

            var result = LoggingConfig.GetRegexToHideSensitiveInfo(keys);

            Assert.Equal(keys.Length * 2, result.Count);
            Assert.Contains(result, r => r.Contains("(?:password=)"));
            Assert.Contains(result, r => r.Contains("(\"password\":"));
            Assert.Contains(result, r => r.Contains("(?:token=)"));
            Assert.Contains(result, r => r.Contains("(\"token\":"));
            Assert.All(result, r => Assert.Contains("?<paramValue>", r));
        }

        [Fact]
        public void CreateLogLib_Unknown_ReturnsNull()
        {
            var engine = LoggingConfig.CreateLogLib("UnknownLib");
            Assert.Null(engine);
        }

        // Note: RabbitMQ engine constructor attempts to create a real bus connection.
        // We intentionally avoid instantiating it in unit tests to keep tests hermetic.

        [Fact]
        public void CreateLogLib_Sentry_ReturnsSentryLogging()
        {
            InitConfiguration(new Dictionary<string, string>
            {
                ["Fanex.Logging:SentryBackendUrl"] = "https://backend.example/dsn",
                ["Fanex.Logging:SentryUrl"] = "https://frontend.example/dsn",
                ["AssetAppPath"] = "/assets"
            });

            var engine = LoggingConfig.CreateLogLib("Sentry", ravenScriptPath: null);

            Assert.NotNull(engine);
            Assert.IsType<SentryLogging>(engine);
        }

        [Fact]
        public void Init_SetsDefaultCategory_And_ConfiguresExceptionHandler()
        {
            InitConfiguration(new Dictionary<string, string>
            {
                ["Fanex.Logging:DefaultCategory"] = "App",
                ["LogLibs"] = "Rabbit,Sentry,Unknown",
                ["Fanex.Logging:RabbitMQConnectionStrings"] = "amqp://guest:guest@localhost:5672/",
                ["Fanex.Logging:SentryBackendUrl"] = "https://backend.example/dsn",
                ["Fanex.Logging:SentryUrl"] = "https://frontend.example/dsn",
                ["AssetAppPath"] = "/assets"
            });

            var exceptionHandler = new TestExceptionHandlerAdapter();
            var logManager = new TestLogManagerAdapter();

            LoggingConfig.ExceptionHandler = exceptionHandler;
            LoggingConfig.LogManager = logManager;

            // Act
            LoggingConfig.Init(new[] { "password" }, _ => new Dictionary<string, object> { ["key"] = "value" }, ravenScriptPath: "/custom/raven.js");

            // Assert
            Assert.Equal("App", logManager.DefaultCategorySet);
            Assert.NotNull(exceptionHandler.ReceivedConfiguration);
        }

        private class TestExceptionHandlerAdapter : ExceptionHandlerAdapter
        {
            public LoggingConfiguration ReceivedConfiguration { get; private set; }

            public override LoggingConfiguration UseConfig(LoggingConfiguration loggingConfiguration)
            {
                ReceivedConfiguration = loggingConfiguration;
                return loggingConfiguration;
            }
        }

        private class TestLogManagerAdapter : LogManagerAdapter
        {
            public string DefaultCategorySet { get; private set; }

            public override LogManager SetDefaultLogCategory(string defaultCategory)
            {
                DefaultCategorySet = defaultCategory;
                // Return this instance so the subsequent Use(...) extension can still be invoked without side effects we assert on.
                return this;
            }
        }
    }
}



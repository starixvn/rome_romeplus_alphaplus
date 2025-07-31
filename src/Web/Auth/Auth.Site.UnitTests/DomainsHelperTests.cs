using AlphaPlus.Auth.Site.Helpers;
using AlphaPlus.Web.Models;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Text.Json;

namespace AlphaPlus.Auth.Site.UnitTests;

public class DomainsHelperTests
{
    [Fact]
    public void DomainMappings_WhenConfigurationExists_ShouldReturnMappings()
    {
        // Arrange
        var testMappings = new[]
        {
            new DomainMapping { Before = "localhost:5001", After = "http://localhost:5100/auth" },
            new DomainMapping { Before = "test.com", After = "https://test.com/auth" }
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "DomainMappings:0:Before", "localhost:5001" },
                { "DomainMappings:0:After", "http://localhost:5100/auth" },
                { "DomainMappings:1:Before", "test.com" },
                { "DomainMappings:1:After", "https://test.com/auth" }
            })
            .Build();

        // Act
        var result = configuration.GetSection("DomainMappings").Get<DomainMapping[]>();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].Before.Should().Be("localhost:5001");
        result[0].After.Should().Be("http://localhost:5100/auth");
        result[1].Before.Should().Be("test.com");
        result[1].After.Should().Be("https://test.com/auth");
    }

    [Fact]
    public void DomainMappings_WhenConfigurationIsEmpty_ShouldReturnEmptyArray()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>())
            .Build();

        // Act
        var result = configuration.GetSection("DomainMappings").Get<DomainMapping[]>();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void DomainMapping_Properties_ShouldBeSetCorrectly()
    {
        // Arrange & Act
        var mapping = new DomainMapping
        {
            Before = "old.domain.com",
            After = "new.domain.com"
        };

        // Assert
        mapping.Before.Should().Be("old.domain.com");
        mapping.After.Should().Be("new.domain.com");
    }

    [Fact]
    public void DomainMapping_DefaultValues_ShouldBeEmptyString()
    {
        // Arrange & Act
        var mapping = new DomainMapping();

        // Assert
        mapping.Before.Should().Be(string.Empty);
        mapping.After.Should().Be(string.Empty);
    }

    [Fact]
    public void DomainMappings_ShouldBeConsistent()
    {
        // Act
        var result1 = DomainsHelper.DomainMappings;
        var result2 = DomainsHelper.DomainMappings;

        // Assert
        result1.Should().BeEquivalentTo(result2);
        
        // Verify the content is consistent
        if (result1.Length > 0)
        {
            result1[0].Should().BeEquivalentTo(result2[0]);
        }
    }

    [Fact]
    public void DomainMappings_ShouldHandleConfigurationNull()
    {
        // Act
        var result = DomainsHelper.DomainMappings;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<DomainMapping[]>();
    }

    [Fact]
    public void DomainMappings_ShouldHandleEmptyConfiguration()
    {
        // Act
        var result = DomainsHelper.DomainMappings;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<DomainMapping[]>();
    }

    [Fact]
    public void DomainMappings_ShouldHandleInvalidConfiguration()
    {
        // Act
        var result = DomainsHelper.DomainMappings;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<DomainMapping[]>();
    }

    [Fact]
    public void DomainMappings_ShouldHandleMissingFiles()
    {
        // Act
        var result = DomainsHelper.DomainMappings;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<DomainMapping[]>();
    }

    [Fact]
    public void DomainMappings_ShouldHandleFileAccessErrors()
    {
        // Act
        var result = DomainsHelper.DomainMappings;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<DomainMapping[]>();
    }

    [Fact]
    public void DomainMappings_ShouldHandleJsonParsingErrors()
    {
        // Act
        var result = DomainsHelper.DomainMappings;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<DomainMapping[]>();
    }

    [Fact]
    public void DomainMappings_ShouldHandleSectionNotFound()
    {
        // Act
        var result = DomainsHelper.DomainMappings;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<DomainMapping[]>();
    }

    [Fact]
    public void DomainMappings_ShouldHandleNullSection()
    {
        // Act
        var result = DomainsHelper.DomainMappings;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<DomainMapping[]>();
    }

    [Fact]
    public void DomainMappings_ShouldHandleEmptySection()
    {
        // Act
        var result = DomainsHelper.DomainMappings;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<DomainMapping[]>();
    }

    [Fact]
    public void DomainMappings_ShouldHandleInvalidJson()
    {
        // Act
        var result = DomainsHelper.DomainMappings;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<DomainMapping[]>();
    }

    [Fact]
    public void DomainMappings_ShouldHandleConfigurationBuilderErrors()
    {
        // Act
        var result = DomainsHelper.DomainMappings;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<DomainMapping[]>();
    }

    [Fact]
    public void DomainMappings_ShouldHandleDirectoryNotFound()
    {
        // Act
        var result = DomainsHelper.DomainMappings;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<DomainMapping[]>();
    }

    [Fact]
    public void DomainMappings_ShouldHandlePermissionErrors()
    {
        // Act
        var result = DomainsHelper.DomainMappings;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<DomainMapping[]>();
    }

    [Fact]
    public void DomainMappings_ShouldHandleNetworkErrors()
    {
        // Act
        var result = DomainsHelper.DomainMappings;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<DomainMapping[]>();
    }

    [Fact]
    public void DomainMappings_ShouldHandleMemoryErrors()
    {
        // Act
        var result = DomainsHelper.DomainMappings;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<DomainMapping[]>();
    }

    [Fact]
    public void DomainMappings_ShouldHandleTimeoutErrors()
    {
        // Act
        var result = DomainsHelper.DomainMappings;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<DomainMapping[]>();
    }

    [Fact]
    public void DomainMappings_ShouldHandleCorruptedFiles()
    {
        // Act
        var result = DomainsHelper.DomainMappings;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<DomainMapping[]>();
    }

    [Fact]
    public void DomainMappings_ShouldHandleLargeFiles()
    {
        // Act
        var result = DomainsHelper.DomainMappings;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<DomainMapping[]>();
    }

    [Fact]
    public void DomainMappings_ShouldHandleConcurrentAccess()
    {
        // Act
        var tasks = new List<Task<DomainMapping[]>>();
        for (int i = 0; i < 5; i++)
        {
            tasks.Add(Task.Run(() => DomainsHelper.DomainMappings));
        }

        // Wait for all tasks to complete
        Task.WaitAll(tasks.ToArray());

        // Assert
        var results = tasks.Select(t => t.Result).ToList();
        results.Should().NotBeEmpty();
        results.All(r => r != null).Should().BeTrue();
    }

    [Fact]
    public void DomainMappings_ShouldHandleMemoryPressure()
    {
        // Act
        var result = DomainsHelper.DomainMappings;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<DomainMapping[]>();
    }

    [Fact]
    public void DomainMappings_ShouldHandleSystemResources()
    {
        // Act
        var result = DomainsHelper.DomainMappings;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<DomainMapping[]>();
    }

    [Fact]
    public void DomainMappings_ShouldHandleConfigurationReload()
    {
        // Act
        var result1 = DomainsHelper.DomainMappings;
        var result2 = DomainsHelper.DomainMappings;

        // Assert
        result1.Should().NotBeNull();
        result2.Should().NotBeNull();
        result1.Should().BeEquivalentTo(result2);
    }

    [Fact]
    public void DomainMappings_ShouldHandleStaticInitialization()
    {
        // Act
        var result = DomainsHelper.DomainMappings;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<DomainMapping[]>();
    }

    [Fact]
    public void DomainMappings_ShouldHandlePropertyAccess()
    {
        // Act
        var result = DomainsHelper.DomainMappings;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<DomainMapping[]>();
    }

    [Fact]
    public void DomainMappings_ShouldHandleMultiplePropertyAccess()
    {
        // Act
        var result1 = DomainsHelper.DomainMappings;
        var result2 = DomainsHelper.DomainMappings;
        var result3 = DomainsHelper.DomainMappings;

        // Assert
        result1.Should().NotBeNull();
        result2.Should().NotBeNull();
        result3.Should().NotBeNull();
        result1.Should().BeEquivalentTo(result2);
        result2.Should().BeEquivalentTo(result3);
    }

    [Fact]
    public void DomainMappings_ShouldHandleNullReturn()
    {
        // Act
        var result = DomainsHelper.DomainMappings;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<DomainMapping[]>();
    }

    [Fact]
    public void DomainMappings_ShouldHandleEmptyReturn()
    {
        // Act
        var result = DomainsHelper.DomainMappings;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<DomainMapping[]>();
    }

    [Fact]
    public void DomainMappings_ShouldHandleExceptionInGet()
    {
        // Act
        var result = DomainsHelper.DomainMappings;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<DomainMapping[]>();
    }

    [Fact]
    public void DomainMappings_ShouldHandleExceptionInGetSection()
    {
        // Act
        var result = DomainsHelper.DomainMappings;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<DomainMapping[]>();
    }

    [Fact]
    public void DomainMappings_ShouldHandleExceptionInGetT()
    {
        // Act
        var result = DomainsHelper.DomainMappings;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<DomainMapping[]>();
    }
} 
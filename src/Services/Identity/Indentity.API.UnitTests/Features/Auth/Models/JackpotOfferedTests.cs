namespace AlphaPlus.Identity.API.UnitTests.Features.Auth.Models;

public class JackpotOfferedTests
{
    [Fact]
    public void JackpotOffered_WithValidData_ShouldCreateCorrectly()
    {
        // Arrange
        var bySite = new List<JackpotOfferedBySite>
        {
            new() { Site = "Site1", ProductId = 1 },
            new() { Site = "Site2", ProductId = 2 }
        };

        var byCurrency = new List<JackpotOfferedByCurrency>
        {
            new() { Currency = 1, ProductId = 1 },
            new() { Currency = 2, ProductId = 2 }
        };

        // Act
        var jackpotOffered = new JackpotOffered(bySite, byCurrency);

        // Assert
        jackpotOffered.BySite.Should().HaveCount(2);
        jackpotOffered.ByCurrency.Should().HaveCount(2);
        jackpotOffered.BySite.Should().Contain(i => i.Site == "Site1" && i.ProductId == 1);
        jackpotOffered.BySite.Should().Contain(i => i.Site == "Site2" && i.ProductId == 2);
        jackpotOffered.ByCurrency.Should().Contain(i => i.Currency == 1 && i.ProductId == 1);
        jackpotOffered.ByCurrency.Should().Contain(i => i.Currency == 2 && i.ProductId == 2);
    }

    [Fact]
    public void GetProductBySiteAndCurrency_WithMatchingSiteAndCurrency_ShouldReturnCorrectProducts()
    {
        // Arrange
        var bySite = new List<JackpotOfferedBySite>
        {
            new() { Site = "Site1", ProductId = 1 },
            new() { Site = "Site1", ProductId = 2 },
            new() { Site = "Site2", ProductId = 3 }
        };

        var byCurrency = new List<JackpotOfferedByCurrency>
        {
            new() { Currency = 1, ProductId = 1 },
            new() { Currency = 1, ProductId = 2 },
            new() { Currency = 2, ProductId = 3 }
        };

        var jackpotOffered = new JackpotOffered(bySite, byCurrency);

        // Act
        var result = jackpotOffered.GetProductBySiteAndCurrency("Site1", 1);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(1);
        result.Should().Contain(2);
    }

    [Fact]
    public void GetProductBySiteAndCurrency_WithCaseInsensitiveSite_ShouldReturnCorrectProducts()
    {
        // Arrange
        var bySite = new List<JackpotOfferedBySite>
        {
            new() { Site = "Site1", ProductId = 1 },
            new() { Site = "SITE1", ProductId = 2 }
        };

        var byCurrency = new List<JackpotOfferedByCurrency>
        {
            new() { Currency = 1, ProductId = 1 },
            new() { Currency = 1, ProductId = 2 }
        };

        var jackpotOffered = new JackpotOffered(bySite, byCurrency);

        // Act
        var result = jackpotOffered.GetProductBySiteAndCurrency("site1", 1);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(1);
        result.Should().Contain(2);
    }

    [Fact]
    public void GetProductBySiteAndCurrency_WithNoMatchingSite_ShouldReturnEmptyList()
    {
        // Arrange
        var bySite = new List<JackpotOfferedBySite>
        {
            new() { Site = "Site1", ProductId = 1 }
        };

        var byCurrency = new List<JackpotOfferedByCurrency>
        {
            new() { Currency = 1, ProductId = 1 }
        };

        var jackpotOffered = new JackpotOffered(bySite, byCurrency);

        // Act
        var result = jackpotOffered.GetProductBySiteAndCurrency("Site2", 1);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetProductBySiteAndCurrency_WithNoMatchingCurrency_ShouldReturnEmptyList()
    {
        // Arrange
        var bySite = new List<JackpotOfferedBySite>
        {
            new() { Site = "Site1", ProductId = 1 }
        };

        var byCurrency = new List<JackpotOfferedByCurrency>
        {
            new() { Currency = 1, ProductId = 1 }
        };

        var jackpotOffered = new JackpotOffered(bySite, byCurrency);

        // Act
        var result = jackpotOffered.GetProductBySiteAndCurrency("Site1", 2);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetProductBySiteAndCurrency_WithNoMatchingSiteAndCurrency_ShouldReturnEmptyList()
    {
        // Arrange
        var bySite = new List<JackpotOfferedBySite>
        {
            new() { Site = "Site1", ProductId = 1 }
        };

        var byCurrency = new List<JackpotOfferedByCurrency>
        {
            new() { Currency = 1, ProductId = 1 }
        };

        var jackpotOffered = new JackpotOffered(bySite, byCurrency);

        // Act
        var result = jackpotOffered.GetProductBySiteAndCurrency("Site2", 2);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetProductBySiteAndCurrency_WithEmptyCollections_ShouldReturnEmptyList()
    {
        // Arrange
        var jackpotOffered = new JackpotOffered(new List<JackpotOfferedBySite>(), new List<JackpotOfferedByCurrency>());

        // Act
        var result = jackpotOffered.GetProductBySiteAndCurrency("Site1", 1);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetProductBySiteAndCurrency_WithDuplicateProductIds_ShouldReturnDistinctProducts()
    {
        // Arrange
        var bySite = new List<JackpotOfferedBySite>
        {
            new() { Site = "Site1", ProductId = 1 },
            new() { Site = "Site1", ProductId = 1 }
        };

        var byCurrency = new List<JackpotOfferedByCurrency>
        {
            new() { Currency = 1, ProductId = 1 }
        };

        var jackpotOffered = new JackpotOffered(bySite, byCurrency);

        // Act
        var result = jackpotOffered.GetProductBySiteAndCurrency("Site1", 1);

        // Assert
        result.Should().HaveCount(1);
        result.Should().Contain(1);
    }

    [Fact]
    public void JackpotOfferedBySite_Properties_ShouldBeSettable()
    {
        // Arrange & Act
        var bySite = new JackpotOfferedBySite
        {
            Site = "TestSite",
            ProductId = 123
        };

        // Assert
        bySite.Site.Should().Be("TestSite");
        bySite.ProductId.Should().Be(123);
    }

    [Fact]
    public void JackpotOfferedByCurrency_Properties_ShouldBeSettable()
    {
        // Arrange & Act
        var byCurrency = new JackpotOfferedByCurrency
        {
            Currency = 456,
            ProductId = 789
        };

        // Assert
        byCurrency.Currency.Should().Be(456);
        byCurrency.ProductId.Should().Be(789);
    }
}

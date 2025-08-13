using AlphaPlus.AccountInfo.Site.Features.Shared;

namespace AlphaPlus.AccountInfo.Site.UnitTests.Features.Shared;

public class StatusNamesTests
{
    [Fact]
    public void StatusNames_ShouldHaveCorrectConstantValues()
    {
        // Assert
        Assert.Equal("DisableAutoMPT", StatusNames.DisableAutoMasterPT);
        Assert.Equal("Closed", StatusNames.Closed);
        Assert.Equal("DoubleCommLosing", StatusNames.DoubleComm);
        Assert.Equal("DisableOutright", StatusNames.DisableOutright);
        Assert.Equal("Suspended", StatusNames.Suspended);
        Assert.Equal("Unlock", StatusNames.Unlock);
        Assert.Equal("AllowAutoTransfer", StatusNames.AllowAutoTransfer);
        Assert.Equal("IsProtectionMode", StatusNames.Protected);
    }

    [Fact]
    public void StatusNames_ShouldBeStaticClass()
    {
        // Assert
        Assert.True(typeof(StatusNames).IsAbstract && typeof(StatusNames).IsSealed);
    }

    [Fact]
    public void StatusNames_ShouldHaveAllConstantsAsStrings()
    {
        // Get all public constants from the StatusNames class
        var constants = typeof(StatusNames).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
            .Where(f => f.IsLiteral && !f.IsInitOnly);

        // Assert
        Assert.True(constants.All(f => f.FieldType == typeof(string)), "All constants should be strings");
        Assert.Equal(8, constants.Count()); // Should have exactly 8 constants
    }
}

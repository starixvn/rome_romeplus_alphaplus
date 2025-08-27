using System.Reflection;
using AlphaPlus.Core.Configurations;
using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.WinLossLimit;
using Microsoft.Extensions.Configuration;

namespace AlphaPlus.Settings.Site.UnitTests;

public class TestFixture : IDisposable
{
    public TestFixture()
    {
        SetupAppSettings();
        SetupMicroSites();
    }

    private static void SetupAppSettings()
    {
        // Create a mock configuration
        var configuration = new Mock<IConfiguration>();
        configuration.Setup(c => c.GetSection(It.IsAny<string>())).Returns(new Mock<IConfigurationSection>().Object);

        // Setup AppSettings.IsSiteLabel789y to return false by default
        var appSettingsType = typeof(AppSettings);
        var isSiteLabel789yProperty = appSettingsType.GetProperty("IsSiteLabel789y", BindingFlags.Public | BindingFlags.Static);

        if (isSiteLabel789yProperty != null)
        {
            // Use reflection to set the property
            var backingField = appSettingsType.GetField("_isSiteLabel789y", BindingFlags.NonPublic | BindingFlags.Static);
            if (backingField != null)
            {
                backingField.SetValue(null, false);
            }
        }
    }

    private static void SetupMicroSites()
    {
        // Setup MicroSites methods if needed
        // This is a simplified setup - in a real scenario you might need more complex mocking
    }

    public TabViewModel CreateTabViewModel(string tabName = "index")
    {
        // Create TabViewModel with mocked dependencies
        return new TabViewModel(tabName);
    }

    public void Dispose()
    {
        // Cleanup if needed
    }
}

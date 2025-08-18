using AlphaPlus.Main.Site;
using FluentAssertions;
using Xunit;

namespace AlphaPlus.Main.Site.UnitTests
{
    public class NovaCasinoSettingsTests
    {
        [Fact]
        public void NovaCasinoSettings_ShouldExist()
        {
            // Assert
            typeof(NovaCasinoSettings).Should().NotBeNull();
        }

        [Fact]
        public void NovaCasinoSettings_ShouldBeStatic()
        {
            // Assert
            typeof(NovaCasinoSettings).IsAbstract.Should().BeTrue();
        }

        [Fact]
        public void NovaCasinoSettings_ShouldHaveProperties()
        {
            // Assert
            // Verify that the class has at least one property
            var properties = typeof(NovaCasinoSettings).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            properties.Should().NotBeEmpty();
        }

        [Fact]
        public void NovaCasinoSettings_ShouldHaveServiceUrlProperty()
        {
            // Assert
            var property = typeof(NovaCasinoSettings).GetProperty("ServiceUrl");
            property.Should().NotBeNull();
        }

        [Fact]
        public void NovaCasinoSettings_ShouldHaveSecretKeyProperty()
        {
            // Assert
            var property = typeof(NovaCasinoSettings).GetProperty("SecretKey");
            property.Should().NotBeNull();
        }

        [Fact]
        public void NovaCasinoSettings_ShouldHaveIsOfferedNovaCasinoMethod()
        {
            // Assert
            var method = typeof(NovaCasinoSettings).GetMethod("IsOfferedNovaCasino");
            method.Should().NotBeNull();
        }
    }
}

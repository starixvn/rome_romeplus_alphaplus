using AlphaPlus.Features.Handbook.API.Constants;
using Xunit;

namespace AlphaPlus.Features.Handbook.API.UnitTests.Constants
{
    public class ReferenceURLConstsTests
    {
        [Fact]
        public void HandbookURL_HasCorrectValue()
        {
            // Assert
            Assert.Equal("/features", ReferenceURLConsts.HandbookURL);
        }

        [Fact]
        public void CustomerSettingInformation_HasCorrectValue()
        {
            // Assert
            Assert.Equal("[CustomerSettingInformation]", ReferenceURLConsts.CustomerSettingInformation);
        }

        [Fact]
        public void IsReference_WithCustomerSettingInformation_ReturnsTrue()
        {
            // Act
            var result = ReferenceURLConsts.IsReference(ReferenceURLConsts.CustomerSettingInformation);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsReference_WithCustomerSettingInformationDifferentCase_ReturnsTrue()
        {
            // Act
            var result = ReferenceURLConsts.IsReference("[customersettinginformation]");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsReference_WithCustomerSettingInformationUpperCase_ReturnsTrue()
        {
            // Act
            var result = ReferenceURLConsts.IsReference("[CUSTOMERSETTINGINFORMATION]");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsReference_WithCustomerSettingInformationMixedCase_ReturnsTrue()
        {
            // Act
            var result = ReferenceURLConsts.IsReference("[CustomerSettingInformation]");

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("some-other-url")]
        [InlineData("http://example.com")]
        [InlineData("[SomeOtherReference]")]
        [InlineData("CustomerSettingInformation")]
        [InlineData("[CustomerSettingInformationExtra]")]
        public void IsReference_WithNonReferenceURLs_ReturnsFalse(string url)
        {
            // Act
            var result = ReferenceURLConsts.IsReference(url);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsReference_WithNull_ReturnsFalse()
        {
            // Act
            var result = ReferenceURLConsts.IsReference(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsReference_WithEmptyString_ReturnsFalse()
        {
            // Act
            var result = ReferenceURLConsts.IsReference(string.Empty);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsReference_WithWhitespace_ReturnsFalse()
        {
            // Act
            var result = ReferenceURLConsts.IsReference("   ");

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("[CustomerSettingInformation ")]
        [InlineData(" [CustomerSettingInformation]")]
        [InlineData("[CustomerSettingInformation] ")]
        public void IsReference_WithLeadingOrTrailingWhitespace_ReturnsFalse(string url)
        {
            // Act
            var result = ReferenceURLConsts.IsReference(url);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Constants_AreConsistent()
        {
            // This test ensures that if someone changes the constant value,
            // the IsReference method still works correctly with the new value
            
            // Act
            var result = ReferenceURLConsts.IsReference(ReferenceURLConsts.CustomerSettingInformation);

            // Assert
            Assert.True(result);
        }
    }
}

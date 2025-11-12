using AlphaPlus.Features.Messages.API.Constants;
using AlphaPlus.Features.Messages.API.Models.Constants;
using Xunit;

namespace AlphaPlus.Features.Messages.API.UnitTests.Constants
{
    public class MessageConstantsTests
    {
        [Fact]
        public void ContentFontStartMarkup_HasCorrectValue()
        {
            // Assert
            Assert.Equal("<font color=red>", MessageConstants.ContentFontStartMarkup);
        }

        [Fact]
        public void ContentFontEndMarkup_HasCorrectValue()
        {
            // Assert
            Assert.Equal("</font>", MessageConstants.ContentFontEndMarkup);
        }

        [Fact]
        public void DefaultTotalMessageHistory_HasCorrectValue()
        {
            // Assert
            Assert.Equal(100, MessageConstants.DefaultTotalMessageHistory);
        }

        [Fact]
        public void DefaultTotalSpecialMessageHistory_HasCorrectValue()
        {
            // Assert
            Assert.Equal(500, MessageConstants.DefaultTotalSpecialMessageHistory);
        }

        [Fact]
        public void SiteMessageTag_HasCorrectValue()
        {
            // Assert
            Assert.Equal("{features}", MessageConstants.SiteMessageTag);
        }

        [Fact]
        public void ProductsTag_HasCorrectValue()
        {
            // Assert
            Assert.Equal("{products}", MessageConstants.ProductsTag);
        }

        [Fact]
        public void PageSize_HasCorrectValue()
        {
            // Assert
            Assert.Equal(100, MessageConstants.PageSize);
        }

        [Fact]
        public void PageIndex_HasCorrectValue()
        {
            // Assert
            Assert.Equal(1, MessageConstants.PageIndex);
        }

        [Fact]
        public void ParentSitesByLevel_IsImmutable()
        {
            // Act & Assert
            Assert.NotNull(MessageConstants.ParentSitesByLevel);
            Assert.IsType<System.Collections.Immutable.ImmutableDictionary<byte, string>>(MessageConstants.ParentSitesByLevel);
        }

        [Fact]
        public void ParentSitesByLevel_ContainsExpectedKeys()
        {
            // Assert
            Assert.True(MessageConstants.ParentSitesByLevel.ContainsKey(UserLevel.Super));
            Assert.True(MessageConstants.ParentSitesByLevel.ContainsKey(UserLevel.Master));
            Assert.True(MessageConstants.ParentSitesByLevel.ContainsKey(UserLevel.Agent));
        }

        [Fact]
        public void ParentSitesByLevel_HasCorrectSize()
        {
            // Assert
            Assert.Equal(3, MessageConstants.ParentSitesByLevel.Count);
        }

        [Fact]
        public void SpecialTypes_IsImmutable()
        {
            // Act & Assert
            Assert.NotNull(MessageConstants.SpecialTypes);
            Assert.IsType<System.Collections.Immutable.ImmutableDictionary<int, int>>(MessageConstants.SpecialTypes);
        }

        [Fact]
        public void SpecialTypes_ContainsExpectedKeys()
        {
            // Assert
            Assert.True(MessageConstants.SpecialTypes.ContainsKey(MessageType.Normal));
            Assert.True(MessageConstants.SpecialTypes.ContainsKey(MessageType.Special));
            Assert.True(MessageConstants.SpecialTypes.ContainsKey(MessageType.Personal));
        }

        [Fact]
        public void SpecialTypes_HasCorrectValues()
        {
            // Assert
            Assert.Equal(0, MessageConstants.SpecialTypes[MessageType.Normal]);
            Assert.Equal(1, MessageConstants.SpecialTypes[MessageType.Special]);
            Assert.Equal(2, MessageConstants.SpecialTypes[MessageType.Personal]);
        }

        [Fact]
        public void SpecialTypes_HasCorrectSize()
        {
            // Assert
            Assert.Equal(3, MessageConstants.SpecialTypes.Count);
        }

        [Fact]
        public void FontMarkups_AreConsistent()
        {
            // Assert - Start and end markups should be properly paired
            Assert.True(MessageConstants.ContentFontStartMarkup.Contains("<"));
            Assert.True(MessageConstants.ContentFontEndMarkup.Contains(">"));
            Assert.True(MessageConstants.ContentFontStartMarkup.Contains("font"));
            Assert.True(MessageConstants.ContentFontEndMarkup.Contains("font"));
        }

        [Fact]
        public void Constants_AreNotNull()
        {
            // Assert
            Assert.NotNull(MessageConstants.ContentFontStartMarkup);
            Assert.NotNull(MessageConstants.ContentFontEndMarkup);
            Assert.NotNull(MessageConstants.SiteMessageTag);
            Assert.NotNull(MessageConstants.ProductsTag);
        }

        [Fact]
        public void Constants_AreNotEmpty()
        {
            // Assert
            Assert.NotEmpty(MessageConstants.ContentFontStartMarkup);
            Assert.NotEmpty(MessageConstants.ContentFontEndMarkup);
            Assert.NotEmpty(MessageConstants.SiteMessageTag);
            Assert.NotEmpty(MessageConstants.ProductsTag);
        }

        [Fact]
        public void DefaultValues_ArePositive()
        {
            // Assert
            Assert.True(MessageConstants.DefaultTotalMessageHistory > 0);
            Assert.True(MessageConstants.DefaultTotalSpecialMessageHistory > 0);
            Assert.True(MessageConstants.PageSize > 0);
            Assert.True(MessageConstants.PageIndex > 0);
        }

        [Fact]
        public void SpecialMessageHistory_IsGreaterThanNormalHistory()
        {
            // Assert - Special messages should have higher limit than normal messages
            Assert.True(MessageConstants.DefaultTotalSpecialMessageHistory > MessageConstants.DefaultTotalMessageHistory);
        }

        [Fact]
        public void Tags_HaveProperFormat()
        {
            // Assert - Tags should be wrapped in curly braces
            Assert.StartsWith("{", MessageConstants.SiteMessageTag);
            Assert.EndsWith("}", MessageConstants.SiteMessageTag);
            Assert.StartsWith("{", MessageConstants.ProductsTag);
            Assert.EndsWith("}", MessageConstants.ProductsTag);
        }

        [Theory]
        [InlineData(UserLevel.Super)]
        [InlineData(UserLevel.Master)]
        [InlineData(UserLevel.Agent)]
        public void ParentSitesByLevel_ReturnsNonNullForValidLevels(byte level)
        {
            // Act
            var result = MessageConstants.ParentSitesByLevel[level];

            // Assert
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(MessageType.Normal, 0)]
        [InlineData(MessageType.Special, 1)]
        [InlineData(MessageType.Personal, 2)]
        public void SpecialTypes_ReturnsCorrectMapping(int messageType, int expectedValue)
        {
            // Act
            var result = MessageConstants.SpecialTypes[messageType];

            // Assert
            Assert.Equal(expectedValue, result);
        }
    }
}

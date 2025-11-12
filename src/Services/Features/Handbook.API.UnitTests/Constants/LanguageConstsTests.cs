using AlphaPlus.Features.Handbook.API.Constants;
using Xunit;

namespace AlphaPlus.Features.Handbook.API.UnitTests.Constants
{
    public class LanguageConstsTests
    {
        [Fact]
        public void EnglishKey_HasCorrectValue()
        {
            // Assert
            Assert.Equal("e", LanguageConsts.EnglishKey);
        }

        [Fact]
        public void VietnameseKey_HasCorrectValue()
        {
            // Assert
            Assert.Equal("vi", LanguageConsts.VietnameseKey);
        }

        [Fact]
        public void Default_IsEnglishKey()
        {
            // Assert
            Assert.Equal(LanguageConsts.EnglishKey, LanguageConsts.Default);
        }

        [Fact]
        public void LanguageNames_ContainsAllExpectedLanguages()
        {
            // Assert
            Assert.Equal("English", LanguageConsts.LanguageNames["e"]);
            Assert.Equal("繁體中文", LanguageConsts.LanguageNames["ct"]);
            Assert.Equal("简体中文", LanguageConsts.LanguageNames["cs"]);
            Assert.Equal("Japanese", LanguageConsts.LanguageNames["j"]);
            Assert.Equal("ภาษาไทย", LanguageConsts.LanguageNames["th"]);
            Assert.Equal("Korean", LanguageConsts.LanguageNames["ko"]);
            Assert.Equal("Tiếng Việt", LanguageConsts.LanguageNames["vi"]);
        }

        [Fact]
        public void LanguageNames_HasCorrectCount()
        {
            // Assert
            Assert.Equal(7, LanguageConsts.LanguageNames.Count);
        }

        [Fact]
        public void Codes_ContainsAllExpectedLanguageCodes()
        {
            // Assert
            Assert.Equal("en-US", LanguageConsts.Codes["e"]);
            Assert.Equal("zh-TW", LanguageConsts.Codes["ct"]);
            Assert.Equal("zh-CN", LanguageConsts.Codes["cs"]);
            Assert.Equal("ja-JP", LanguageConsts.Codes["j"]);
            Assert.Equal("th-TH", LanguageConsts.Codes["th"]);
            Assert.Equal("ko-KR", LanguageConsts.Codes["ko"]);
            Assert.Equal("vi-VN", LanguageConsts.Codes["vi"]);
        }

        [Fact]
        public void Codes_HasCorrectCount()
        {
            // Assert
            Assert.Equal(7, LanguageConsts.Codes.Count);
        }

        [Fact]
        public void Locales_ContainsAllExpectedLocales()
        {
            // Assert
            Assert.Equal("en", LanguageConsts.Locales["e"]);
            Assert.Equal("zh-Hant", LanguageConsts.Locales["ct"]);
            Assert.Equal("zh-Hans", LanguageConsts.Locales["cs"]);
            Assert.Equal("en", LanguageConsts.Locales["j"]);
            Assert.Equal("th-TH", LanguageConsts.Locales["th"]);
            Assert.Equal("en", LanguageConsts.Locales["ko"]);
            Assert.Equal("vi-VN", LanguageConsts.Locales["vi"]);
        }

        [Fact]
        public void Locales_HasCorrectCount()
        {
            // Assert
            Assert.Equal(7, LanguageConsts.Locales.Count);
        }

        [Fact]
        public void AllDictionaries_HaveSameKeys()
        {
            // Arrange
            var languageNamesKeys = LanguageConsts.LanguageNames.Keys;
            var codesKeys = LanguageConsts.Codes.Keys;
            var localesKeys = LanguageConsts.Locales.Keys;

            // Assert
            Assert.Equal(languageNamesKeys.OrderBy(k => k), codesKeys.OrderBy(k => k));
            Assert.Equal(languageNamesKeys.OrderBy(k => k), localesKeys.OrderBy(k => k));
        }

        [Theory]
        [InlineData("e")]
        [InlineData("ct")]
        [InlineData("cs")]
        [InlineData("j")]
        [InlineData("th")]
        [InlineData("ko")]
        [InlineData("vi")]
        public void AllDictionaries_ContainExpectedKey(string key)
        {
            // Assert
            Assert.True(LanguageConsts.LanguageNames.ContainsKey(key));
            Assert.True(LanguageConsts.Codes.ContainsKey(key));
            Assert.True(LanguageConsts.Locales.ContainsKey(key));
        }

        [Theory]
        [InlineData("e", "English", "en-US", "en")]
        [InlineData("vi", "Tiếng Việt", "vi-VN", "vi-VN")]
        [InlineData("ct", "繁體中文", "zh-TW", "zh-Hant")]
        [InlineData("cs", "简体中文", "zh-CN", "zh-Hans")]
        [InlineData("j", "Japanese", "ja-JP", "en")]
        [InlineData("th", "ภาษาไทย", "th-TH", "th-TH")]
        [InlineData("ko", "Korean", "ko-KR", "en")]
        public void LanguageMapping_IsConsistent(string key, string expectedName, string expectedCode, string expectedLocale)
        {
            // Assert
            Assert.Equal(expectedName, LanguageConsts.LanguageNames[key]);
            Assert.Equal(expectedCode, LanguageConsts.Codes[key]);
            Assert.Equal(expectedLocale, LanguageConsts.Locales[key]);
        }

        [Fact]
        public void EnglishKey_ExistsInAllDictionaries()
        {
            // Assert
            Assert.True(LanguageConsts.LanguageNames.ContainsKey(LanguageConsts.EnglishKey));
            Assert.True(LanguageConsts.Codes.ContainsKey(LanguageConsts.EnglishKey));
            Assert.True(LanguageConsts.Locales.ContainsKey(LanguageConsts.EnglishKey));
        }

        [Fact]
        public void VietnameseKey_ExistsInAllDictionaries()
        {
            // Assert
            Assert.True(LanguageConsts.LanguageNames.ContainsKey(LanguageConsts.VietnameseKey));
            Assert.True(LanguageConsts.Codes.ContainsKey(LanguageConsts.VietnameseKey));
            Assert.True(LanguageConsts.Locales.ContainsKey(LanguageConsts.VietnameseKey));
        }

        [Fact]
        public void Default_ExistsInAllDictionaries()
        {
            // Assert
            Assert.True(LanguageConsts.LanguageNames.ContainsKey(LanguageConsts.Default));
            Assert.True(LanguageConsts.Codes.ContainsKey(LanguageConsts.Default));
            Assert.True(LanguageConsts.Locales.ContainsKey(LanguageConsts.Default));
        }

        [Fact]
        public void LanguageNames_ValuesAreNotEmpty()
        {
            // Assert
            foreach (var value in LanguageConsts.LanguageNames.Values)
            {
                Assert.False(string.IsNullOrWhiteSpace(value));
            }
        }

        [Fact]
        public void Codes_ValuesAreNotEmpty()
        {
            // Assert
            foreach (var value in LanguageConsts.Codes.Values)
            {
                Assert.False(string.IsNullOrWhiteSpace(value));
            }
        }

        [Fact]
        public void Locales_ValuesAreNotEmpty()
        {
            // Assert
            foreach (var value in LanguageConsts.Locales.Values)
            {
                Assert.False(string.IsNullOrWhiteSpace(value));
            }
        }
    }
}

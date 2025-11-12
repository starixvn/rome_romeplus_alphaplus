using AlphaPlus.Settings.ViewLogs.API.Models;

namespace AlphaPlus.Settings.ViewLogs.API.UnitTests.Models;

public class LogActionTests
{
    [Fact]
    public void LogAction_Should_Initialize_With_Default_Values()
    {
        // Act
        var logAction = new LogAction();

        // Assert
        Assert.Equal(0, logAction.Id);
        Assert.Equal(0, logAction.ActionTypeId);
        Assert.Null(logAction.ChineseTraditionalName);
        Assert.Null(logAction.ChineseSimplifiedName);
        Assert.Null(logAction.EnglishName);
        Assert.Null(logAction.VietnameseName);
        Assert.Null(logAction.JapaneseName);
        Assert.Null(logAction.KoreanName);
        Assert.Null(logAction.ThaiName);
        Assert.False(logAction.IsInternal);
        Assert.Null(logAction.SPName);
    }

    [Fact]
    public void LogAction_Should_Set_All_Properties()
    {
        // Arrange
        var logAction = new LogAction
        {
            Id = 1,
            ActionTypeId = 100,
            ChineseTraditionalName = "繁體中文",
            ChineseSimplifiedName = "简体中文",
            EnglishName = "English Action",
            VietnameseName = "Hành động tiếng Việt",
            JapaneseName = "日本語アクション",
            KoreanName = "한국어 액션",
            ThaiName = "การดำเนินการภาษาไทย",
            IsInternal = true,
            SPName = "sp_LogAction"
        };

        // Assert
        Assert.Equal(1, logAction.Id);
        Assert.Equal(100, logAction.ActionTypeId);
        Assert.Equal("繁體中文", logAction.ChineseTraditionalName);
        Assert.Equal("简体中文", logAction.ChineseSimplifiedName);
        Assert.Equal("English Action", logAction.EnglishName);
        Assert.Equal("Hành động tiếng Việt", logAction.VietnameseName);
        Assert.Equal("日本語アクション", logAction.JapaneseName);
        Assert.Equal("한국어 액션", logAction.KoreanName);
        Assert.Equal("การดำเนินการภาษาไทย", logAction.ThaiName);
        Assert.True(logAction.IsInternal);
        Assert.Equal("sp_LogAction", logAction.SPName);
    }

    [Fact]
    public void LogAction_Should_Handle_Partial_Data()
    {
        // Arrange
        var logAction = new LogAction
        {
            Id = 5,
            ActionTypeId = 200,
            EnglishName = "Login Action",
            IsInternal = false
        };

        // Assert
        Assert.Equal(5, logAction.Id);
        Assert.Equal(200, logAction.ActionTypeId);
        Assert.Equal("Login Action", logAction.EnglishName);
        Assert.False(logAction.IsInternal);
        Assert.Null(logAction.ChineseTraditionalName);
        Assert.Null(logAction.SPName);
    }

    [Fact]
    public void LogAction_Should_Handle_Special_Characters_In_Names()
    {
        // Arrange
        var logAction = new LogAction
        {
            EnglishName = "Action with special chars: @#$%^&*()",
            VietnameseName = "Hành động với ký tự đặc biệt: àáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþÿ"
        };

        // Assert
        Assert.Equal("Action with special chars: @#$%^&*()", logAction.EnglishName);
        Assert.Equal("Hành động với ký tự đặc biệt: àáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþÿ", logAction.VietnameseName);
    }

    [Fact]
    public void LogAction_Should_Handle_Boolean_IsInternal_Property()
    {
        // Arrange
        var internalAction = new LogAction { IsInternal = true };
        var externalAction = new LogAction { IsInternal = false };

        // Assert
        Assert.True(internalAction.IsInternal);
        Assert.False(externalAction.IsInternal);
    }
}

using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Transfer.API.Helpers;

namespace AlphaPlus.Settings.Transfer.API.UnitTests.Helpers;

public class TransferConditionHelperTests
{
    [Fact]
    public void GetTransferrableDatesFromSchedule_Should_Parse_Valid_Schedule_String()
    {
        // Arrange
        var schedule = "1,3,5,7";

        // Act
        var result = TransferConditionHelper.GetTransferrableDatesFromSchedule(schedule);

        // Assert
        Assert.Equal(4, result.Count);
        Assert.Contains(1, result);
        Assert.Contains(3, result);
        Assert.Contains(5, result);
        Assert.Contains(7, result);
    }

    [Fact]
    public void GetTransferrableDatesFromSchedule_Should_Handle_Empty_String()
    {
        // Arrange
        var schedule = string.Empty;

        // Act
        var result = TransferConditionHelper.GetTransferrableDatesFromSchedule(schedule);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetTransferrableDatesFromSchedule_Should_Handle_Null_String()
    {
        // Arrange
        string schedule = null;

        // Act
        var result = TransferConditionHelper.GetTransferrableDatesFromSchedule(schedule);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetTransferrableDatesFromSchedule_Should_Handle_Single_Date()
    {
        // Arrange
        var schedule = "4";

        // Act
        var result = TransferConditionHelper.GetTransferrableDatesFromSchedule(schedule);

        // Assert
        Assert.Single(result);
        Assert.Equal(4, result[0]);
    }

    [Fact]
    public void GetAutoTransferScheduleFromDates_String_Should_Return_Input_String()
    {
        // Arrange
        var selectedDates = "1,2,3,4,5";

        // Act
        var result = TransferConditionHelper.GetAutoTransferScheduleFromDates(selectedDates);

        // Assert
        Assert.Equal(selectedDates, result);
    }

    [Fact]
    public void GetAutoTransferScheduleFromDates_String_Should_Handle_Null()
    {
        // Arrange
        string selectedDates = null;

        // Act
        var result = TransferConditionHelper.GetAutoTransferScheduleFromDates(selectedDates);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GetAutoTransferScheduleFromDates_Array_Should_Join_With_Comma()
    {
        // Arrange
        var selectedDates = new[] { "Monday", "Wednesday", "Friday" };

        // Act
        var result = TransferConditionHelper.GetAutoTransferScheduleFromDates(selectedDates);

        // Assert
        Assert.Equal("Monday,Wednesday,Friday", result);
    }

    [Fact]
    public void GetAutoTransferScheduleFromDates_Array_Should_Handle_Empty_Array()
    {
        // Arrange
        var selectedDates = new string[0];

        // Act
        var result = TransferConditionHelper.GetAutoTransferScheduleFromDates(selectedDates);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GetTransferrableDatesFromDates_Should_Parse_Date_String()
    {
        // Arrange
        var selectedDates = "2,4,6";

        // Act
        var result = TransferConditionHelper.GetTransferrableDatesFromDates(selectedDates);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Contains(2, result);
        Assert.Contains(4, result);
        Assert.Contains(6, result);
    }

    [Theory]
    [InlineData(1, "Sun")]
    [InlineData(2, "Mon")]
    [InlineData(3, "Tue")]
    [InlineData(4, "Wed")]
    [InlineData(5, "Thu")]
    [InlineData(6, "Fri")]
    [InlineData(7, "Sat")]
    public void GetDateName_Should_Return_Correct_Day_Name(int dateIndex, string expectedName)
    {
        // Act
        var result = TransferConditionHelper.GetDateName(dateIndex);

        // Assert
        Assert.Equal(expectedName, result);
    }

    [Fact]
    public void GetTransferOption_Should_Return_Daily_Option_When_All_Seven_Days_Selected()
    {
        // Arrange
        var selectedDates = new List<int> { 1, 2, 3, 4, 5, 6, 7 };

        // Act
        var result = TransferConditionHelper.GetTransferOption(selectedDates);

        // Assert
        Assert.Equal("00000000", result); // DailyOption
    }

    [Fact]
    public void GetTransferOption_Should_Build_Correct_Pattern_For_Specific_Days()
    {
        // Arrange
        var selectedDates = new List<int> { 1, 3, 5, 7 }; // Sunday, Tuesday, Thursday, Saturday

        // Act
        var result = TransferConditionHelper.GetTransferOption(selectedDates);

        // Assert
        Assert.Equal("10101011", result); // Pattern: 1010101 + 1 (weekly indicator)
    }

    [Fact]
    public void GetTransferOption_Should_Handle_Empty_List()
    {
        // Arrange
        var selectedDates = new List<int>();

        // Act
        var result = TransferConditionHelper.GetTransferOption(selectedDates);

        // Assert
        Assert.Equal("00000001", result); // All zeros + 1 (weekly indicator)
    }

    [Fact]
    public void GetTransferOption_Should_Handle_Single_Day()
    {
        // Arrange
        var selectedDates = new List<int> { 3 }; // Tuesday

        // Act
        var result = TransferConditionHelper.GetTransferOption(selectedDates);

        // Assert
        Assert.Equal("00100001", result); // Only Tuesday enabled + 1 (weekly indicator)
    }

    [Fact]
    public void GetAutoTransferScheduleFromTransferOption_Should_Return_All_Days_For_Super_User()
    {
        // Arrange
        var transferOption = "1010101";
        var roleId = UserLevel.Super.Value;

        // Act
        var result = TransferConditionHelper.GetAutoTransferScheduleFromTransferOption(transferOption, roleId);

        // Assert
        Assert.Equal(7, result.Count);
        Assert.Equal(new List<int> { 2, 3, 4, 5, 6, 7, 1 }, result);
    }

    [Fact]
    public void GetAutoTransferScheduleFromTransferOption_Should_Return_Empty_For_Null_Or_Empty_Option()
    {
        // Arrange
        var roleId = (byte)1; // Non-super user

        // Act
        var resultNull = TransferConditionHelper.GetAutoTransferScheduleFromTransferOption(null, roleId);
        var resultEmpty = TransferConditionHelper.GetAutoTransferScheduleFromTransferOption(string.Empty, roleId);
        var resultWhitespace = TransferConditionHelper.GetAutoTransferScheduleFromTransferOption("   ", roleId);

        // Assert
        Assert.Empty(resultNull);
        Assert.Empty(resultEmpty);
        Assert.Empty(resultWhitespace);
    }

    [Fact]
    public void GetAutoTransferScheduleFromTransferOption_Should_Return_All_Days_For_Daily_Option()
    {
        // Arrange
        var transferOption = "10101010"; // 8th character is '0' - daily
        var roleId = (byte)1;

        // Act
        var result = TransferConditionHelper.GetAutoTransferScheduleFromTransferOption(transferOption, roleId);

        // Assert
        Assert.Equal(7, result.Count);
        Assert.Equal(new List<int> { 2, 3, 4, 5, 6, 7, 1 }, result);
    }

    [Fact]
    public void GetAutoTransferScheduleFromTransferOption_Should_Return_Specific_Days_For_Weekly_Option()
    {
        // Arrange
        var transferOption = "10101011"; // 8th character is '1' - weekly, pattern 1010101
        var roleId = (byte)1;

        // Act
        var result = TransferConditionHelper.GetAutoTransferScheduleFromTransferOption(transferOption, roleId);

        // Assert
        Assert.Equal(4, result.Count);
        Assert.Contains(1, result); // Sunday (index 0 -> day 1)
        Assert.Contains(3, result); // Tuesday (index 2 -> day 3)
        Assert.Contains(5, result); // Thursday (index 4 -> day 5)
        Assert.Contains(7, result); // Saturday (index 6 -> day 7)
    }

    [Fact]
    public void GetAutoTransferScheduleFromTransferOption_Should_Handle_Valid_Length_Transfer_Option()
    {
        // Arrange
        var transferOption = "10101010"; // Valid length (8 characters)
        var roleId = (byte)1;

        // Act
        var result = TransferConditionHelper.GetAutoTransferScheduleFromTransferOption(transferOption, roleId);

        // Assert
        Assert.NotNull(result); // Should return a valid result for proper length strings
    }

    [Fact]
    public void BuildDaysFromSchedule_Should_Return_Daily_For_All_Seven_Days()
    {
        // Arrange
        var scheduleWeekDays = "1,2,3,4,5,6,7";

        // Act
        var result = TransferConditionHelper.BuildDaysFromSchedule(scheduleWeekDays);

        // Assert
        Assert.Equal("Daily", result);
    }

    [Fact]
    public void BuildDaysFromSchedule_Should_Return_Hyphen_Separated_Day_Names()
    {
        // Arrange
        var scheduleWeekDays = "1,3,5"; // Sunday, Tuesday, Thursday

        // Act
        var result = TransferConditionHelper.BuildDaysFromSchedule(scheduleWeekDays);

        // Assert
        Assert.Equal("Sun-Tue-Thu", result);
    }

    [Fact]
    public void BuildDaysFromSchedule_Should_Handle_Empty_Or_Null_Input()
    {
        // Act
        var resultNull = TransferConditionHelper.BuildDaysFromSchedule(null);
        var resultEmpty = TransferConditionHelper.BuildDaysFromSchedule(string.Empty);
        var resultWhitespace = TransferConditionHelper.BuildDaysFromSchedule("   ");

        // Assert
        Assert.Equal(string.Empty, resultNull);
        Assert.Equal(string.Empty, resultEmpty);
        Assert.Equal(string.Empty, resultWhitespace);
    }

    [Fact]
    public void BuildDaysFromSchedule_Should_Handle_Single_Day()
    {
        // Arrange
        var scheduleWeekDays = "4"; // Wednesday

        // Act
        var result = TransferConditionHelper.BuildDaysFromSchedule(scheduleWeekDays);

        // Assert
        Assert.Equal("Wed", result);
    }

    [Theory]
    [InlineData("1,2,3", "Sun-Mon-Tue")]
    [InlineData("6,7", "Fri-Sat")]
    [InlineData("2,4,6", "Mon-Wed-Fri")]
    public void BuildDaysFromSchedule_Should_Handle_Various_Day_Combinations(string input, string expected)
    {
        // Act
        var result = TransferConditionHelper.BuildDaysFromSchedule(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetTransferOption_Should_Be_Consistent_With_GetAutoTransferScheduleFromTransferOption()
    {
        // Arrange
        var selectedDates = new List<int> { 1, 3, 5 }; // Sunday, Tuesday, Thursday

        // Act
        var transferOption = TransferConditionHelper.GetTransferOption(selectedDates);
        var retrievedDates = TransferConditionHelper.GetAutoTransferScheduleFromTransferOption(transferOption, (byte)1);

        // Assert
        Assert.Equal(selectedDates.OrderBy(x => x).ToList(), retrievedDates.OrderBy(x => x).ToList());
    }

    [Fact]
    public void GetDateName_Should_Throw_Exception_For_Invalid_Index()
    {
        // Act & Assert
        Assert.Throws<IndexOutOfRangeException>(() => TransferConditionHelper.GetDateName(0));
        Assert.Throws<IndexOutOfRangeException>(() => TransferConditionHelper.GetDateName(8));
        Assert.Throws<IndexOutOfRangeException>(() => TransferConditionHelper.GetDateName(-1));
    }
}

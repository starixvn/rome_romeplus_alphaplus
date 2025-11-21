using System.Globalization;
using AlphaPlus.Settings.Transfer.API.Helpers;
using Fanex.RemotingRef;

namespace AlphaPlus.Settings.Transfer.API.UnitTests.Helpers;

public class CacheRefServiceHelperTests
{
    private readonly Mock<IRemotingRefService> _mockRemotingRefService;

    public CacheRefServiceHelperTests()
    {
        _mockRemotingRefService = new Mock<IRemotingRefService>();
    }

    [Fact]
    public void GetProgramName_Should_Return_Program_Name_From_Service()
    {
        // Arrange
        var resourceId = 123;
        var expectedProgramName = "Test Reward Program";
        var currentCulture = Thread.CurrentThread.CurrentCulture.Name;

        _mockRemotingRefService
            .Setup(x => x.GetResourceDataByRefIdAndResourceId(
                "RewardProgram",
                resourceId.ToString(),
                currentCulture,
                string.Empty))
            .Returns(expectedProgramName);

        CacheRefServiceHelper.Instance = _mockRemotingRefService.Object;

        // Act
        var result = CacheRefServiceHelper.GetProgramName(resourceId);

        // Assert
        Assert.Equal(expectedProgramName, result);
        _mockRemotingRefService.Verify(x => x.GetResourceDataByRefIdAndResourceId(
            "RewardProgram",
            resourceId.ToString(),
            currentCulture,
            string.Empty), Times.Once);
    }

    [Fact]
    public void GetProgramName_Should_Use_Current_Thread_Culture()
    {
        // Arrange
        var resourceId = 456;
        var expectedProgramName = "プログラム名";
        var originalCulture = Thread.CurrentThread.CurrentCulture;

        try
        {
            // Set a specific culture
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ja-JP");
            var currentCulture = Thread.CurrentThread.CurrentCulture.Name;

            _mockRemotingRefService
                .Setup(x => x.GetResourceDataByRefIdAndResourceId(
                    "RewardProgram",
                    resourceId.ToString(),
                    currentCulture,
                    string.Empty))
                .Returns(expectedProgramName);

            CacheRefServiceHelper.Instance = _mockRemotingRefService.Object;

            // Act
            var result = CacheRefServiceHelper.GetProgramName(resourceId);

            // Assert
            Assert.Equal(expectedProgramName, result);
            _mockRemotingRefService.Verify(x => x.GetResourceDataByRefIdAndResourceId(
                "RewardProgram",
                resourceId.ToString(),
                "ja-JP",
                string.Empty), Times.Once);
        }
        finally
        {
            // Restore original culture
            Thread.CurrentThread.CurrentCulture = originalCulture;
        }
    }

    [Theory]
    [InlineData(1, "Program One")]
    [InlineData(999, "Program Nine Nine Nine")]
    [InlineData(0, "Default Program")]
    public void GetProgramName_Should_Handle_Different_Resource_Ids(int resourceId, string expectedName)
    {
        // Arrange
        var currentCulture = Thread.CurrentThread.CurrentCulture.Name;

        _mockRemotingRefService
            .Setup(x => x.GetResourceDataByRefIdAndResourceId(
                "RewardProgram",
                resourceId.ToString(),
                currentCulture,
                string.Empty))
            .Returns(expectedName);

        CacheRefServiceHelper.Instance = _mockRemotingRefService.Object;

        // Act
        var result = CacheRefServiceHelper.GetProgramName(resourceId);

        // Assert
        Assert.Equal(expectedName, result);
    }

    [Fact]
    public void GetProgramName_Should_Return_Null_When_Service_Returns_Null()
    {
        // Arrange
        var resourceId = 123;
        var currentCulture = Thread.CurrentThread.CurrentCulture.Name;

        _mockRemotingRefService
            .Setup(x => x.GetResourceDataByRefIdAndResourceId(
                "RewardProgram",
                resourceId.ToString(),
                currentCulture,
                string.Empty))
            .Returns((string)null);

        CacheRefServiceHelper.Instance = _mockRemotingRefService.Object;

        // Act
        var result = CacheRefServiceHelper.GetProgramName(resourceId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetProgramName_Should_Return_Empty_String_When_Service_Returns_Empty()
    {
        // Arrange
        var resourceId = 123;
        var currentCulture = Thread.CurrentThread.CurrentCulture.Name;

        _mockRemotingRefService
            .Setup(x => x.GetResourceDataByRefIdAndResourceId(
                "RewardProgram",
                resourceId.ToString(),
                currentCulture,
                string.Empty))
            .Returns(string.Empty);

        CacheRefServiceHelper.Instance = _mockRemotingRefService.Object;

        // Act
        var result = CacheRefServiceHelper.GetProgramName(resourceId);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GetProgramName_Should_Propagate_Exception_From_Service()
    {
        // Arrange
        var resourceId = 123;
        var currentCulture = Thread.CurrentThread.CurrentCulture.Name;
        var expectedException = new InvalidOperationException("Service error");

        _mockRemotingRefService
            .Setup(x => x.GetResourceDataByRefIdAndResourceId(
                "RewardProgram",
                resourceId.ToString(),
                currentCulture,
                string.Empty))
            .Throws(expectedException);

        CacheRefServiceHelper.Instance = _mockRemotingRefService.Object;

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => CacheRefServiceHelper.GetProgramName(resourceId));
        Assert.Equal("Service error", exception.Message);
    }

    [Fact]
    public void Instance_Property_Should_Allow_Setting_And_Getting()
    {
        // Arrange
        var mockService = new Mock<IRemotingRefService>();

        // Act
        CacheRefServiceHelper.Instance = mockService.Object;
        var retrievedInstance = CacheRefServiceHelper.Instance;

        // Assert
        Assert.Same(mockService.Object, retrievedInstance);
    }

    [Fact]
    public void GetProgramName_Should_Always_Use_RewardProgram_As_RefId()
    {
        // Arrange
        var resourceId = 789;
        var currentCulture = Thread.CurrentThread.CurrentCulture.Name;

        _mockRemotingRefService
            .Setup(x => x.GetResourceDataByRefIdAndResourceId(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Returns("Test Program");

        CacheRefServiceHelper.Instance = _mockRemotingRefService.Object;

        // Act
        CacheRefServiceHelper.GetProgramName(resourceId);

        // Assert
        _mockRemotingRefService.Verify(x => x.GetResourceDataByRefIdAndResourceId(
            "RewardProgram", // Should always be "RewardProgram"
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void GetProgramName_Should_Always_Use_Empty_String_As_Last_Parameter()
    {
        // Arrange
        var resourceId = 789;
        var currentCulture = Thread.CurrentThread.CurrentCulture.Name;

        _mockRemotingRefService
            .Setup(x => x.GetResourceDataByRefIdAndResourceId(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Returns("Test Program");

        CacheRefServiceHelper.Instance = _mockRemotingRefService.Object;

        // Act
        CacheRefServiceHelper.GetProgramName(resourceId);

        // Assert
        _mockRemotingRefService.Verify(x => x.GetResourceDataByRefIdAndResourceId(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            string.Empty), Times.Once); // Should always be empty string
    }

    [Fact]
    public void GetProgramName_Should_Convert_ResourceId_To_String()
    {
        // Arrange
        var resourceId = 12345;
        var currentCulture = Thread.CurrentThread.CurrentCulture.Name;

        _mockRemotingRefService
            .Setup(x => x.GetResourceDataByRefIdAndResourceId(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Returns("Test Program");

        CacheRefServiceHelper.Instance = _mockRemotingRefService.Object;

        // Act
        CacheRefServiceHelper.GetProgramName(resourceId);

        // Assert
        _mockRemotingRefService.Verify(x => x.GetResourceDataByRefIdAndResourceId(
            It.IsAny<string>(),
            "12345", // Should be converted to string
            It.IsAny<string>(),
            It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void GetProgramName_Should_Work_With_Multiple_Cultures()
    {
        // Arrange
        var resourceId = 100;
        var cultures = new[] { "en-US", "zh-TW", "zh-CN", "th-TH", "ja-JP" };
        var expectedResults = new[] { "English Program", "中文程式", "中文程序", "โปรแกรมไทย", "日本プログラム" };

        for (int i = 0; i < cultures.Length; i++)
        {
            var culture = cultures[i];
            var expectedResult = expectedResults[i];
            var originalCulture = Thread.CurrentThread.CurrentCulture;

            try
            {
                // Set culture
                Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);

                _mockRemotingRefService
                    .Setup(x => x.GetResourceDataByRefIdAndResourceId(
                        "RewardProgram",
                        resourceId.ToString(),
                        culture,
                        string.Empty))
                    .Returns(expectedResult);

                CacheRefServiceHelper.Instance = _mockRemotingRefService.Object;

                // Act
                var result = CacheRefServiceHelper.GetProgramName(resourceId);

                // Assert
                Assert.Equal(expectedResult, result);
            }
            finally
            {
                // Restore original culture
                Thread.CurrentThread.CurrentCulture = originalCulture;
            }
        }
    }
}

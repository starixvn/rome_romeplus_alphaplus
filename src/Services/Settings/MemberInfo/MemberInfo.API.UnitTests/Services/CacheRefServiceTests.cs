using AlphaPlus.Settings.MemberInfo.API.Services;
using Fanex.RemotingRef;
using Moq;
using Xunit;
using Microsoft.Extensions.Configuration;
using CoreConfigManager = AlphaPlus.Core.Configurations.ConfigurationManager;

namespace AlphaPlus.Settings.MemberInfo.API.UnitTests.Services;

public class CacheRefServiceTests
{
	[Fact]
	public void GetProductName_WhenSiteLabel777_AndProductNameExists_ReturnsSite777Name()
	{
		// Arrange
		var config = new ConfigurationBuilder()
			.AddInMemoryCollection(new Dictionary<string, string> { { "SiteLabel", "777" } })
			.Build();
		CoreConfigManager.Init(config);

		var remotingMock = new Mock<IRemotingRefService>();
		remotingMock.Setup(r => r.GetResourceDataByRefIdAndResourceId("Product_Saba", "10", It.IsAny<string>(), string.Empty))
			.Returns("Site777Name");

		var service = new CacheRefServiceImpl(remotingMock.Object);

		// Act
		var name = service.GetProductName(10);

		// Assert
		Assert.Equal("Site777Name", name);
		remotingMock.Verify(r => r.GetResourceDataByRefIdAndResourceId("Product_Saba", "10", It.IsAny<string>(), string.Empty), Times.Once);
	}

	[Fact]
	public void GetProductName_WhenSiteLabel777_AndSiteNameEmpty_FallsBackToDefault()
	{
		// Arrange
		var config = new ConfigurationBuilder()
			.AddInMemoryCollection(new Dictionary<string, string> { { "SiteLabel", "777" } })
			.Build();
		CoreConfigManager.Init(config);

		var remotingMock = new Mock<IRemotingRefService>();
		remotingMock.Setup(r => r.GetResourceDataByRefIdAndResourceId("Product_Saba", "11", It.IsAny<string>(), string.Empty))
			.Returns(string.Empty);
		remotingMock.Setup(r => r.GetResourceDataByRefIdAndResourceId("Product", "11", It.IsAny<string>(), string.Empty))
			.Returns("DefaultName");

		var service = new CacheRefServiceImpl(remotingMock.Object);

		// Act
		var name = service.GetProductName(11);

		// Assert
		Assert.Equal("DefaultName", name);
		remotingMock.Verify(r => r.GetResourceDataByRefIdAndResourceId("Product_Saba", "11", It.IsAny<string>(), string.Empty), Times.Once);
		remotingMock.Verify(r => r.GetResourceDataByRefIdAndResourceId("Product", "11", It.IsAny<string>(), string.Empty), Times.Once);
	}

	[Fact]
	public void GetSportTypeName_WhenRemotingReturnsEmpty_UsesDummyRef()
	{
		// Arrange
		var remotingMock = new Mock<IRemotingRefService>();
		remotingMock.Setup(r => r.GetSportName("20", It.IsAny<string>())).Returns(string.Empty);
		remotingMock.Setup(r => r.GetResourceDataByRefIdAndResourceId("DummySport", "20", It.IsAny<string>(), string.Empty)).Returns("DummySportName");

		var service = new CacheRefServiceImpl(remotingMock.Object);

		// Act
		var name = service.GetSportTypeName(20);

		// Assert
		Assert.Equal("DummySportName", name);
	}

	[Fact]
	public void GetCustBetBehavior_WhenNotSiteLabel777_Returns_DefaultBehavior()
	{
		// Arrange
		var config = new ConfigurationBuilder()
			.AddInMemoryCollection(new Dictionary<string, string> { { "SiteLabel", "mbc" } })
			.Build();
		CoreConfigManager.Init(config);

		var remotingMock = new Mock<IRemotingRefService>();
		remotingMock.Setup(r => r.GetResourceDataByRefIdAndResourceId("CustBetBehaviour", "abc", It.IsAny<string>(), string.Empty)).Returns("Normal");

		var service = new CacheRefServiceImpl(remotingMock.Object);

		// Act
		var name = service.GetCustBetBehavior("abc");

		// Assert
		Assert.Equal("Normal", name);
	}

	[Fact]
	public void GetCustBetBehavior_WhenSiteLabel777_TriesSiteSpecificFirst()
	{
		// Arrange
		var config = new ConfigurationBuilder()
			.AddInMemoryCollection(new Dictionary<string, string> { { "SiteLabel", "777" } })
			.Build();
		CoreConfigManager.Init(config);

		var remotingMock = new Mock<IRemotingRefService>();
		remotingMock.Setup(r => r.GetResourceDataByRefIdAndResourceId("CustBetBehaviour", "abc_777", It.IsAny<string>(), string.Empty)).Returns("SiteSpecific");

		var service = new CacheRefServiceImpl(remotingMock.Object);

		// Act
		var name = service.GetCustBetBehavior("abc");

		// Assert
		Assert.Equal("SiteSpecific", name);
	}
}

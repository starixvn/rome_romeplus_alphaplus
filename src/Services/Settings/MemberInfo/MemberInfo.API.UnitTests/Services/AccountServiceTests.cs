using AlphaPlus.Settings.MemberInfo.API.Services;
using AlphaPlus.Settings.MemberInfo.API.Criteria;
using AlphaPlus.Settings.MemberInfo.Domain.Features.Shared.Models;
using Fanex.Data.Repository;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlphaPlus.Settings.MemberInfo.API.UnitTests.Services;

public class AccountServiceTests
{
	[Fact]
	public void GetTransferOption_WhenRoleIsSuper_Returns_DailyTransferCondition()
	{
		// Arrange
		var repositoryMock = new Mock<IDynamicRepository>();
		var service = new AccountService(repositoryMock.Object);

		// Act
		var result = service.GetTransferOption(123, AlphaPlus.Core.Models.UserLevel.Super.Value);

		// Assert
		Assert.Equal("00000000", result);
		// repository should not be called for super
		repositoryMock.Verify(r => r.Execute(It.IsAny<GetTransferOptionCriteria>(), It.IsAny<string>()), Times.Never);
	}

	[Fact]
	public void GetTransferOption_WhenRoleIsNotSuper_ExecutesRepositoryAndReturns_TransferOption()
	{
		// Arrange
		var repositoryMock = new Mock<IDynamicRepository>();
		repositoryMock
			.Setup(r => r.Execute(It.IsAny<INonQueryCommand>(), It.IsAny<string>()))
			.Callback<INonQueryCommand, string>((c, s) => ((GetTransferOptionCriteria)c).TransferOption = "OPTION_ABC");

		var service = new AccountService(repositoryMock.Object);

		// Act
		var result = service.GetTransferOption(123, AlphaPlus.Core.Models.UserLevel.Member.Value);

		// Assert
		Assert.Equal("OPTION_ABC", result);
		repositoryMock.Verify(r => r.Execute(It.Is<GetTransferOptionCriteria>(x => x.CustId == 123), It.IsAny<string>()), Times.Once);
	}

	[Fact]
	public async Task GetMasterList_CallsRepositoryFetchAsync_AndReturnsResults()
	{
		// Arrange
		var repositoryMock = new Mock<IDynamicRepository>();
		var expected = new List<CustInfo>
		{
			new CustInfo(1, "A", (byte)1),
			new CustInfo(2, "B", (byte)1)
		};

		repositoryMock
			.Setup(r => r.FetchAsync<CustInfo>(It.IsAny<GetMasterListCriteria>(), It.IsAny<string>()))
			.ReturnsAsync(expected);

		var service = new AccountService(repositoryMock.Object);

		// Act
		var result = await service.GetMasterList(99);

		// Assert
		Assert.NotNull(result);
		Assert.Equal(2, System.Linq.Enumerable.Count(result));
		repositoryMock.Verify(r => r.FetchAsync<CustInfo>(It.Is<GetMasterListCriteria>(c => c.SRecommend == 99), It.IsAny<string>()), Times.Once);
	}

	[Fact]
	public async Task GetAgentList_CallsRepositoryFetchAsync_AndReturnsResults()
	{
		// Arrange
		var repositoryMock = new Mock<IDynamicRepository>();
		var expected = new List<CustInfo>
		{
			new CustInfo(10, "X", (byte)1)
		};

		repositoryMock
			.Setup(r => r.FetchAsync<CustInfo>(It.IsAny<GetAgentListCriteria>(), It.IsAny<string>()))
			.ReturnsAsync(expected);

		var service = new AccountService(repositoryMock.Object);

		// Act
		var result = await service.GetAgentList(55);

		// Assert
		Assert.NotNull(result);
		Assert.Single(result);
		repositoryMock.Verify(r => r.FetchAsync<CustInfo>(It.Is<GetAgentListCriteria>(c => c.MRecommend == 55), It.IsAny<string>()), Times.Once);
	}
}

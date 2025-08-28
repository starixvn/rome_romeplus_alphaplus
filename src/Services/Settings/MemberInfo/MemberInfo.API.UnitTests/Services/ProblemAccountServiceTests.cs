using AlphaPlus.Settings.MemberInfo.API.Services;
using AlphaPlus.Settings.MemberInfo.Domain.Params.ProblemAccount;
using AlphaPlus.Settings.MemberInfo.API.Criteria.ProblemAccount;
using AlphaPlus.Settings.MemberInfo.Domain.Models.ProblemAccount;
using Fanex.Data.Repository;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace AlphaPlus.Settings.MemberInfo.API.UnitTests.Services;

public class ProblemAccountServiceTests
{
	[Fact]
	public async Task UpdateReviewStatus_WhenRepositoryReturnsNoError_ReturnsSuccessResult()
	{
		// Arrange
		var repoMock = new Mock<IDynamicRepository>();
		// Simulate repository executing and setting ErrCode = 0
		repoMock.Setup(r => r.ExecuteAsync(It.IsAny<INonQueryCommand>(), It.IsAny<string>()))
			.Callback<INonQueryCommand, string>((c, s) => ((UpdateReviewStatusProblemAccountCriteria)c).ErrCode = 0)
			.Returns(Task.FromResult(0));

		var service = new ProblemAccountService(repoMock.Object);
		var param = new UpdateProblemAccountParam { CustId = 1, SubAccId = 0, RoleId = 0 };

		// Act
		var result = await service.UpdateReviewStatus(param);

		// Assert
		Assert.Equal(0, result.Code);
		Assert.Equal("Successfully updated!", result.Message);
	}

	[Fact]
	public async Task UpdateReviewStatus_WhenRepositorySetsError_Returns_ErrorResult()
	{
		// Arrange
		var repoMock = new Mock<IDynamicRepository>();
		repoMock.Setup(r => r.ExecuteAsync(It.IsAny<INonQueryCommand>(), It.IsAny<string>()))
			.Callback<INonQueryCommand, string>((c, s) => { ((UpdateReviewStatusProblemAccountCriteria)c).ErrCode = -1; ((UpdateReviewStatusProblemAccountCriteria)c).ErrMsg = "Fail"; })
			.Returns(Task.FromResult(-1));

		var service = new ProblemAccountService(repoMock.Object);
		var param = new UpdateProblemAccountParam { CustId = 1, SubAccId = 0, RoleId = 0 };

		// Act
		var result = await service.UpdateReviewStatus(param);

		// Assert
		Assert.Equal(-1, result.Code);
		Assert.Equal("Fail", result.Message);
	}

	[Fact]
	public async Task Get_WhenRepositoryReturnsItems_Returns_OrderedResults()
	{
		// Arrange
		var repoMock = new Mock<IDynamicRepository>();
		var items = new List<ProblemAccountModel>
		{
			new ProblemAccountModel { CustId = 1, IsReviewed = true, CreatedDate = System.DateTime.UtcNow.AddDays(-1) },
			new ProblemAccountModel { CustId = 2, IsReviewed = false, CreatedDate = System.DateTime.UtcNow }
		};

		repoMock.Setup(r => r.FetchAsync<ProblemAccountModel>(It.IsAny<GetProblemAccountCriteria>(), It.IsAny<string>()))
			.ReturnsAsync(items);

		var service = new ProblemAccountService(repoMock.Object);
		var param = new GetProblemAccountParam { CustId = 0 };

		// Act
		var result = await service.Get(param);

		// Assert
		var array = result.ToArray();
		Assert.Equal(2, array.Length);
		// First should be not reviewed (IsReviewed == false), then the reviewed one
		Assert.False(array[0].IsReviewed);
		Assert.True(array[1].IsReviewed);
	}
}

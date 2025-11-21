using AlphaPlus.Settings.MemberInfo.API.Services;
using AlphaPlus.Settings.MemberInfo.Domain.Params;
using Fanex.Data.Repository;
using Moq;
using Xunit;

namespace AlphaPlus.Settings.MemberInfo.API.UnitTests.Services;

public class CheckUMServiceTests
{
	[Fact]
	public void CheckUM_WhenCriteriaSetsNoError_ReturnsSuccessResult_WithFormattedMessage()
	{
		// Arrange
		var refServiceMock = new Mock<CacheRefService>();
		var repoMock = new Mock<IDynamicRepository>();

		var criteria = new CheckUMCriteria(1, 10, "en");
		repoMock.Setup(r => r.Execute(It.IsAny<INonQueryCommand>(), It.IsAny<string>())).Callback<INonQueryCommand, string>((c, s) => { ((CheckUMCriteria)c).ErrCode = 0; ((CheckUMCriteria)c).ErrMsg = "OK {0}"; });
		refServiceMock.Setup(r => r.GetProductName(10)).Returns("ProductX");

		var service = new CheckUMServiceImpl(refServiceMock.Object, repoMock.Object);
		var param = new AlphaPlus.Settings.MemberInfo.Domain.Params.CheckUMParam(1, (byte)10, "en");

		// Act
		var result = service.CheckUM(param);

		// Assert
		Assert.Equal(0, result.Code);
		Assert.Equal("OK ProductX", result.Message);
	}

	[Fact]
	public void CheckUM_WhenCriteriaSetsError_Returns_ErrorCodeAndMessageFormatted()
	{
		// Arrange
		var refServiceMock = new Mock<CacheRefService>();
		var repoMock = new Mock<IDynamicRepository>();

		repoMock.Setup(r => r.Execute(It.IsAny<INonQueryCommand>(), It.IsAny<string>())).Callback<INonQueryCommand, string>((c, s) => { ((CheckUMCriteria)c).ErrCode = -1; ((CheckUMCriteria)c).ErrMsg = "ERR {0}"; });
		refServiceMock.Setup(r => r.GetProductName(20)).Returns("ProductY");

		var service = new CheckUMServiceImpl(refServiceMock.Object, repoMock.Object);
		var param = new AlphaPlus.Settings.MemberInfo.Domain.Params.CheckUMParam(1, (byte)20, "en");

		// Act
		var result = service.CheckUM(param);

		// Assert
		Assert.Equal(-1, result.Code);
		Assert.Equal("ERR ProductY", result.Message);
	}
}

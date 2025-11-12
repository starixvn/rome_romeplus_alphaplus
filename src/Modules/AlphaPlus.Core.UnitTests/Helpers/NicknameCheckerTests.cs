namespace AlphaPlus.Core.UnitTests.Helpers;

public class NicknameCheckerTests
{
    [Theory]
    [InlineData(false, null, true)]
    [InlineData(false, "", true)]
    [InlineData(false, "   ", true)]
    [InlineData(false, "nick", false)]
    [InlineData(true, null, false)]
    [InlineData(true, "nick", false)]
    public void IsOfferForceLoginFlow_Rules(bool isInternal, string nickName, bool expected)
    {
        var userSession = new Mock<IUserSession>();
        userSession.SetupGet(x => x.IsInternal).Returns(isInternal);
        userSession.SetupProperty(x => x.NickName, nickName);

        var checker = new NicknameChecker(userSession.Object);

        Assert.Equal(expected, checker.IsOfferForceLoginFlow);
        Assert.Equal(expected, checker.IsForceLoginByNickname);
    }
}



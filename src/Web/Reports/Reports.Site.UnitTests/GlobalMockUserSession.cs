using AlphaPlus.Core.Models;

namespace AlphaPlus.Reports.Site.UnitTests;

public static class GlobalMockUserSession
{
    public static Mock<IUserSession> CreateMockUserSessionDefault()
    {
        return CreateMockUserSession(123, "TestSuper", 4, "TestSite", 20, "en-US", DateTime.MinValue);
    }

    public static Mock<IUserSession> CreateMockUserSession(
        int custId,
        string username,
        byte roleId,
        string site,
        int currencyId,
        string language,
        DateTime limitedDateToShowInfo)
    {
        var mockUserSession = new Mock<IUserSession>();
        mockUserSession.Setup(x => x.CustId).Returns(custId);
        mockUserSession.Setup(x => x.UserName).Returns(username);
        mockUserSession.Setup(x => x.RoleId).Returns(roleId);
        mockUserSession.Setup(x => x.Site).Returns(site);
        mockUserSession.Setup(x => x.CurrencyId).Returns(currencyId);
        mockUserSession.Setup(x => x.Language).Returns(language);
        mockUserSession.Setup(x => x.GetProperty("CustomerDateLimit", DateTime.MinValue)).Returns(limitedDateToShowInfo);

        return mockUserSession;
    }
}
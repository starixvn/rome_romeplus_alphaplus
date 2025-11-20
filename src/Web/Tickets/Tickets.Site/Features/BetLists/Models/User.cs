using AlphaPlus.BetLists.LastBets;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Models;

public interface IUser
{
    int Id { get; }

    string Site { get; }

    int CurrencyId { get; }

    string Name { get; }

    string Language { get; }

    byte LevelId { get; }

    int AdminId { get; }

    int SubAccId { get; }

    bool IsInternal { get; }

    bool IsAdminCanViewVNIP { get; }

    int SRecommendId { get; }

    string System { get; }

    string LangKey { get; }

    bool IsEnabledJackpot { get; }

    DateTime LimitedDateToShowInfo { get; }

    bool AutoFollowBetIsDummyAgent { get; }

    string IP { get; }

    AddFilterParam GetUnsavedFilter(int custId, int subAccId);

    void SetUnsavedFilter(int custId, int subAccId, AddFilterParam value);

    bool IsAgent { get; }
}

public class User(IUserSession userSession) : IUser
{
    private const int Casino = 6;
    private const string Jackpot = "Jackpot";
    private const string CustomerDatelimit = "CustomerDateLimit";
    private const string GameEnable = "GameEnable_";
    private const string LastBetsMonitoringUnsavedFilter = "LastBetsMonitoringUnsavedFilter_{0}_{1}";

    public int Id => userSession.CustId;

    public string Site => userSession.Site;

    public int CurrencyId => userSession.CurrencyId;

    public string Name => userSession.UserName;

    public string Language => userSession.Language;

    public byte LevelId => userSession.RoleId;

    public int AdminId => userSession.AdminId;

    public int SubAccId => userSession.SubAccId;

    public bool IsInternal => userSession.IsInternal;

    public bool IsAdminCanViewVNIP => userSession.IsAdminCanViewVNIP;

    public int SRecommendId => userSession.SRecommendId;

    public string System => userSession.System;

    public string LangKey => userSession.LangKey;

    public bool IsEnabledJackpot
    {
        get
        {
            var products = GetEnableProductsByGameName(Jackpot);

            return products.Count == 1 && products.Contains(Casino)
                ? userSession.GetSyncedProduct(Casino)
                : products.Any();
        }
    }

    public DateTime LimitedDateToShowInfo => userSession.GetProperty<DateTime>(CustomerDatelimit);

    public bool AutoFollowBetIsDummyAgent => userSession.AutoFollowBetIsDummyAgent;

    internal virtual IList<int> GetEnableProductsByGameName(string gameName)
        => userSession.GetProperty<IList<int>>(GameEnable + gameName);

    public string IP => userSession.UserLoginIp;

    public AddFilterParam GetUnsavedFilter(int custId, int subAccId)
        => userSession.GetProperty<AddFilterParam>(UnsavedFilterSessionKey(custId, subAccId));

    public void SetUnsavedFilter(int custId, int subAccId, AddFilterParam value)
        => userSession.SetProperty(UnsavedFilterSessionKey(custId, subAccId), value);

    private static string UnsavedFilterSessionKey(int custId, int subAccId)
        => string.Format(LastBetsMonitoringUnsavedFilter, custId, subAccId);

    public bool IsAgent => userSession.IsAgent;
}

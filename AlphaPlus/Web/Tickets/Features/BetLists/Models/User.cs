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

    string LangKey { get; }

    bool IsEnabledJackpot { get; }

    bool HasPermissionOnProduct(int productId);

    DateTime LimitedDateToShowInfo { get; }

    bool AutoFollowBetIsDummyAgent { get; }

    string IP { get; }
}

public class User : IUser
{
    private const int Casino = 6;
    private const string Jackpot = "Jackpot";
    private const string CustomerDatelimit = "CustomerDateLimit";
    private const string GameEnable = "GameEnable_";

    private readonly IUserSession userSession;

    public User(IUserSession userSession)
        => this.userSession = userSession;

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

    public bool HasPermissionOnProduct(int productId) => UserPermission.HasPermissionOnProduct(productId);

    public DateTime LimitedDateToShowInfo => userSession.GetProperty<DateTime>(CustomerDatelimit);

    public bool AutoFollowBetIsDummyAgent => userSession.AutoFollowBetIsDummyAgent;

    internal virtual IList<int> GetEnableProductsByGameName(string gameName)
        => userSession.GetProperty<IList<int>>(GameEnable + gameName);

    public string IP => userSession.UserLoginIp;
}

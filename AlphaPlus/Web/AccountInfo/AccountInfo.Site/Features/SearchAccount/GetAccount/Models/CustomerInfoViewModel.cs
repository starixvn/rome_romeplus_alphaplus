using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Balance;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.LoginInfo;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.ViewModels;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.Transfer.Requests;
using AlphaPlus.Core.Models;
using AlphaPlus.Shared.AppResources.Common;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Models;

public class CustomerInfoViewModel : AccountInfoViewModelBase
{
    private readonly CustomerInfoQuery message;
    private readonly CustomerInfoModel customerInfo;
    private readonly bool showQRCode;
    private readonly bool canCreateAccount;

    public CustomerInfoViewModel(
        CustomerInfoQuery message,
        CustomerInfoModel customerInfo,
        IBalanceInfoViewModel balanceInfo,
        bool showQRCode,
        bool canCreateAccount)
    {
        this.message = message;
        this.customerInfo = customerInfo;
        BalanceInfo = balanceInfo;
        this.showQRCode = showQRCode;
        this.canCreateAccount = canCreateAccount;
    }

    private UserLevel SearchAccountLevel => Enumeration.FromValue<UserLevel>(RoleId);

    private UserLevel ActorLevel => Enumeration.FromValue<UserLevel>(ActorRoleId);

    public int CustId => customerInfo.CustId;

    public byte RoleId => customerInfo.RoleId;

    public byte ActorRoleId => message.ActorRoleId;

    public string UserName => customerInfo.UserName;

    public string NickName => DefaultStringValueIfNull(customerInfo.UserName2);

    public bool ShowNickName =>
        SearchAccountLevel.IsDirectDownlineOf(ActorLevel)
        && !string.IsNullOrEmpty(NickName);

    public string FullName => DefaultStringValueIfNull($"{customerInfo.FirstName} {customerInfo.LastName}");

    public string Phone => DefaultStringValueIfNull(customerInfo.Phone);

    public string Fax => DefaultStringValueIfNull(customerInfo.Fax);

    public bool Closed => customerInfo.Closed;

    public byte ClosedReason => customerInfo.ClosedReason;

    public bool Suspended => customerInfo.Suspended;

    public bool Enabled => customerInfo.Enabled;

    public bool ShowMember => SearchAccountLevel.IsMemberLevel();

    public string MemberName => DefaultStringValueIfNull(customerInfo.MemberName);

    public bool ShowAgent => ActorLevel.IsMasterOrSuperLevel()
        && SearchAccountLevel.IsDownlineOfMasterLevel();

    public int AgentId => customerInfo.AgentId;

    public string AgentName => DefaultStringValueIfNull(customerInfo.AgentName);

    public bool ShowMaster => ActorLevel.IsSuperLevel()
        && SearchAccountLevel.IsDownlineOfSuperLevel();

    public int MasterId => customerInfo.MasterId;

    public string MasterName => DefaultStringValueIfNull(customerInfo.MasterName);

    public int SuperId => customerInfo.SuperId;

    public string SuperName => DefaultStringValueIfNull(customerInfo.SuperName);

    public LoginInfoViewModel LoginInfo =>
        new LoginInfoViewModel(
            customerInfo.CreatDate,
            customerInfo.LastLogged,
            customerInfo.LastLoginIP);

    public IBalanceInfoViewModel BalanceInfo { get; }

    internal Link CustomerListLink =>
        new Link(SearchAccountLevel.IsMasterLevel() ? CommonResx.AgentList : CommonResx.MemberList,
            $"{MemberInfoUrl}CustomerList/{Enumeration.FromValue<UserLevel>((byte)(RoleId - 1)).DisplayName}?CustId={CustId}&CustName={UserName}&CustLevelId={RoleId}");

    internal Link BetListLink =>
        new Link(CommonResx.BetList, $"{BetListsUrl}MemberBetList/Index?CustID={CustId}&CustName={UserName}");

    internal Link StatemenLink
    {
        get
        {
            var actionName = SearchAccountLevel.IsMemberLevel() ? "MemberStatement" : "SMAStatement";
            var link = $"{ReportsUrl}Statement/{actionName}?CustID={CustId}&CustName={UserName}&CustLevelId={RoleId}";
            return new Link(CommonResx.Statement, link);
        }
    }

    public IEnumerable<Link> Buttons
    {
        get
        {
            var buttons = new List<Link>();

            if (!SearchAccountLevel.IsMemberLevel())
            {
                buttons.Add(CustomerListLink);
            }

            if (SearchAccountLevel.IsMemberLevel() && message.CanViewReports)
            {
                buttons.Add(BetListLink);
            }

            if (message.CanViewReports)
            {
                buttons.Add(StatemenLink);
            }

            return buttons;
        }
    }

    public bool CanViewLog => message.CanViewLog;

    public string ViewLogLink => $"{ViewLogUrl}setting/index?username={UserName}";

    public string ViewLoginIPLink => $"{OldMainUrl}_IPInfo/IpInfo.aspx";

    public bool IsShowCreateMemberByQRCode => ActorLevel.IsAgentLevel() && canCreateAccount && Core.Configurations.ConfigurationManager.GetValue<string>("QRCode:Hide") != "1";

    public string CreateMemberByQRCodeUrl => IsShowCreateMemberByQRCode ?
        $"{MemberInfoUrl}/QRCodeMemberSignIn?MemberName={UserName}" : string.Empty;

    public string ShowQRCodeUrl =>
        IsShowCreateMemberByQRCode &&
        customerInfo.LastLogged == DateTime.MinValue &&
        showQRCode ?
            $"{MemberInfoUrl}/QRCodeMemberSignIn/ShowQRCode?FirstName={customerInfo.FirstName}" +
            $"&SearchAccountId={CustId}&SearchAccountName={UserName}&Credit={BalanceInfo.GivenCredit}" +
            $"&CreditMin={BalanceInfo.GivenCreditMin}&CreditMax={BalanceInfo.GivenCreditMax}" : string.Empty;

    internal string DefaultStringValueIfNull(string value)
    {
        return string.IsNullOrEmpty(value) ? string.Empty : value;
    }

    public byte Locked => customerInfo.Locked;
}
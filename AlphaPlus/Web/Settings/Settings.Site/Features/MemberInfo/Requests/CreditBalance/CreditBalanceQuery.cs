using System.Net;
using AlphaPlus.Settings.MemberInfo.Domain.Constants;
using MediatR;
using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.CreditBalance;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Requests.CreditBalance;

public class CreditBalanceQuery : MemberInfoBaseQuery, IRequest<CreditBalanceViewModel>
{
    private string userName;

    public CreditBalanceQuery()
    {
        IsAllowedTotalBalanceTransfer = viewer.IsAllowedTotalBalanceTransfer;
        CanTransfer = viewer.CanTransfer;
        CanViewBetLists = viewer.CanViewBetLists;
        LimitedDateToShowInfo = viewer.LimitedDateToShowInfo;
    }

    public string UserName
    {
        get
        {
            return string.IsNullOrWhiteSpace(userName) ? userName : WebUtility.HtmlEncode(userName);
        }
        set
        {
            userName = value;
        }
    }

    public byte? StatusId { get; set; } = StatusIdentity.Open;

    public byte LevelType { get; set; }

    public int PageIndex { get; set; } = 1;

    public int PageSize { get; set; } = 500;

    public bool IsAllowedTotalBalanceTransfer { get; }

    public bool CanTransfer { get; }

    public bool CanViewBetLists { get; }

    public DateTime LimitedDateToShowInfo { get; }
}
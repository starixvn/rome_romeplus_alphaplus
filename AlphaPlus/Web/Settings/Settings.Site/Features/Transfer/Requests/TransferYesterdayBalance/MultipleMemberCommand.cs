using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Transfer.Constants;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferYesterdayBalance;

public class MultipleMemberCommand : TransferBaseQuery, IRequest<Result>
{
    public MultipleMemberCommand()
    {
        AllowTotalBalanceTransfer = viewer.AllowTotalBalanceTransfer;
        IsInternal = viewer.IsInternal;
        LangKey = viewer.LanguageKey;
        IP = viewer.IP;
        SubAccId = viewer.SubAccId;

        if (SubAccId > 0)
        {
            AdminId = SubAccId;
            AdminName = viewer.SubAccName;
        }
        else
        {
            AdminId = viewer.Id;
            AdminName = viewer.Name;
        }
    }

    public string CustIds { get; set; } = "0";

    public string Username { get; set; }

    public string Status { get; set; }

    public byte AutoTransfer { get; set; } = AutoTransferConstant.All;

    public bool AllYesterdayBalance { get; set; }

    public bool IsAllPages { get; set; }

    public bool AllowTotalBalanceTransfer { get; }

    public bool IsInternal { get; }

    public string LangKey { get; }

    public string IP { get; }

    public int SubAccId { get; }

    public int AdminId { get; }

    public string AdminName { get; }

    public override bool IsValid()
        => !IsInternal && ActorLevel.IsAgentLevel() && base.IsValid();
}

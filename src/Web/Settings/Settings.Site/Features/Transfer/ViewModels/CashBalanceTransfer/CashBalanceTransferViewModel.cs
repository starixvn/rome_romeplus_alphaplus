using AlphaPlus.Core.Models;
using AlphaPlus.Shared.AppResources.Settings.Transfer;

namespace AlphaPlus.Settings.Site.Features.Transfer.ViewModels.CashBalanceTransfer;

public class CashBalanceTransferViewModel
{
    public CashBalanceTransferViewModel(UserLevel actorLevel, bool isDirectMember, Settings.Transfer.API.Models.CashBalanceTransfer.CashBalanceTransfer model)
    {
        UplineLevelName = GetUserLevelDisplayName(actorLevel);
        UplineCashBalance = model.UplineCashBalance;
        CustId = model.CustId;
        UserName = model.UserName;
        CustLevelName = GetUserLevelDisplayName(Enumeration.FromValue<UserLevel>(model.RoleId));
        CashBalance = model.CashBalance;
        Outstanding = model.Outstanding;
        PendingTransfer3rd = model.PendingTransfer3rd;
        IsDirectMember = isDirectMember;
        GivenCredit = model.GivenCredit;

        TransferAmountFrom = CashBalance - Outstanding - PendingTransfer3rd;
        TransferAmountFrom = TransferAmountFrom > 0 ? TransferAmountFrom * -1 : 0;
        TransferAmountTo = UplineCashBalance > 0 ? UplineCashBalance : 0;

        DisableTransfer = TransferAmountFrom == 0 && TransferAmountTo == 0;

        if (DisableTransfer)
        {
            Hint = Resources.HintNotAvailableToTransfer;
        }
        else
        {
            string amountFrom = Format4Digits(TransferAmountFrom);
            string amountTo = Format4Digits(TransferAmountTo);

            Hint = string.Format(Resources.HintRangeTransfer, amountFrom, amountTo);
            AvailableRangeTransfer = string.Format(Resources.AvailableRangeTransfer, amountFrom, amountTo);
        }
    }

    public string UplineLevelName { get; }

    public decimal UplineCashBalance { get; }

    public string CustLevelName { get; }

    public int CustId { get; }

    public string UserName { get; }

    public decimal CashBalance { get; }

    public decimal Outstanding { get; }

    public decimal PendingTransfer3rd { get; }

    public bool IsDirectMember { get; }

    public decimal GivenCredit { get; }

    public decimal TransferAmountFrom { get; }

    public decimal TransferAmountTo { get; }

    public string Hint { get; }

    public string AvailableRangeTransfer { get; }

    public bool DisableTransfer { get; }

    private static string Format4Digits(decimal value)
        => value == 0 ? "0" : value.ToString("N4");

    private static string GetUserLevelDisplayName(UserLevel userLevel)
    {
        if (userLevel.IsSuperLevel())
        {
            return Resources.Super;
        }

        if (userLevel.IsMasterLevel())
        {
            return Resources.Master;
        }

        if (userLevel.IsAgentLevel())
        {
            return Resources.Agent;
        }

        return Resources.Member;
    }
}
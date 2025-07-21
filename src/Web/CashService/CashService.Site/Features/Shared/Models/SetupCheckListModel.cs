namespace AlphaPlus.CashService.Site.Features.Shared.Models;

public class SetupCheckListModel
{
    public bool IsCanEditCashService { get; set; }
    public bool HasDepositMethods { get; set; }
    public bool HasWithdrawalMethods { get; set; }
    public bool HasLineID { get; set; }
    public bool IsCompleteAll => HasDepositMethods && HasWithdrawalMethods && HasLineID;
} 
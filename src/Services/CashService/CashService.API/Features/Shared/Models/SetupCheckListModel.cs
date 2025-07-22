namespace AlphaPlus.CashService.API.Features.Shared.Models;

public class SetupCheckListModel
{
    public SetupCheckListModel()
    {
    }

    public SetupCheckListModel(bool hasDepositMethods, bool hasWithdrawalMethods, bool hasLineID)
    {
        HasDepositMethods = hasDepositMethods;
        HasWithdrawalMethods = hasWithdrawalMethods;
        HasLineID = hasLineID;
    }

    public bool HasDepositMethods { get; set; }
    public bool HasWithdrawalMethods { get; set; }
    public bool HasLineID { get; set; }
    public bool IsCompleteAll => HasDepositMethods && HasWithdrawalMethods && HasLineID;
} 
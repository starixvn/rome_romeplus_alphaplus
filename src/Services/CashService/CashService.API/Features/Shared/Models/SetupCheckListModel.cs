namespace AlphaPlus.CashService.API.Features.Shared.Models;

public class SetupCheckListModel
{
    public SetupCheckListModel()
    {
    }

    public SetupCheckListModel(bool completeDepositMethods, bool completeWithdrawalMethods, bool completeLineID)
    {
        CompleteDepositMethods = completeDepositMethods;
        CompleteWithdrawalMethods = completeWithdrawalMethods;
        CompleteLineID = completeLineID;
    }

    public bool CompleteDepositMethods { get; set; }
    public bool CompleteWithdrawalMethods { get; set; }
    public bool CompleteLineID { get; set; }
    public bool IsCompleteAll => CompleteDepositMethods && CompleteWithdrawalMethods && CompleteLineID;
} 
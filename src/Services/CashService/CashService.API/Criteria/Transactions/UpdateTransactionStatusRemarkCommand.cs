namespace AlphaPlus.CashService.API.Criteria.Transactions;

using Fanex.Data.Repository;

public class UpdateTransactionStatusRemarkCommand : NonQueryCommand
{
    public int TransactionId { get; set; }
    public string Status { get; set; }
    public string Remark { get; set; }

    public override string GetSettingKey() => "Age_Transaction_RevertToPendingWithRemark";

    public override bool IsValid() => TransactionId > 0;
}
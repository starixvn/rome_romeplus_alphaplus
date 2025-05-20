namespace AlphaPlus.CashService.API.Criteria.Transactions;

using System.Numerics;
using Fanex.Data.Repository;

public class ProcessTransactionCommand : NonQueryCommand
{
    public BigInteger TransId { get; set; }
    public string PaymentFrom { get; set; }
    public string Remark { get; set; }
    public byte[] Payslip { get; set; }
    public int StatusId { get; set; }
    public int ErrCode { get; set; }
    public string ErrMsg { get; set; }

    public override string GetSettingKey() => "";

    public override bool IsValid()
    {
        throw new NotImplementedException();
    }
}
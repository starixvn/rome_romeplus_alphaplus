namespace AlphaPlus.CashService.API.Features.PendingDeposit.Criteria;

using System.Numerics;
using Fanex.Data.Repository;

public class ProcessDepositCommand : NonQueryCommand
{
    public BigInteger TransId { get; set; }

    public decimal Amount { get; set; }

    public string Remark { get; set; }

    public int StatusId { get; set; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public override string GetSettingKey() => "";

    public override bool IsValid()
    {
        throw new NotImplementedException();
    }
}
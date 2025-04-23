using Fanex.Data.Repository;

namespace AlphaPlus.Identity.API.Features.Auth
{
    public class CheckAllowTotalBalanceTransferCommand(int custId, string superIDsTransferTotal) : INonQueryCommand
    {
        public int CustId { get; } = custId;

        public string SuperIDsTransferTotal { get; } = superIDsTransferTotal;

        public bool IsAllowed { get; set; }

        public string GetSettingKey() => "Age_Transfer_CheckTotalBalanceTransfer";

        public bool IsValid() => CustId > 0;
    }
}
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Criteria.Transfer
{
    public class GetTransferOptionCriteria : NonQueryCommand
    {
        public GetTransferOptionCriteria(int custId)
        {
            CustId = custId;
            TransferOption = string.Empty;
        }

        public int CustId { get; }

        public string TransferOption { get; set; }

        public override string GetSettingKey() => "Acc_CustomerSelTransferOption";

        public override bool IsValid() => CustId > 0;
    }
}

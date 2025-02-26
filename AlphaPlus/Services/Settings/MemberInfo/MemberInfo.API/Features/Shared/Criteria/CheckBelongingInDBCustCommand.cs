using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Features.Shared.Criteria
{
    public class CheckBelongingInDBCustCommand : NonQueryCommand
    {
        public CheckBelongingInDBCustCommand(
            int viewerId,
            int custId,
            int subAccId,
            long transId)
        {
            SessionCustId = viewerId;
            CustId = custId;
            SubAccountId = subAccId;
            TransId = transId;
        }

        public int SessionCustId { get; }

        public int CustId { get; }

        public int SubAccountId { get; }

        public long TransId { get; }

        public bool IsActive { get; set; }

        public string Username { get; set; }

        public byte RoleId { get; set; }

        public override string GetSettingKey()
            => "Age_CustomerSelCheckBelonging_DBCUST";

        public override bool IsValid()
            => SessionCustId > 0
            && (CustId > 0 || TransId > 0)
            && SubAccountId >= 0;
    }
}

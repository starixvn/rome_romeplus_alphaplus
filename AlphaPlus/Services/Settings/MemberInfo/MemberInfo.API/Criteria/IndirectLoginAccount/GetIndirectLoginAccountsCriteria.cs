namespace AlphaPlus.Settings.MemberInfo.API.Criteria.IndirectLoginAccount
{
    using Fanex.Data.Repository;

    public class GetIndirectLoginAccountsCriteria : CriteriaBase
    {
        public GetIndirectLoginAccountsCriteria()
        {
        }

        public GetIndirectLoginAccountsCriteria(int custId, int subAccId)
        {
            CustId = custId;
            SubAccId = subAccId;
        }

        public GetIndirectLoginAccountsCriteria(
            int custId,
            int subAccId,
            int indirectCustId,
            int indirectSubAccId)
        {
            CustId = custId;
            SubAccId = subAccId;
            CustIDInDirect = indirectCustId;
            SubAccIDIndirect = indirectSubAccId;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public int? CustIDInDirect { get; }

        public int? SubAccIDIndirect { get; }

        public override string GetSettingKey()
            => "Age_CustomerIndirectLoginAccountSel";

        public override bool IsValid()
            => CustId > 0 && SubAccId >= 0;
    }
}

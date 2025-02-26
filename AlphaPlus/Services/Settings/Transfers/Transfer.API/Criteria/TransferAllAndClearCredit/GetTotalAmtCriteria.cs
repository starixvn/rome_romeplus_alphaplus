using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.Criteria.TransferAllAndClearCredit
{
    public class GetTotalAmtCriteria : CriteriaBase
    {
        public GetTotalAmtCriteria(int downlineId, byte downlineRoleId)
        {
            CustId = downlineId;
            RoleId = downlineRoleId;
        }

        public int CustId { get; set; }

        public byte RoleId { get; set; }

        public override string GetSettingKey()
            => "SiteDB_Age_Customer_TotalAmount_Sel";

        public override bool IsValid()
            => CustId > 0 && RoleId > 0;
    }
}

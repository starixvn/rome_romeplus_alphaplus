using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Features.QRCodeMemberSignIn.Criteria
{
    public class SelectFirstLoginByQRCodeCriteria : CriteriaBase
    {
        public SelectFirstLoginByQRCodeCriteria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public override string GetSettingKey() => "Age_CheckQRFirstLogin";

        public override bool IsValid() => CustId > 0;
    }
}
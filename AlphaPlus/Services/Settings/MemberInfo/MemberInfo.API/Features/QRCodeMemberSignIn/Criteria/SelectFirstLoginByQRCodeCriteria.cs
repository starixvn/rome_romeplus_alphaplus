namespace AlphaPlus.Settings.MemberInfo.API.Features.QRCodeMemberSignIn.Criteria
{
    using Fanex.Data.Repository;

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
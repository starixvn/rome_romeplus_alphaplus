namespace AlphaPlus.Settings.MemberInfo.API.Features.CreateAccount.Criteria
{
    using Fanex.Data.Repository;
  
    public class ProductSettingInfoCriteria : CriteriaBase
    {
        public byte CreationStatus { get; set; }
        
        public int CustId { get; set; }
        
        public override string GetSettingKey() => "Acc_LiteMode_CustSetting_ProductListInfoSel";

        public override bool IsValid() => CustId > 0;
    }
}
using Fanex.Data.Repository;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.GivenCredit.Update
{
    public class GetUplineByCustIdCommand(int custId) : NonQueryCommand
    {
        public int CustId { get; } = custId;

        public int UplineId { get; set; }

        public override string GetSettingKey()
            => "Age_CustSetting_GetUplineByCustID";

        public override bool IsValid() => CustId > 0;
    }
}
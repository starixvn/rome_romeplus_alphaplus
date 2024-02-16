namespace Alpha.Main.Site.Features.Default.MenuBar
{
    using Fanex.Data.Repository;

    public class GetIsAllDayBalTransferCriteria(int custId) : CriteriaBase
    {
        public int CustId { get; } = custId;

        public override string GetSettingKey()
            => "Age_GetIsAllDayBalTransfer";

        public override bool IsValid()
            => CustId > 0;
    }
}

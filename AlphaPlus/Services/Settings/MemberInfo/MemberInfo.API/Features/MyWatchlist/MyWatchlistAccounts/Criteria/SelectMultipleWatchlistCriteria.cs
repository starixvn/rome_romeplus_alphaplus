namespace AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.MyWatchlistAccounts.Criteria
{
    using Fanex.Data.Repository;

    public class SelectMultipleWatchlistCriteria : CriteriaBase 
    {
        public SelectMultipleWatchlistCriteria(string custList)
        {
            CustList = custList;
        }

        public string CustList { get; }

        public override string GetSettingKey() => "Acc_WLMonitored_CheckExistSettingByCust";

        public override bool IsValid() => !string.IsNullOrWhiteSpace(CustList);
    }
}

using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.Criteria.AutoTransfer
{
    public class GetHistoryTransfersCriteria : CriteriaBase
    {
        public int FromCustId { get; set; }

        public string Language { get; set; } = string.Empty;

        public string Searchname { get; set; } = string.Empty;

        public string ViewStatus { get; set; } = string.Empty;

        public int SortBy { get; set; }

        public int SortType { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalRow { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public override bool IsValid() => !string.IsNullOrWhiteSpace(ViewStatus);

        public override string GetSettingKey() => "Age_ClearBalanceCust_ProcessHistorySel";
    }
}

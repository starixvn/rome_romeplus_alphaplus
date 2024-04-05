namespace AlphaPlus.Features.Messages.Criteria.ToastMessage
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetToastMessagesCriteria : CriteriaBase
    {
        public GetToastMessagesCriteria(int custId, int subAccId, string language, int lastToastId)
        {
            CustId = custId;
            Language = language;
            LastToastId = lastToastId;
            SubAccId = subAccId;
        }

        [SpParam(SqlParamName = "TargetId")]
        public int CustId { get; }

        public int SubAccId { get; }

        public string Language { get; }

        public int LastToastId { get; }

        public override string GetSettingKey() => "Age_ToastMessage_Sel";

        public override bool IsValid() => CustId > 0;
    }
}

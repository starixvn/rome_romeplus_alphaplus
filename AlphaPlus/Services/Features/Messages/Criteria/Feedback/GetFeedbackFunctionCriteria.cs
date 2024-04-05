namespace AlphaPlus.Features.Messages.Criteria.Feedback
{
    using Fanex.Data.Repository;

    public class GetFeedbackFunctionCriteria : CriteriaBase
    {
        public GetFeedbackFunctionCriteria(int functionId)
            => FuncId = functionId;

        public int FuncId { get; }

        public override string GetSettingKey()
            => "Age_FeedbackFunction_Sel";

        public override bool IsValid()
            => FuncId > 0;
    }
}

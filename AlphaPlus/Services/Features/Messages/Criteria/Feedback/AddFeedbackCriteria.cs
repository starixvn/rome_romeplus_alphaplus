namespace AlphaPlus.Features.Messages.Criteria.Feedback
{
    using Fanex.Data.Repository;

    public class AddFeedbackCriteria : NonQueryCommand
    {
        private const int MaxCommentLength = 4000;

        public AddFeedbackCriteria(
            int custId,
            int subAccId,
            string comment,
            byte rate,
            int? funcId,
            string xmlfb)
        {
            CustId = custId;
            SubAccId = subAccId;
            Comment = comment ?? string.Empty;
            Rate = rate;
            FuncId = funcId;
            XMLfb = xmlfb;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public string Comment { get; }

        public byte Rate { get; }

        public int? FuncId { get; }

        public string XMLfb { get; }

        public override string GetSettingKey()
            => "Age_CustFeedbackIns";

        public override bool IsValid()
            => Rate != 0 ||
            (!string.IsNullOrWhiteSpace(Comment) && Comment.Length <= MaxCommentLength)
            || !string.IsNullOrWhiteSpace(XMLfb);
    }
}

using Fanex.Data.Repository;

namespace AlphaPlus.Features.Messages.API.Criteria.Feedback
{
    public class AddFeedbackCriteria : NonQueryCommand
    {
        private const int MaxCommentLength = 4000;

        public AddFeedbackCriteria(
            int custId,
            int subAccId,
            string comment,
            byte rate,
            int? funcId,
            string xmlfb,
            int statusId,
            string extraInfo)
        {
            CustId = custId;
            SubAccId = subAccId;
            Comment = comment ?? string.Empty;
            Rate = rate;
            FuncId = funcId;
            XMLfb = xmlfb;
            StatusId = statusId;
            ExtraInfo = extraInfo;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public string Comment { get; }

        public byte Rate { get; }

        public int? FuncId { get; }

        public string XMLfb { get; }

        public int StatusId { get; }

        public string ExtraInfo { get; }

        public override string GetSettingKey() => "Age_CustFeedbackIns";

        public override bool IsValid()
            => Rate != 0 ||
            (!string.IsNullOrWhiteSpace(Comment) && Comment.Length <= MaxCommentLength)
            || !string.IsNullOrWhiteSpace(XMLfb);
    }
}

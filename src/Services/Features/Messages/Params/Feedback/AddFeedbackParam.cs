namespace AlphaPlus.Features.Messages.API.Params.Feedback
{
    public class AddFeedbackParam
    {
        public AddFeedbackParam(
            int custId,
            int subAccId,
            string comment,
            byte rate,
            int? functionId,
            string xmlFeedback,
            string extraInfo)
        {
            CustId = custId;
            SubAccountId = subAccId;
            Comment = comment;
            Rate = rate;
            FunctionId = functionId;
            XMLFeedback = xmlFeedback;
            ExtraInfo = extraInfo;
        }

        public int CustId { get; }

        public int SubAccountId { get; }

        public string Comment { get; }

        public byte Rate { get; }

        public int? FunctionId { get; }

        public string XMLFeedback { get; }

        public string ExtraInfo { get; }
    }
}

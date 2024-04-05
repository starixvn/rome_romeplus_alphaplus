namespace AlphaPlus.Features.Messages.Params.Feedback
{
    public class AddFeedbackParam
    {
        public AddFeedbackParam(
            int custId,
            int subAccId,
            string comment,
            byte rate,
            int? functionId,
            string xmlFeedback)
        {
            CustId = custId;
            SubAccountId = subAccId;
            Comment = comment;
            Rate = rate;
            FunctionId = functionId;
            XMLFeedback = xmlFeedback;
        }

        public int CustId { get; }

        public int SubAccountId { get; }

        public string Comment { get; }

        public byte Rate { get; }

        public int? FunctionId { get; }

        public string XMLFeedback { get; }
    }
}

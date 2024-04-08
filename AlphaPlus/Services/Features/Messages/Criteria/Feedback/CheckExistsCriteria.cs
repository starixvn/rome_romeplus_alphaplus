namespace AlphaPlus.Features.Messages.API.Criteria.Feedback
{
    using Fanex.Data.Repository;

    public class CheckExistsCriteria : NonQueryCommand
    {
        public CheckExistsCriteria(int custId, int subAccId, int functionId)
        {
            CustId = custId;
            SubAccId = subAccId;
            FuncId = functionId;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public int FuncId { get; }

        public bool IsExists { get; set; }

        public override string GetSettingKey()
            => "Age_CustFeedbackCheckExists";

        public override bool IsValid()
            => CustId > 0 && SubAccId >= 0 && FuncId > 0;
    }
}

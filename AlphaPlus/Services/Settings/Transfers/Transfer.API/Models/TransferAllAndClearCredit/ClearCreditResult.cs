namespace AlphaPlus.Settings.Transfer.API.Models.TransferAllAndClearCredit
{
    public class ClearCreditResult
    {
        public ClearCreditResult(IEnumerable<ClearCreditStep> steps, int errCode, string errMsg, IEnumerable<string> conditions)
        {
            Steps = steps;
            ErrCode = errCode;
            ErrMsg = errMsg;
            Conditions = conditions;
        }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public IEnumerable<ClearCreditStep> Steps { get; }

        public IEnumerable<string> Conditions { get; }
    }
}

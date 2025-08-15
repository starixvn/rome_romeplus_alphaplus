using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.GivenCredit.Update
{
    public class UpdateMasterGivenCreditCommand : NonQueryCommand
    {
        public UpdateMasterGivenCreditCommand(
            int searchAccountId,
            decimal creditAmount,
            int actorId,
            int subAccountId,
            string language,
            string ip)
        {
            SearchAccountId = searchAccountId;
            CreditAmount = creditAmount;
            ActorId = actorId;
            SubAccountId = subAccountId;
            Language = language;
            IP = ip;
        }

        [SpParam(SqlParamName = "CustId")]
        public int SearchAccountId { get; }

        [SpParam(SqlParamName = "CreditAmt")]
        public decimal CreditAmount { get; }

        [SpParam(SqlParamName = "UserId")]
        public int ActorId { get; }

        [SpParam(SqlParamName = "SubAccId")]
        public int SubAccountId { get; }

        public string Language { get; }

        public string IP { get; }

        public bool CreditProblem { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Age_CustomerUpdCredit_SAg";

        public override bool IsValid() => SearchAccountId > 0 && ActorId > 0 && CreditAmount >= 0;
    }
}
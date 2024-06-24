using AlphaPlus.Core.Models;
using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Site.Features.AccountInfo.Criteria
{
    public class UpdateSMAGivenCreditCommand : NonQueryCommand
    {
        private static readonly IDictionary<UserLevel, string> settingKeys =
                    new Dictionary<UserLevel, string>
                    {
                {UserLevel.Master, "Age_CustomerUpdCredit_SAg" },
                {UserLevel.Agent, "Age_CustomerUpdCredit_MAg" },
                {UserLevel.Member, "Age_CustomerUpdCredit_Ag" }
                    };

        public UpdateSMAGivenCreditCommand(
            int searchAccountId,
            byte searchAccountRoleId,
            decimal creditAmount,
            int actorId,
            int subAccountId,
            string language,
            string ip)
        {
            SearchAccountId = searchAccountId;
            SearchAccountRoleId = searchAccountRoleId;
            CreditAmount = creditAmount;
            ActorId = actorId;
            SubAccountId = subAccountId;
            Language = language;
            IP = ip;
        }

        [SpParam(SqlParamName = "CustId")]
        public int SearchAccountId { get; }

        [SpParam(Ignored = true)]
        public byte SearchAccountRoleId { get; }

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

        public override string GetSettingKey() => settingKeys[SearchAccountLevel];

        public override bool IsValid() => SearchAccountId > 0 && ActorId > 0 && CreditAmount >= 0;

        private UserLevel SearchAccountLevel => Enumeration.FromValue<UserLevel>(SearchAccountRoleId);
    }
}
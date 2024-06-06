using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.AccountInfo.Models;
using AlphaPlus.Settings.Site.Features.AccountInfo.Requests;
using AlphaPlus.Settings.Site.Features.Shared.Helpers;

namespace AlphaPlus.Settings.Site.Features.AccountInfo.ViewModels
{
    public class GivenCreditViewModel
    {
        private readonly GivenCreditQuery message;

        public GivenCreditViewModel(
            GivenCreditQuery message,
            CreditInfo creditInfo,
            decimal memberCreditMin)
        {
            this.message = message;
            CurrentGivenCredit = creditInfo.Credit;
            var level = Enumeration.FromValue<UserLevel>(message.SearchAccountRoleId);
            if (level.IsMasterLevel())
            {
                GivenCreditMax = NumberHelpers.RoundDown(creditInfo.MasterCreditMax);
                GivenCreditMin = NumberHelpers.RoundUp(creditInfo.MasterCreditMin);
            }
            else if (level.IsAgentLevel())
            {
                GivenCreditMax = NumberHelpers.RoundDown(creditInfo.AgentCreditMax);
                GivenCreditMin = NumberHelpers.RoundUp(creditInfo.AgentCreditMin);
            }
            else
            {
                GivenCreditMax = creditInfo.MemMaxCreditMax > 0
                    ? NumberHelpers.RoundDown(Math.Min(creditInfo.MemMaxCreditMax, creditInfo.AvailableCredit))
                    : NumberHelpers.RoundDown(creditInfo.AvailableCredit);

                GivenCreditMin = NumberHelpers.RoundUp(memberCreditMin);
                MemberMaxCredit = creditInfo.MemMaxCreditMax;
                AvailableCredit = creditInfo.AvailableCredit;
            }
        }

        public int SearchAccountId => message.SearchAccountId;

        public string SearchAccountName => message.SearchAccountName;

        public byte SearchAccountRoleId => message.SearchAccountRoleId;

        public decimal CurrentGivenCredit { get; }

        public decimal GivenCreditMin { get; }

        public decimal GivenCreditMax { get; }

        public decimal MemberMaxCredit { get; }

        public decimal AvailableCredit { get; }
    }
}
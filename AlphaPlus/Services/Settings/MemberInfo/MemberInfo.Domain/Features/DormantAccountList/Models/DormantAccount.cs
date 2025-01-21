using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
using AlphaPlus.Settings.MemberInfo.Domain.Constants;
using AlphaPlus.Core.Models;
using AlphaPlus.Shared.AppResources.Common;

namespace AlphaPlus.Settings.MemberInfo.Domain.Features.DormantAccountList.Models
{
    public class DormantAccount
    {
        public DormantAccount() : this(null, null, 0)
        {
        }

        public DormantAccount(
            decimal? toBeClearedGivenCredit,
            decimal? clearedGivenCredit,
            decimal yesterdayBalance)
        {
            ToBeClearedGivenCredit = toBeClearedGivenCredit;
            ClearedGivenCredit = clearedGivenCredit;
            YesterdayBalance = yesterdayBalance;
        }

        public DormantAccount(DormantAccountEntity entity, decimal yesterdayBalance)
        {
            Username = entity.Username ?? string.Empty;
            RoleName = entity.AgentId == DirectMemberAgentIds.Super || entity.AgentId == DirectMemberAgentIds.Master
                ? CommonResx.DirectMember
                : Resources.ResourceManager.GetString(Enumeration.FromValue<UserLevel>(entity.RoleId).DisplayName) ?? string.Empty;
            Status = GetStatus(entity.StatusId, entity.IsWarningTicket);
            ArchiveStage = GetArchiveStage(entity.StageId);
            ToBeClearedGivenCredit = entity.ToBeClearedGivenCredit;
            ClearedGivenCredit = entity.ClearedGivenCredit;
            YesterdayBalance = yesterdayBalance;
            LastLogged = entity.LastLogged;
            ClosedDate = entity.ClosedDate;
            DisabledDate = entity.DisabledDate;
            SettingDeletedDate = entity.SettingDeletedDate;
            AccountDeletedDate = entity.AccountDeletedDate;
            IsWarningTicket = entity.IsWarningTicket;
            IsSummaryRow = entity.RowNumber == 0;
        }

        public string Username { get; } = string.Empty;

        public string RoleName { get; } = string.Empty;

        public string Status { get; } = string.Empty;

        public string ArchiveStage { get; } = string.Empty;

        public decimal? ToBeClearedGivenCredit { get; }

        public decimal? ClearedGivenCredit { get; }

        public decimal YesterdayBalance { get; }

        public DateTime? LastLogged { get; }

        public DateTime? ClosedDate { get; }

        public DateTime? DisabledDate { get; }

        public DateTime? SettingDeletedDate { get; }

        public DateTime? AccountDeletedDate { get; }

        public bool IsWarningTicket { get; }

        public bool IsRedDateTime
            => LastLogged.HasValue
            && ClosedDate.HasValue
            && LastLogged.Value > ClosedDate.Value;

        public bool IsSummaryRow { get; }

        private static string GetStatus(byte statusId, bool isWarningTicket)
        {
            var accountStatusList = new Dictionary<byte, string>
            {
                { StatusIdentity.Open, Resources.Open },
                { StatusIdentity.Suspended, Resources.Suspended },
                { StatusIdentity.Closed, Resources.Closed },
                { StatusIdentity.Disabled, Resources.Disabled },
                { StatusIdentity.Deleted, Resources.Deleted }
            };
            var status = accountStatusList.ContainsKey(statusId) ? accountStatusList[statusId] : string.Empty;

            return isWarningTicket ? $"{status} (*)" : status;
        }

        private static string GetArchiveStage(byte stageId)
        {
            const int CloseAccount = 1;
            const int DisableAccount = 2;
            const int DeleteSettings = 3;
            const int DeleteAccount = 4;

            var archiveStageList = new Dictionary<byte, string>
            {
                { CloseAccount,  Resources.CloseAccount},
                { DisableAccount,  Resources.DisableAccount},
                { DeleteSettings,  Resources.DeleteSettings},
                { DeleteAccount,  Resources.DeleteAccount}
            };

            return archiveStageList.ContainsKey(stageId) ? archiveStageList[stageId] : string.Empty;
        }
    }
}
namespace AlphaPlus.Settings.MemberInfo.Domain.Features.DormantAccountList.Models
{
    using System;
    using System.Collections.Generic;
    using AlphaPlus.Settings.MemberInfo.Domain.AppResources;
    using AlphaPlus.Settings.MemberInfo.Domain.Constants;
    using AlphaPlus.Settings.MemberInfo.Domain.Enumerations;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;

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
            Username = entity.Username;
            RoleName = Enumeration.FromValue<UserLevel>(entity.RoleId).DisplayNameViaResource;
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

        public string Username { get; }

        public string RoleName { get; }

        public string Status { get; }

        public string ArchiveStage { get; }

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
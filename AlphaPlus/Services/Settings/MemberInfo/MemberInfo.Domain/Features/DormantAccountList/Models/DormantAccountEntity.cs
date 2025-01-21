namespace AlphaPlus.Settings.MemberInfo.Domain.Features.DormantAccountList.Models
{
    public class DormantAccountEntity
    {
        public DormantAccountEntity()
        {
        }

        public DormantAccountEntity(
            int custId,
            string username,
            byte roleId,
            byte statusId,
            byte stageId,
            decimal? toBeClearedGivenCredit,
            decimal? clearedGivenCredit,
            DateTime? lastLogged,
            DateTime? closedDate,
            DateTime? disabledDate,
            DateTime? settingDeletedDate,
            DateTime? accountDeletedDate,
            bool isWarningTicket,
            int rowNumber,
            int agentId)
        {
            CustId = custId;
            Username = username;
            RoleId = roleId;
            StatusId = statusId;
            StageId = stageId;
            ToBeClearedGivenCredit = toBeClearedGivenCredit;
            ClearedGivenCredit = clearedGivenCredit;
            LastLogged = lastLogged;
            ClosedDate = closedDate;
            DisabledDate = disabledDate;
            SettingDeletedDate = settingDeletedDate;
            AccountDeletedDate = accountDeletedDate;
            IsWarningTicket = isWarningTicket;
            RowNumber = rowNumber;
            AgentId = agentId;
        }

        public int CustId { get; set; }

        public string Username { get; set; }

        public byte RoleId { get; set; }

        public byte StatusId { get; set; }

        public byte StageId { get; set; }

        public decimal? ToBeClearedGivenCredit { get; set; }

        public decimal? ClearedGivenCredit { get; set; }

        public DateTime? LastLogged { get; set; }

        public DateTime? ClosedDate { get; set; }

        public DateTime? DisabledDate { get; set; }

        public DateTime? SettingDeletedDate { get; set; }

        public DateTime? AccountDeletedDate { get; set; }

        public bool IsWarningTicket { get; set; }

        public int RowNumber { get; set; }

        public int AgentId { get; set; }
    }
}

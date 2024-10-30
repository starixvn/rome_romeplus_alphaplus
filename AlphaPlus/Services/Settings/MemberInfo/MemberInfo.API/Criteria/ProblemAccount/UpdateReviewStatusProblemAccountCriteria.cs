namespace AlphaPlus.Settings.MemberInfo.API.Criteria.ProblemAccount
{
    using AlphaPlus.Core.Models;
    using Fanex.Data.Repository;

    public class UpdateReviewStatusProblemAccountCriteria : NonQueryCommand
    {
        public UpdateReviewStatusProblemAccountCriteria(
            int custId,
            int subAccId,
            byte roleId,
            string customerList,
            bool isAffectedAllDownlines,
            string language,
            string ip)
        {
            UserId = custId;
            SubAccId = subAccId;
            RoleId = roleId;
            CustomerList = customerList;
            IsAffectedAllDownlines = isAffectedAllDownlines;
            Language = language;
            LoginIp = ip;
        }

        public int UserId { get; }

        public int SubAccId { get; }

        public byte RoleId { get; }

        public string CustomerList { get; }

        public bool IsAffectedAllDownlines { get; }

        public string Language { get; }

        public string LoginIp { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "Age_ProblemAccount_ReviewStatusUpd";

        public override bool IsValid()
            => UserId > 0 &&
            SubAccId >= 0 &&
            RoleId <= UserLevel.Master.Value &&
            !string.IsNullOrEmpty(CustomerList);
    }
}

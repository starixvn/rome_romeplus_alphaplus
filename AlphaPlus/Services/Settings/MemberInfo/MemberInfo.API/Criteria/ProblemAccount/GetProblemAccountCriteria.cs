namespace AlphaPlus.Settings.MemberInfo.API.Criteria.ProblemAccount
{
    using System;
    using AlphaPlus.Core.Models;
    using Fanex.Data.Repository;

    public class GetProblemAccountCriteria : CriteriaBase
    {
        public GetProblemAccountCriteria(
            int custId,
            int subAccId,
            string uplineUserName,
            byte searchRoleId,
            string searchUserName,
            DateTime fromDate,
            DateTime toDate,
            int reviewStatusId)
        {
            CustId = custId;
            SubAccId = subAccId;
            UplineUserName = uplineUserName;
            SearchRoleId = searchRoleId;
            SearchUserName = searchUserName;
            FromDate = fromDate;
            ToDate = toDate;
            ReviewStatusId = reviewStatusId;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public string UplineUserName { get; }

        public byte SearchRoleId { get; }

        public string SearchUserName { get; }

        public int ReviewStatusId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public override string GetSettingKey()
            => "Age_ProblemAccount_CustomerList";

        public override bool IsValid()
            => CustId > 0 &&
            SubAccId >= 0 &&
            SearchRoleId <= UserLevel.Master.Value;            
    }
}

namespace AlphaPlus.Settings.MemberInfo.Domain.Params.ProblemAccount
{
    using System;

    public class GetProblemAccountParam
    {
        public int CustId { get; set; }

        public int SubAccId { get; set; }

        public string UplineUserName { get; set; }

        public byte SearchRoleId { get; set; }

        public string SearchUserName { get; set; }

        public int ReviewStatusId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}

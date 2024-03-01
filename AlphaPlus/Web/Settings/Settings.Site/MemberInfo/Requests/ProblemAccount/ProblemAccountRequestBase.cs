namespace AlphaPlus.Settings.Site.Requests.ProblemAccount
{
    using System;
    using System.Web.Mvc;
    using AlphaPlus.Settings.MemberInfo.Domain.Constants;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;

    public class ProblemAccountRequestBase : MemberInfoBaseQuery
    {
        public ProblemAccountRequestBase(IUser viewer) : base(viewer)
        {
            FromDate = DateTime.Now.AddDays(-13);
            ToDate = DateTime.Now;
            UserSelectedReviewedStatusId = ReviewStatusConstant.All;
            SearchRoleId = CustLevel.GetDirectDownLineLevel().Value;
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int UserSelectedReviewedStatusId { get; set; }

        public int UplineId { get; set; }

        [AllowHtml]
        public string UplineName { get; set; }

        public byte SearchRoleId { get; set; }

        [AllowHtml]
        public string UserName { get; set; }
    }
}
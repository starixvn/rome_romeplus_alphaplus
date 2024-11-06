using AlphaPlus.Settings.MemberInfo.Domain.Constants;
using AlphaPlus.Settings.MemberInfo.Domain.Models;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Requests.ProblemAccount
{
    public class ProblemAccountRequestBase : MemberInfoBaseQuery
    {
        public ProblemAccountRequestBase() : this(null) { }

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

        public string UplineName { get; set; }

        public byte SearchRoleId { get; set; }

        public string UserName { get; set; }
    }
}
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Shared;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberBettingBehavior.BettingStatistic
{
    public class MemberBettingStatisticBaseQuery : BaseQuery
    {
        public int MemberCustId { get; set; }

        public string UserName { get; set; }

        public int DateRange { get; set; }

        public DateTime EndDate => DateTime.Now.AddDays(-2);

        public DateTime StartDate
        {
            get
            {
                var range = DateRange > 90 ? 90 : DateRange;
                return EndDate.AddDays(-range) > viewer.LimitedDateToShowInfo ? EndDate.AddDays(-range) : viewer.LimitedDateToShowInfo;
            }
        }
    }

    public class MemberBettingStatisticQuery : MemberBettingStatisticBaseQuery, IRequest<MemberBettingStatistic> { }

    public class MemberBettingBehaviorExcelQuery : MemberBettingStatisticBaseQuery, IRequest<ExcelFileViewModel> { }
}
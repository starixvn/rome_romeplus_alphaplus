using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.MemberBettingBehavior.BettingStatistic;
using AlphaPlus.Settings.Site.Features.MemberBettingBehavior.Profile;
using AlphaPlus.Settings.Site.Features.MemberInfo.Handlers;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberBettingBehavior
{
    public class MemberBettingBehaviorHandler :
        ExcelBaseHandler,
        IRequestHandler<MemberProfileQuery, CustomerProfile>,
        IRequestHandler<MemberBettingStatisticQuery, MemberBettingStatistic>,
        IRequestHandler<MemberBettingBehaviorExcelQuery, ExcelFileViewModel>
    {
        public Task<CustomerProfile> Handle(MemberProfileQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<MemberBettingStatistic> Handle(MemberBettingStatisticQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ExcelFileViewModel> Handle(MemberBettingBehaviorExcelQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override string GetExcelTemplateDir() => "~/App_Data/ExcelTemplates/MemberBettingBehavior";
    }
}
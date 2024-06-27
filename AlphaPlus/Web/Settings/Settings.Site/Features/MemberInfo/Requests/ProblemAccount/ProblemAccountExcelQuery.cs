namespace AlphaPlus.Settings.Site.Features.MemberInfo.Requests.ProblemAccount
{
    using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels;
    using AlphaPlus.Settings.MemberInfo.Domain.Enumerations;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class ProblemAccountExcelQuery: ProblemAccountRequestBase, IRequest<ExcelFileViewModel>
    {
        public ProblemAccountExcelQuery() : this(null){}
        
        public ProblemAccountExcelQuery(IUser viewer)
            : base(viewer)
        {
        }

        public UserLevel SearchRole => Enumeration.FromValue<UserLevel>(SearchRoleId);

        public bool IsViewDirectDownLine => CustLevel.GetDirectDownLineLevel().Is(SearchRole);
    }
}
using AlphaPlus.Settings.Site.Features.MemberInfo.Features.DormantAccountList.ViewModels;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using MediatR;
using AlphaPlus.Settings.Site.Features.MemberInfo.Models;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.DormantAccountList.Requests
{
    public class DormantAccountQuery : MemberInfoBaseQuery, IRequest<DormantAccountViewModel>
    {
        public DormantAccountQuery() : this(null) { }

        public DormantAccountQuery(IUser viewer) : base(viewer)
        {
            viewer = viewer ?? new User(UserSession.Current);
            LimitedDateToShowInfo = viewer.LimitedDateToShowInfo;
        }

        public string Username { get; set; }

        public byte LevelId { get; set; }

        public byte StatusId { get; set; }

        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 500;

        public DateTime LimitedDateToShowInfo { get; }
    }
}
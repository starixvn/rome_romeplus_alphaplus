using AlphaPlus.Settings.Site.Features.MemberInfo.Features.DormantAccountList.ViewModels;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.DormantAccountList.Requests
{
    public class DormantAccountQuery : MemberInfoBaseQuery, IRequest<DormantAccountViewModel>
    {
        public string Username { get; set; }

        public byte LevelId { get; set; }

        public byte StatusId { get; set; }

        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 500;
    }
}
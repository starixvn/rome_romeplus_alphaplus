namespace AlphaPlus.Settings.Site.Requests.WinLossLimit
{
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;
    using ViewModels.WinLossLimit;

    public class WinLossLimitQuery : MemberInfoBaseQuery, IRequest<WinLossLimitViewModel>
    {
        public WinLossLimitQuery(IUser viewer) : base(viewer)
        {
            PageSize = 100;
            PageIndex = 1;
        }

        public int BetType { get; set; }

        public int SportType { get; set; }

        public int MasterId { get; set; }

        public int AgentId { get; set; }

        public string Username { get; set; }

        public string PlayerId { get; set; }

        public int UserSelectedProductId { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }
    }
}
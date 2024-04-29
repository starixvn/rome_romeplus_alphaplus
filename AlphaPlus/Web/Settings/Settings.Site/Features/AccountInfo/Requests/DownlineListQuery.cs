using AlphaPlus.Settings.Site.Features.Shared.Requests;

namespace AlphaPlus.Settings.Site.Features.AccountInfo.Requests
{
    public class DownlineListQuery : BaseQuery
    {
        public int CustId { get; set; }

        public int PageIndex { get; set; }

        public byte RoleId { get; set; }

        public byte StatusId { get; set; }

        public int TotalPages { get; set; }
    }
}
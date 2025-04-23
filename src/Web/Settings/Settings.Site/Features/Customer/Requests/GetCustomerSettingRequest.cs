using AlphaPlus.Settings.Site.Features.Shared;

namespace AlphaPlus.Settings.Site.Features.Customer.Requests
{
    public class GetCustomerSettingRequest : BaseQuery
    {
        public int ProductGroupId { get; set; } = 1;

        public bool IsGetUpLine { get; set; } = false;

        public byte RoleId { get; set; }

        public byte UplineRoleId { get; set; }

        public string SiteLabel { get; set; }

        public int AgentId { get; set; } = 0;
    }
}
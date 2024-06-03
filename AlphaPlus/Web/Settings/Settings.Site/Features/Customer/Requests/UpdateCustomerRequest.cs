using AlphaPlus.Settings.Site.Features.Shared;
using Fanex.Customer.Client.Models;

namespace AlphaPlus.Settings.Site.Features.Customer.Requests
{
    public class UpdateCustomerRequest : BaseQuery
    {
        public GeneralInfo GeneralInfo { get; set; }

        public string ProductGroups { get; set; }

        public byte RoleId { get; set; }

        public byte UplineRoleId { get; set; }
    }
}
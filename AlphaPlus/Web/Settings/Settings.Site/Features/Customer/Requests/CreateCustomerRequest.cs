using AlphaPlus.Settings.Site.Features.Shared;
using Fanex.Customer.Client.Models;

namespace AlphaPlus.Settings.Site.Features.Customer.Requests
{
    public class CreateCustomerRequest : BaseQuery
    {
        public GeneralInfo GeneralInfo { get; set; }

        public string Group { get; set; }

        public string ProductGroups { get; set; }

        public override bool IsValid()
            => base.IsValid()
            && GeneralInfo != null
            && !string.IsNullOrWhiteSpace(GeneralInfo.Username)
            && !string.IsNullOrWhiteSpace(GeneralInfo.Password)
            && !string.IsNullOrWhiteSpace(ProductGroups);
    }
}
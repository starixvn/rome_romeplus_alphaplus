
using Fanex.Customer.Client.Models;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.CreateAccount.Requests
{
    public class CreateAccountRequest
    {
        public string UIBehavior { get; set; }

        public GeneralInfo GeneralInfo { get; set; }

        public string ProductGroups { get; set; }
    }
}
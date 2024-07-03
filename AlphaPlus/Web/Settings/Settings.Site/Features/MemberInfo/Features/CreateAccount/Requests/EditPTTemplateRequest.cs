using Fanex.Customer.Client.Models;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.CreateAccount.Requests
{
    public class EditPTTemplateRequest
    {
        public int Id { get; set; }

        public bool IsDefault { get; set; }

        public IEnumerable<PTTemplateSetting> Settings { get; set; }
    }
}
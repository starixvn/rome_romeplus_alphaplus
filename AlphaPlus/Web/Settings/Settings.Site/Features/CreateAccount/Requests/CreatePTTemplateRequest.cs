using Fanex.Customer.Client.Models;

namespace AlphaPlus.Settings.Site.Features.CreateAccount.Requests
{
    public class CreatePTTemplateRequest
    {
        public string Name { get; set; }

        public bool IsDefault { get; set; }

        public IEnumerable<PTTemplateSetting> Settings { get; set; }
    }
}
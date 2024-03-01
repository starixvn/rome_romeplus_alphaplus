using System.Collections.Generic;
using Alpha.MemberInfo.Site.Features.CreateAccount.Models;

namespace Alpha.MemberInfo.Site.Features.CreateAccount.Requests
{
    public class CreatePTTemplateRequest
    {
        public string Name { get; set; }

        public bool IsDefault { get; set; }

        public IEnumerable<PTTemplateSetting> Settings { get; set; }
    }
}
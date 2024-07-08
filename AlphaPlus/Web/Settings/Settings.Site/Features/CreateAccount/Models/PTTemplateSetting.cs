using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlphaPlus.Settings.Site.Features.CreateAccount.Models
{
    public class PTTemplateSetting
    {
        public byte GroupId { get; set; }

        public byte SubGroupId { get; set; }

        public decimal PT { get; set; }
    }
}
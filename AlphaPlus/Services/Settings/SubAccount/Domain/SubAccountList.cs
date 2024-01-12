using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaPlus.Settings.Domain
{
    public class SubAccountList
    {
        public IEnumerable<SubAccountModel> SubAccounts { get; set; }
    }
}
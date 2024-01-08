using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaPlus.Settings.Site.SubAccount.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.SubAccount.Requests
{
    public class SubAccountRequest : IRequest<IEnumerable<SubAccountViewModel>>
    {
        public SubAccountRequest()
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaPlus.Settings.Site.SubAccount.Models;
using AlphaPlus.Settings.Site.SubAccount.Requests;
using AlphaPlus.Settings.Site.SubAccount.Services;
using MediatR;

namespace AlphaPlus.Settings.Site.SubAccount.Handlers
{
    public class SubAccountHandler : 
        IRequestHandler<SubAccountRequest, IEnumerable<SubAccountViewModel>>
    {
        private readonly ISubAccountService subAccountService;

        public SubAccountHandler(ISubAccountService subAccountService)
        {
            this.subAccountService = subAccountService;
        }

        public async Task<IEnumerable<SubAccountViewModel>> Handle(SubAccountRequest request, CancellationToken cancellationToken)
        {
            return await subAccountService.GetSubAccounts(request);
        }
    }
}
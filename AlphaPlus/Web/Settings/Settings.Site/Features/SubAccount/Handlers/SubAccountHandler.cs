using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.SubAccount.Models;
using AlphaPlus.Settings.Site.Features.SubAccount.Requests;
using AlphaPlus.Settings.Site.Features.SubAccount.Services;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.SubAccount.Handlers
{
    public class SubAccountHandler : 
        IRequestHandler<SubAccountListRequest, IEnumerable<SubAccountViewModel>>,
        IRequestHandler<SubAccountRequest, SubAccountViewModel>,
        IRequestHandler<UpdateSubAccountPageAccessRequest, Result>,
        IRequestHandler<UpdateSubAccountInfoRequest, Result>,
        IRequestHandler<AddSubAccountRequest, Result>
    {
        private readonly ISubAccountService subAccountService;

        public SubAccountHandler(ISubAccountService subAccountService)
        {
            this.subAccountService = subAccountService;
        }

        public async Task<IEnumerable<SubAccountViewModel>> Handle(SubAccountListRequest request, CancellationToken cancellationToken)
        {
            return await subAccountService.GetSubAccounts(request);
        }

        public async Task<Result> Handle(UpdateSubAccountInfoRequest request, CancellationToken cancellationToken)
        {
            return await subAccountService.UpdateSubAccountInfo(request);
        }

        public async Task<Result> Handle(UpdateSubAccountPageAccessRequest request, CancellationToken cancellationToken)
        {
            return await subAccountService.UpdateSubAccountPageAccess(request);
        }

        public async Task<SubAccountViewModel> Handle(SubAccountRequest request, CancellationToken cancellationToken)
        {
            return await subAccountService.GetSubAccount(request.CustId, request.SubAccId);
        }

        public async Task<Result> Handle(AddSubAccountRequest request, CancellationToken cancellationToken)
        {
            return await subAccountService.AddSubAccount(request);
        }
    }
}
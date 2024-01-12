using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.SubAccount.Models;
using AlphaPlus.Settings.Site.SubAccount.Requests;

namespace AlphaPlus.Settings.Site.SubAccount.Services
{
    public interface ISubAccountService
    {
        public Task<IEnumerable<SubAccountViewModel>> GetSubAccounts(SubAccountRequest subAccountRequest);

        public Task<SubAccountViewModel> GetSubAccount(int subAccId);

        public Task<Result> Update(SubAccountViewModel subAccountViewModel);
    }

    public class SubAccountService : ISubAccountService
    {
        private readonly Infrastruture.ISubAccountService subAccountService;

        public SubAccountService(Infrastruture.ISubAccountService subAccountService)
        {
            this.subAccountService = subAccountService;
        }

        public async Task<SubAccountViewModel> GetSubAccount(int subAccId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SubAccountViewModel>> GetSubAccounts(SubAccountRequest subAccountRequest)
        {
            var result = await subAccountService.GetSubAccountList(subAccountRequest.CustId);
            return await Task.FromResult(Enumerable.Empty<SubAccountViewModel>());
        }

        public async Task<Result> Update(SubAccountViewModel subAccountViewModel)
        {
            throw new NotImplementedException();
        }
    }
}
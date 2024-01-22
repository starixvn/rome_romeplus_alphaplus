using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.SubAccount.Models;
using AlphaPlus.Settings.Site.SubAccount.Requests;

namespace AlphaPlus.Settings.Site.SubAccount.Services
{
    public interface ISubAccountService
    {
        public Task<IEnumerable<SubAccountViewModel>> GetSubAccounts(SubAccountListRequest subAccountRequest);

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

        public async Task<IEnumerable<SubAccountViewModel>> GetSubAccounts(SubAccountListRequest subAccountRequest)
        {
            var result = await subAccountService.GetSubAccountList(subAccountRequest.CustId);

            return result.Select(item => new SubAccountViewModel
            {
                Closed = item.Closed,
                CreateDate = item.CreateDate,
                Firstname = item.Firstname,
                Lastname = item.Lastname,
                Nickname = item.Nickname,
                PageAccess = item.PageAccess,
                Password = item.Password,
                SubAccId = item.SubAccId,
                Username = item.Username
            });
        }

        public async Task<Result> Update(SubAccountViewModel subAccountViewModel)
        {
            throw new NotImplementedException();
        }
    }
}
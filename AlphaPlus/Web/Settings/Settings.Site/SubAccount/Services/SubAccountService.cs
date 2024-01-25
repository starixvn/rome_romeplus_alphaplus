using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.SubAccount.Models;
using AlphaPlus.Settings.Site.SubAccount.Requests;

namespace AlphaPlus.Settings.Site.SubAccount.Services
{
    public interface ISubAccountService
    {
        public Task<IEnumerable<SubAccountViewModel>> GetSubAccounts(SubAccountListRequest subAccountRequest);

        public Task<SubAccountViewModel> GetSubAccount(int custId, int subAccId);

        public Task<Result> UpdateSubAccountInfo(UpdateSubAccountInfoRequest updateSubAccountInfoRequest);

        public Task<Result> UpdateSubAccountPassword(UpdateSubAccountPasswordRequest updateSubAccountPasswordRequest);

        public Task<Result> UpdateSubAccountPageAccess(UpdateSubAccountPageAccessRequest updateSubAccountPageAccessRequest);

        public Task<Result> UpdateSubAccountSecurityCode(UpdateSubAccountSecurityCodeRequest updateSubAccountSecurityCodeRequest);

        public Task<Result> RemoveSubAccountOtp(RemoveSubAccountOtpRequest removeSubAccountOtpRequest);

        public Task<Result> AddSubAccount(AddSubAccountRequest addSubAccountRequest);
    }

    public class SubAccountService : ISubAccountService
    {
        private readonly Settings.SubAccount.Infrastruture.ISubAccountService subAccountService;

        public SubAccountService(Settings.SubAccount.Infrastruture.ISubAccountService subAccountService)
        {
            this.subAccountService = subAccountService;
        }

        public async Task<Result> AddSubAccount(AddSubAccountRequest addSubAccountRequest)
        {
            return await subAccountService.AddSubAccount(new Settings.SubAccount.Domain.SubAccountModel
            {
                Username = addSubAccountRequest.Username,
                Password = addSubAccountRequest.Password,
                Firstname = addSubAccountRequest.Firstname,
                Lastname = addSubAccountRequest.Lastname,
                PageAccess = addSubAccountRequest.PageAccess
            });
        }

        public async Task<SubAccountViewModel> GetSubAccount(int custId, int subAccId)
        {
            var result = await subAccountService.GetSubAccount(custId, subAccId);
            return new SubAccountViewModel
            {
                Closed = result.Closed,
                CreateDate = result.CreateDate,
                Firstname = result.Firstname,
                Lastname = result.Lastname,
                Nickname = result.Nickname,
                PageAccess = result.PageAccess,
                Password = result.Password,
                SubAccId = result.SubAccId,
                Username = result.Username
            };
        }

        public async Task<IEnumerable<SubAccountViewModel>> GetSubAccounts(SubAccountListRequest subAccountRequest)
        {
            var result = await subAccountService.GetSubAccounts(subAccountRequest.CustId);

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

        public async Task<Result> RemoveSubAccountOtp(RemoveSubAccountOtpRequest removeSubAccountOtpRequest)
        {
            return await subAccountService.RemoveSubAccountOtp(removeSubAccountOtpRequest.SubAccId);
        }

        public async Task<Result> UpdateSubAccountInfo(UpdateSubAccountInfoRequest updateSubAccountInfoRequest)
        {
            return await subAccountService.UpdateSubAccountInfo(new Settings.SubAccount.Domain.SubAccountModel
            {
                SubAccId = updateSubAccountInfoRequest.SubAccId,
                Firstname = updateSubAccountInfoRequest.Firstname,
                Closed = updateSubAccountInfoRequest.Closed,
                Lastname = updateSubAccountInfoRequest.Lastname
            });
        }

        public async Task<Result> UpdateSubAccountPageAccess(UpdateSubAccountPageAccessRequest updateSubAccountPageAccessRequest)
        {
            return await subAccountService.UpdateSubAccountPageAccess(updateSubAccountPageAccessRequest.SubAccId, updateSubAccountPageAccessRequest.PageAccess);
        }

        public async Task<Result> UpdateSubAccountPassword(UpdateSubAccountPasswordRequest updateSubAccountPasswordRequest)
        {
            return await subAccountService.UpdateSubAccountPassword(updateSubAccountPasswordRequest.SubAccId, updateSubAccountPasswordRequest.NewPassword);
        }

        public async Task<Result> UpdateSubAccountSecurityCode(UpdateSubAccountSecurityCodeRequest updateSubAccountSecurityCodeRequest)
        {
            return await subAccountService.UpdateSubAccountSecurityCode(updateSubAccountSecurityCodeRequest.SubAccId, updateSubAccountSecurityCodeRequest.NewSecurityCode);
        }
    }
}
using AlphaPlus.Settings.Site.Features.CopyAccount.Request;
using AlphaPlus.Settings.Site.Features.CopyAccount.ViewModels;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
using Fanex.Cryptography.Aes;
using MediatR;
using AlphaPlus.Core.Models;
using Fanex.Customer.Client.Services;
using Fanex.Customer.Client.Params;
using Fanex.Customer.Client.Models;
using AlphaPlus.Core.Utils;

namespace AlphaPlus.Settings.Site.Features.CopyAccount
{
    public class CopyAccountHandler :
        IRequestHandler<CopyAccountQuery, CopyAccountViewModel>,
        IRequestHandler<CopyAccountCommand, CopyMultipleCustomersResult>,
        IRequestHandler<CreditQuery, CreditInfo>,
        IRequestHandler<CheckUsernameQuery, AlphaPlus.Core.Models.Result>,
        IRequestHandler<CheckExistedCustomerQuery, bool>
    {
        private const string HashKeySessionKey = "AlphaPlus.Settings.Site.Features.CopyAccount.CopyAccountHandler.PasswordHash";

        private readonly IEncryption encryption;
        private readonly IAesService aes;
        private readonly ISessionManager sessionManager;
        private readonly ICustomerService customerService;

        public CopyAccountHandler(
            IEncryption encryption,
            IAesService aes,
            ISessionManager sessionManager,
            ICustomerService customerService)
        {
            this.encryption = encryption;
            this.aes = aes;
            this.sessionManager = sessionManager;
            this.customerService = customerService;
        }

        public async Task<CopyAccountViewModel> Handle(CopyAccountQuery message, CancellationToken cancellation)
        {
            var limitNewDownlineResult = await customerService.CheckAllLimitedRules(
                   message.ViewerId,
                   message.ViewerLevelId,
                   message.CustName,
                   message.Language);

            if (limitNewDownlineResult.IsFailed)
            {
                return new CopyAccountViewModel(isLimitNewDownline: true, limitNewDownlineResult.Message, message);
            }

            return new CopyAccountViewModel(
                message,
                await customerService.GetSuggestUsername(message.CustName, message.ViewerLevelId),
                GetHasKey());
        }

        private string GetHasKey()
        {
            var hashKey = encryption.RandomString(4);

            sessionManager.Add(HashKeySessionKey, hashKey);

            return hashKey;
        }

        public async Task<CopyMultipleCustomersResult> Handle(CopyAccountCommand message, CancellationToken cancellation)
        {
            var model = await customerService.GetDownlineByUsername(message.CustId, message.SubAccId, message.SourceUsername);
            var sourceCustId = model.CustId;
            message.Password = aes.Decrypt(message.Password, sessionManager.Get<string>(HashKeySessionKey));
            if (string.IsNullOrWhiteSpace(message.Password) || sourceCustId <= 0 || !Enumeration.FromValue<UserLevel>(model.RoleId).IsDirectDownlineOf(message.ViewerLevel))
            {
                return new CopyMultipleCustomersResult
                {
                    Code = 1,
                    Message = Lang.NotAuthorized
                };
            }

            message.Password = encryption.CfsEnCode(message.Password);

            return await customerService.CopyMultipleCustomers(new CopyMultipleCustomersParam(
                message.ViewerId,
                message.SubAccId,
                message.CustId,
                message.CustLevel.GetDirectDownLineLevel().Value,
                message.ViewerUsername,
                message.Username,
                message.Password,
                message.Credit,
                message.MemberMaxCredit,
                sourceCustId,
                message.NumberOfAccounts,
                message.Site,
                false,
                message.IP,
                message.Language));
        }

        public async Task<CreditInfo> Handle(CreditQuery message, CancellationToken cancellation)
        {
            var result = await customerService.GetDownlineByUsername(message.ViewerId, message.SubAccId, message.CustName);
            if (result.CustId > 0 && Enumeration.FromValue<UserLevel>(result.RoleId).IsDirectDownlineOf(message.ViewerLevel))
            {
                return await customerService.GetCreditInfo(message.CustName);
            }
            else
            {
                throw new InvalidOperationException(Lang.NotAuthorized);
            }
        }

        public async Task<Core.Models.Result> Handle(CheckUsernameQuery message, CancellationToken cancellation)
        {
            var result = await customerService.GetDownlineByUsername(message.ViewerId, message.SubAccId, message.CustName);
            if (result.CustId > 0 && Enumeration.FromValue<UserLevel>(result.RoleId).IsDirectDownlineOf(message.ViewerLevel))
            {
                if (await customerService.IsCustomerEnabled(result.CustId))
                {
                    return Core.Models.Result.CreateSuccessfulResult();
                }
                else
                {
                    return Core.Models.Result.CreateErrorResult(Resources.AccountWasDisabled);
                }
            }

            return Core.Models.Result.CreateErrorResult(Resources.UserNotExist);
        }

        public Task<bool> Handle(CheckExistedCustomerQuery message, CancellationToken cancellation)
        {
            return customerService.CheckExistedCustomer(message.CustName);
        }
    }
}
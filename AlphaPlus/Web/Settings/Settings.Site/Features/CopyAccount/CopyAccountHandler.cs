using AlphaPlus.Settings.Site.Features.CopyAccount.Models;
using AlphaPlus.Settings.Site.Features.CopyAccount.Request;
using AlphaPlus.Settings.Site.Features.CopyAccount.ViewModels;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
using Fanex.Cryptography.Aes;
using MediatR;
using Sunplus.Agent.Utilities;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.CopyAccount
{
    public class CopyAccountHandler :
        IRequestHandler<CopyAccountQuery, CopyAccountViewModel>,
        IRequestHandler<CopyAccountCommand, CopyAccountResponse>,
        IRequestHandler<CreditQuery, CreditInfo>,
        IRequestHandler<CheckUsernameQuery, Result>,
        IRequestHandler<CheckExistedCustomerQuery, bool>
    {
        private const string HashKeySessionKey = "AlphaPlus.Settings.Site.Features.CopyAccount.CopyAccountHandler.PasswordHash";

        private readonly Encryptor encryptor;
        private readonly IAesService aes;
        private readonly ISessionManager sessionManager;
        private readonly ICopyAccountService copyAccountService;

        public CopyAccountHandler(
            Encryptor encryptor,
            IAesService aes,
            ISessionManager sessionManager,
            ICopyAccountService copyAccountService)
        {
            this.encryptor = encryptor;
            this.aes = aes;
            this.sessionManager = sessionManager;
            this.copyAccountService = copyAccountService;
        }

        public async Task<CopyAccountViewModel> Handle(CopyAccountQuery message, CancellationToken cancellation)
        {
            var limitNewDownlineResult = await copyAccountService.CheckAllLimitedRules(
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
                await copyAccountService.GetSuggestUsername(message.CustName, message.ViewerLevelId),
                GetHasKey());
        }

        private string GetHasKey()
        {
            var hashKey = encryptor.RandomString(4);

            sessionManager.Add(HashKeySessionKey, hashKey);

            return hashKey;
        }

        public async Task<CopyAccountResponse> Handle(CopyAccountCommand message, CancellationToken cancellation)
        {
            var model = await copyAccountService.GetDownlineByUsername(message.CustId, message.SubAccId, message.SourceUsername);
            var sourceCustId = model.CustId;
            message.Password = aes.Decrypt(message.Password, sessionManager.Get<string>(HashKeySessionKey));
            if (string.IsNullOrWhiteSpace(message.Password) || sourceCustId <= 0 || !Enumeration.FromValue<UserLevel>(model.RoleId).IsDirectDownlineOf(message.ViewerLevel))
            {
                return new CopyAccountResponse
                {
                    Code = 1,
                    Message = Lang.NotAuthorized
                };
            }

            message.Password = encryptor.CfsEnCode(message.Password);

            return await copyAccountService.CreateAccount(message, sourceCustId);
        }

        public async Task<CreditInfo> Handle(CreditQuery message, CancellationToken cancellation)
        {
            var result = await copyAccountService.GetDownlineByUsername(message.ViewerId, message.SubAccId, message.CustName);
            if (result.CustId > 0 && Enumeration.FromValue<UserLevel>(result.RoleId).IsDirectDownlineOf(message.ViewerLevel))
            {
                return await copyAccountService.GetCreditInfo(message.CustName);
            }
            else
            {
                throw new InvalidOperationException(Lang.NotAuthorized);
            }
        }

        public async Task<Result> Handle(CheckUsernameQuery message, CancellationToken cancellation)
        {
            var result = await copyAccountService.GetDownlineByUsername(message.ViewerId, message.SubAccId, message.CustName);
            if (result.CustId > 0 && Enumeration.FromValue<UserLevel>(result.RoleId).IsDirectDownlineOf(message.ViewerLevel))
            {
                if (await copyAccountService.IsCustomerEnabled(result.CustId))
                {
                    return Result.CreateSuccessfulResult();
                }
                else
                {
                    return Result.CreateErrorResult(Resources.AccountWasDisabled);
                }
            }

            return Result.CreateErrorResult(Resources.UserNotExist);
        }

        public Task<bool> Handle(CheckExistedCustomerQuery message, CancellationToken cancellation)
        {
            return copyAccountService.CheckExistedCustomer(message.CustName);
        }
    }
}
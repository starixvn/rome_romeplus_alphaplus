namespace AlphaPlus.Settings.MemberInfo.API.Services.IndirectLoginAccount
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AlphaPlus.Settings.MemberInfo.API.Criteria.IndirectLoginAccount;
    using AlphaPlus.Settings.MemberInfo.Domain.Params.IndirectLoginAccount;
    using AlphaPlus.Settings.MemberInfo.Domain.AppResources;
    using AlphaPlus.Settings.MemberInfo.Domain.Models.IndirectLoginAccount;
    using AlphaPlus.Settings.MemberInfo.Domain.Configurations;
    using Fanex.Cryptography.Aes;
    using Fanex.Data.Repository;
    using RSACryptoPad;
    using Sunplus.Agent.Utilities;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;

    public interface IndirectLoginAccountService
    {
        Task<IEnumerable<IndirectLoginAccountItem>> GetIndirectLoginAccountsAsync(int custId, int subAccId);

        Task<Result> ValidateIndirectLoginAccountAsync(int custId, int subAccId, int indirectCustId, int indirectSubAccId);

        Task<Result> NewAccountAsync(NewAccountParam param);

        Task<Result> UpdatePasswordAsync(UpdatePasswordParam param);

        Task<Result> DisconnectAsync(DisconnectAccountParam param);

        Task<string> BuildRedirectURLAsync(RedirectParam param);
    }

    public class IndirectLoginAccountServiceImpl : IndirectLoginAccountService
    {
        private const int NeedToReEnterPassword = 5001;
        private const int KeySize = 1024;

        private readonly IDynamicRepository dynamicRepository;
        private readonly IAesService aes;
        private readonly Encryptor encryptor;

        public IndirectLoginAccountServiceImpl(
            IDynamicRepository dynamicRepository,
            IAesService aes,
            Encryptor encryptor)
        {
            this.dynamicRepository = dynamicRepository;
            this.aes = aes;
            this.encryptor = encryptor;
        }

        public async Task<IEnumerable<IndirectLoginAccountItem>> GetIndirectLoginAccountsAsync(int custId, int subAccId)
            => await dynamicRepository.FetchAsync<IndirectLoginAccountItem>(new GetIndirectLoginAccountsCriteria(custId, subAccId));

        public async Task<Result> ValidateIndirectLoginAccountAsync(int custId, int subAccId, int indirectCustId, int indirectSubAccId)
        {
            Result result;
            var criteria = new GetIndirectLoginAccountsCriteria(custId, subAccId, indirectCustId, indirectSubAccId);
            var account = await dynamicRepository.GetAsync<IndirectLoginAccountItem>(criteria);

            if (account == null)
            {
                result = Result.CreateErrorResult(Resources.IndirectLoginAccountClosedOrDisabledError);
            }
            else if (!account.Enabled || account.Closed)
            {
                result = Result.CreateErrorResult(Resources.IndirectLoginAccountClosedOrDisabledError);
            }
            else if (account.IsLock)
            {
                result = Result.CreateErrorResult(Resources.IndirectLoginAccountLockedError);
            }
            else if (account.LastVerifyPwd.HasValue
                && account.LastChangedPwdDate.HasValue
                && account.LastVerifyPwd <= account.LastChangedPwdDate)
            {
                result = Result.CreateErrorResult(string.Empty, NeedToReEnterPassword);
            }
            else
            {
                result = new Result();
            }

            return result;
        }

        public async Task<Result> NewAccountAsync(NewAccountParam param)
        {
            param.Password = DecryptAndEncodePassword(param.Password, param.HashKey);

            var criteria = new NewAccountCriteria(
                param.CustName,
                param.Password,
                param.ActorId,
                param.ActorSubAccId,
                param.ActorLevelId,
                param.LanguageKey,
                param.RequestInfo.LoginIP,
                param.RequestInfo.LoginDomain,
                param.RequestInfo.BrowserName,
                param.RequestInfo.Platform,
                param.RequestInfo.CountryCode);
            await dynamicRepository.ExecuteAsync(criteria);
            var result = new Result { Code = criteria.ErrCode, Message = criteria.ErrMsg ?? string.Empty };

            if (result.IsOk)
            {
                result.Message = Resources.IndirectLoginAccountNewSuccessfully;
            }

            return result;
        }

        public async Task<Result> UpdatePasswordAsync(UpdatePasswordParam param)
        {
            param.Password = DecryptAndEncodePassword(param.Password, param.HashKey);

            var criteria = new UpdatePasswordCriteria(
                param.IndirectCustId,
                param.IndirectSubAccId,
                param.IndirectUsername,
                param.IndirectLevelId,
                param.Password,
                param.ActorId,
                param.ActorSubAccId,
                param.ActorUsername,
                param.LanguageKey,
                param.RequestInfo.LoginIP,
                param.RequestInfo.LoginDomain,
                param.RequestInfo.BrowserName,
                param.RequestInfo.Platform,
                param.RequestInfo.CountryCode);
            await dynamicRepository.ExecuteAsync(criteria);

            return new Result
            {
                Code = criteria.ErrCode,
                Message = criteria.ErrMsg ?? string.Empty
            };
        }

        private string DecryptAndEncodePassword(string encryptPassword, string hashKey)
        {
            var rawPassword = aes.Decrypt(encryptPassword, hashKey);

            return encryptor.CfsEnCode(rawPassword);
        }

        public async Task<Result> DisconnectAsync(DisconnectAccountParam param)
        {
            var criteria = new DisconnectAccountCriteria(
                param.IndirectCustId,
                param.IndirectSubAccId,
                param.IndirectUsername,
                param.ActorId,
                param.ActorSubAccId,
                param.ActorUsername,
                param.LanguageKey,
                param.IP);
            await dynamicRepository.ExecuteAsync(criteria);
            var result = new Result { Code = criteria.ErrCode, Message = criteria.ErrMsg ?? string.Empty };

            if (result.IsOk)
            {
                result.Message = Resources.IndirectLoginAccountDisconnectSuccessfully;
            }

            return result;
        }

        public async Task<string> BuildRedirectURLAsync(RedirectParam param)
        {
            // IndirectCustId, IndirectSubAccId, LoginTimeTicks, Language, CustId, SubAccId, Username
            // BrowserSize, IndirectUsername, DarkModeName, AssetAppPath
            var parameter = string.Format(
                "{0}&{1}&{2}&{3}&{4}&{5}&{6}&{7}&{8}&{9}&{10}",
                param.IndirectCustId,
                param.IndirectSubAccId,
                DateTime.Now.Ticks,
                param.Language,
                param.ActorId,
                param.ActorSubAccId,
                param.ActorUsername,
                param.BrowserSize,
                param.IndirectUsername,
                param.DarkModeName,
                param.AssetAppPath);

            parameter = await Task.Run(() =>
                new EncryptionThread().EncryptString(parameter, KeySize, param.IBCPKey)
            ).ConfigureAwait(false);

            return $"{AppSettings.IndirectLoginURL}?param={parameter}";
        }
    }
}

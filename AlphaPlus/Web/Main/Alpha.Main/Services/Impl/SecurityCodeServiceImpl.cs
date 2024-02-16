using System.Text.RegularExpressions;
using Alpha.Main.AppResources;
using Alpha.Main.Criteria.SecurityCode;
using Alpha.Main.Params.SecurityCode;
using Fanex.Data.Repository;
using AlphaPlus.Core.Utils;
using AlphaPlus.Core.Models;
using TelegramBot.Client.Services;

namespace Alpha.Main.Services.Impl
{
    public class SecurityCodeServiceImpl : BaseService, SecurityCodeService
    {
        private const string MD5 = "MD5";
        private const string SHA1 = "SHA1";
        private const string SHA256 = "SHA256";
        private const string SECURITYCODEPADDING = "1qaw3+{:P)";
        private const int AllowSecurityCodeLength = 6;
        private const string SecurityCodeRule = @"^([0-9])(?!\1+$)[0-9]+$";
        private const int Length = 6;
        private const int AccountClosed = 200;
        private const int AccountClosedStatus = 2;
        private const int WrongSecurityCode = 9009;
        private readonly IEncryption encryption;
        // private readonly INetThrottlingService netThrottlingService;
        private readonly INotificationService notificationService;

        public SecurityCodeServiceImpl(
            IDynamicRepository dynamicRepository,
            IEncryption encryption,
            // INetThrottlingService netThrottlingService,
            INotificationService notificationService)
            : base(dynamicRepository)
        {
            this.encryption = encryption;
            // this.netThrottlingService = netThrottlingService;
            this.notificationService = notificationService;
        }

        public async Task<Result> ChangeMainAccountSecurityCode(ChangeSecurityCodeParam param)
        {
            var validateResult = await Validate(param.NewSecurityCode, param);
            if (validateResult.IsNotOk)
            {
                return validateResult;
            }

            var result = await ExecuteCommand(
                 new UpdateMainAccountSecurityCodeCriteria
                 {
                     CustId = param.CustId,
                     SubaccId = param.SubAccId,
                     UserId = param.UserId,
                     HashedSecurityCode = HashSecurityCode(param.CustId, param.NewSecurityCode, param.IsResetMember),
                     IP = param.IP,
                     Language = param.Language
                 });

            if (result != null && result.IsSuccessful && !param.IsMemberLevel)
            {
                Task.Run(() => notificationService.NotifyChangeSecurityCode(param.CustId, 0));
            }

            return result;
        }

        public async Task<Result> ChangeSubAccountSecurityCode(ChangeSecurityCodeParam param)
        {
            var validateResult = await Validate(param.NewSecurityCode, param);
            if (validateResult.IsNotOk)
            {
                return validateResult;
            }

            var command = new UpdateSubAccountSecurityCodeCriteria
            {
                CustId = param.CustId,
                SubaccId = param.SubAccId,
                HashedSecurityCode = HashSecurityCode(param.SubAccId, param.NewSecurityCode),
                IP = param.IP,
                Language = param.Language
            };

            var result = await ExecuteCommand(command);

            if (result != null && result.IsSuccessful && !param.IsMemberLevel)
            {
                Task.Run(() => notificationService.NotifyChangeSecurityCode(param.CustId, param.SubAccId));
            }

            return result;
        }

        public async Task<Result> ResetSubAccountSecurityCode(ResetSecurityCodeParam param)
        {
            // TODO
            // if (netThrottlingService.IsBlacklist())
            // {
            //     return Result.CreateErrorResult("Too many requests");
            // }

            var validateResult = await Validate(param.NewSecurityCode);
            if (!validateResult.IsOk)
            {
                return validateResult;
            }

            var command = new ResetSubAccountSecurityCodeCriteria
            {
                CustId = param.CustId,
                SubaccId = param.SubAccId,
                UserId = param.UserId,
                HashedSecurityCode = HashSecurityCode(param.SubAccId, param.NewSecurityCode),
                IP = param.IP,
                Language = param.Language
            };

            var result = await ExecuteCommand(command);

            if (result != null && result.IsSuccessful && !param.IsMemberLevel)
            {
                // TODO
                // netThrottlingService.Throttle();

                Task.Run(() => notificationService.NotifyChangeSecurityCode(param.CustId, param.SubAccId));
            }

            return result;
        }

        private string HashSecurityCode(int custId, string securityCode, bool isResetMember = false)
        {
            //// because the Member site doesn't want to edit his code.
            //// so that we must use the CfsEnCode method.
            if (isResetMember)
            {
                return encryption.CfsEnCode(securityCode);
            }

            var salt = GenerateSalt(custId);

            return encryption.ComputeHash(SHA1, securityCode, salt);
        }

        public string GenerateSalt(int custId)
        {
            string plainText = custId.ToString();

            return encryption.ComputeHash(MD5, plainText, SECURITYCODEPADDING);
        }

        private async Task<Result> Validate(string securityCode, ChangeSecurityCodeParam param = null)
        {
            if (string.IsNullOrWhiteSpace(securityCode))
            {
                return Result.CreateErrorResult(Security.alertSecCodeEmpty);
            }

            if (!IsMatchedRules(securityCode))
            {
                return Result.CreateErrorResult(Security.lblSecCodeWrongRule);
            }

            if (param != null && param.MustVerifyCurrentSecurityCode)
            {
                if (string.IsNullOrWhiteSpace(param.CurrentSecurityCode))
                {
                    return Result.CreateErrorResult(Security.alertOldSecCodeEmpty);
                }

                var verifyResult = await VerifySecurityCode(new VerifySecurityCodeParam
                {
                    CustId = param.CustId,
                    SecurityCode = param.CurrentSecurityCode,
                    IP = param.IP,
                    Language = param.Language,
                    Salt = param.Salt,
                    SubAccId = param.SubAccId
                });

                if (verifyResult.IsNotOk)
                {
                    return verifyResult;
                }
            }

            return new Result();
        }

        private static bool IsMatchedRules(string securityCode)
        {
            int securityCodeLength = securityCode == null ? 0 : securityCode.Trim().Length;
            if (securityCodeLength != AllowSecurityCodeLength)
            {
                return false;
            }

            Regex regex = new(SecurityCodeRule);
            //// check digits and all of them are not the same
            bool isMatched = regex.IsMatch(securityCode);
            if (isMatched)
            {
                //// check 'Forward Run' and 'Backward Run'
                bool isForwardRun = true, isBackwardRun = true;
                int forward = 0, backward = 0;
                forward = backward = int.Parse(securityCode[0].ToString());
                for (int i = 1; i < securityCodeLength; i++)
                {
                    int current = int.Parse(securityCode[i].ToString());
                    if (++forward != current)
                    {
                        isForwardRun = false;
                    }

                    if (--backward != current)
                    {
                        isBackwardRun = false;
                    }
                }

                isMatched = !(isForwardRun || isBackwardRun);
            }

            return isMatched;
        }

        public async Task<Result> VerifySecurityCode(VerifySecurityCodeParam param)
        {
            if (string.IsNullOrEmpty(param.SecurityCode))
            {
                return Result.CreateErrorResult(Security.informSecCode);
            }

            if (string.IsNullOrEmpty(param.SecurityCode)
                || string.IsNullOrEmpty(param.Salt))
            {
                return Result.CreateErrorResult(Security.alertInputWrongSecCode);
            }

            // Database store Security Code hashed with CustID/SubAccID Salt
            // But Security code send from client browser has been hashed with CustID/SubAccID Salt by SHA256 Algorithm
            // So we have to hased security code from database with CustID/SubAccID Salt by SHA256 first, then compare
            // with hased security code which was sent from client browser.
            var actualSecurityCode = await GetSecurityCode(param.CustId, param.SubAccId);
            var hashedActualSecurityCodeWithSalt = this.encryption.ComputeHash(SHA256, actualSecurityCode, param.Salt);
            var isCorrectSecurityCode = string.Equals(param.SecurityCode, hashedActualSecurityCodeWithSalt);

            //// update counter after verify
            return await UpdateSecurityCodeCounter(param.CustId, isCorrectSecurityCode, param.Language, param.SubAccId, param.IP);
        }

        public string GenerateSecurityToken()
        {
            return this.encryption.RandomString(Length);
        }

        private Task<string> GetSecurityCode(int custId, int subAccountId)
        {
            if (subAccountId.Equals(0))
            {
                return dynamicRepository.GetAsync<string>(
                    new GetMainAccountSecurityCodeCriteria
                    {
                        CustId = custId
                    });
            }

            return dynamicRepository.GetAsync<string>(
                    new GetSubAccountSecurityCodeCriteria
                    {
                        CustId = custId,
                        SubaccId = subAccountId
                    });
        }

        private async Task<Result> UpdateSecurityCodeCounter(
            int custId,
            bool isCorrectSecurityCode,
            string language,
            int subAccountId,
            string ip)
        {
            var result = subAccountId.Equals(0)
                ? await ExecuteCommand(
                    new UpdateMainAccountSecurityCodeCounterCriteria
                    {
                        CustId = custId,
                        CheckedSecurityCode = isCorrectSecurityCode,
                        IP = ip,
                        Language = language
                    })
                : await ExecuteCommand(
                    new UpdateSubAccountSecurityCodeCounterCriteria
                    {
                        CustId = custId,
                        SubaccId = subAccountId,
                        CheckedSecurityCode = isCorrectSecurityCode,
                        IP = ip,
                        Language = language
                    });

            if (result.Code == AccountClosed)
            {
                result.Code = AccountClosedStatus; //// account is closed
            }
            else if (result.IsFailed)
            {
                result.Code = WrongSecurityCode; //// wrong SC
                result.Message = Security.alertInputWrongSecCode;
            }

            return result;
        }

        public Task<Result> UpdateSecurityCodeFunction(UpdateSecurityCodeFunctionParam param)
            => ExecuteCommand(new UpdateSecurityCodeFunctionCriteria(param));
    }
}

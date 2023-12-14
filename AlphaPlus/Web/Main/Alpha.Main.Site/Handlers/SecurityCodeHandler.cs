using Alpha.Main.ProfilePicture.Services;
using AlphaPlus.Core.Models;
using Alpha.Main.AppResources;
using Fanex.Cryptography.Aes;
using Alpha.Main.Models;
using MediatR;
using Alpha.Main.Site.Models.Request.SecurityCode;
using Alpha.Main.Params.SecurityCode;
using Alpha.Main.Services;
using Alpha.Main.Site.ViewModels.Security;
using CustomResult = Alpha.Main.Models.Result;
using AlphaPlus.SecurityCode.Web;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Utils;

namespace Alpha.Main.Site.Handlers.SecurityCode
{
    public class SecurityCodeHandler :
        IRequestHandler<ForceChangeSecurityCodeQuery, ForceChangeSecurityCodeViewModel>,
        IRequestHandler<SecurityCodeQuery, SecurityCodeViewModel>,
        IRequestHandler<ChangeSecurityCodeFirstLoginCommand, CustomResult>,
        IRequestHandler<ChangeSecurityCodeByForcingCommand, CustomResult>,
        IRequestHandler<ChangeUserOwnSecurityCodeCommand, CustomResult>,
        IRequestHandler<ResetSecurityCodeCommand, CustomResult>,
        IRequestHandler<VerifySecurityCodeCommand, CustomResult>,
        IRequestHandler<UpdateSecurityCodeFunctionCommand, CustomResult>
    {
        private const string HashKeySessionKey = "ph";
        private const string SecurityCodeTokenSessionKey = "SECURITYCODETOKEN";

        private readonly ISessionManager sessionManager;
        private readonly IEncryption encryption;
        private readonly SecurityCodeService securityCodeService;
        private readonly ProfilePictureServices customerProfileService;
        private readonly IUserSession userSession;
        private readonly IUser user;
        private readonly IAesService aes;

        public SecurityCodeHandler(
          SecurityCodeService securityCodeService,
          ProfilePictureServices customerProfileService,
          ISessionManager sessionManager,
          IEncryption encryption,
          IUserSession userSession,
          IUser user,
          IAesService aes)
        {
            this.sessionManager = sessionManager;
            this.encryption = encryption;
            this.securityCodeService = securityCodeService;
            this.customerProfileService = customerProfileService;
            this.userSession = userSession;
            this.user = user;
            this.aes = aes;
        }

        private string HashKey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(sessionManager.Get<string>(HashKeySessionKey)))
                {
                    var hashKey = encryption.RandomString(4);

                    sessionManager.Add(HashKeySessionKey, hashKey);
                }

                return sessionManager.Get<string>(HashKeySessionKey);
            }
        }

        private string Decrypt(string encryptedData)
        {
            if (string.IsNullOrWhiteSpace(encryptedData))
            {
                return string.Empty;
            }

            return aes.Decrypt(encryptedData, HashKey);
        }

        public Task<SecurityCodeViewModel> Handle(SecurityCodeQuery message, CancellationToken cancellationToken)
        {
            string securityCodeToken = securityCodeService.GenerateSecurityToken();
            sessionManager.Add(SecurityCodeTokenSessionKey, securityCodeToken);

            return Task.FromResult(new SecurityCodeViewModel
            {
                CustId = message.CustId,
                SubAccId = message.SubAccId,
                SecurityCodeToken = securityCodeToken,
                SecuritySalt = GenerateSecuritySalt(),
                HashKey = HashKey,
                CustRoleId = message.RoleId,
                Title = BuildSecurityCodeTitle(message.Username),
                UsernameShower = BuildUsernameShower(message.IsSubAcc, message.SubUserName, message.NickName, message.Username),
                IsEnabledBetLists = message.IsEnabledSecurityCodeInBetLists,
                IsEnabledStatement = message.IsEnabledSecurityCodeInStatement,
                IsOfferProtectedMode = message.IsOfferProtectedMode
            });
        }

        public async Task<ForceChangeSecurityCodeViewModel> Handle(ForceChangeSecurityCodeQuery message, CancellationToken cancellationToken)
        {
            var securityCodeToken = securityCodeService.GenerateSecurityToken();

            sessionManager.Add(SecurityCodeTokenSessionKey, securityCodeToken);

            var profile = await customerProfileService.GetActorProfilePicture(this.userSession.CustId);

            return new ForceChangeSecurityCodeViewModel
            {
                SecurityCodeToken = securityCodeToken,
                SecuritySalt = GenerateSecuritySalt(),
                HashKey = HashKey,
                CustRoleId = message.RoleId,
                Username = message.UserName,
                AlternateUsername = BuildAlternateUsername(message),
                UsernameShower = BuildUsernameShower(message.IsSubAcc, message.SubUserName, message.NickName, message.UserName),
                IsDefaultAvatar = string.IsNullOrEmpty(profile.ImageBase64String),
                EmbedImage = profile.ImageBase64String
            };
        }

        public async Task<CustomResult> Handle(ChangeSecurityCodeFirstLoginCommand message, CancellationToken cancellationToken)
        {
            // Make sure that, when user already has SC in session, then cannot be forced change by first login
            if (!string.IsNullOrWhiteSpace(userSession.SecurityCode))
            {
                return CustomResult.CreateErrorResult(Security.NotAuthorized);
            }

            message.SecurityCode = Decrypt(message.SecurityCode);
            var validateResult = HasUpdatePermission();

            if (!validateResult.IsOk)
            {
                return validateResult;
            }

            var custLevel = Enumeration.FromValue<UserLevel>(userSession.RoleId);
            var result = await ChangeSecurityCode(new ChangeSecurityCodeParam
            {
                CustId = message.CustId,
                SubAccId = message.SubAccId,
                UserId = message.UserId,
                IsMemberLevel = custLevel.IsMemberLevel(),
                NewSecurityCode = message.SecurityCode,
                Language = message.Language,
                IP = message.IP,
                MustVerifyCurrentSecurityCode = false
            });

            if (result.IsSuccessful)
            {
                if (string.IsNullOrWhiteSpace(userSession.SecurityCode))
                {
                    userSession.SecurityCode = message.SecurityCode;
                }

                ResetToken();
            }

            return result;
        }

        public async Task<CustomResult> Handle(ChangeSecurityCodeByForcingCommand message, CancellationToken cancellationToken)
        {
            message.SecurityCode = Decrypt(message.SecurityCode);
            var validateResult = HasUpdatePermission();

            if (!validateResult.IsOk)
            {
                return validateResult;
            }

            var custLevel = Enumeration.FromValue<UserLevel>(userSession.RoleId);
            var result = await ChangeSecurityCode(new ChangeSecurityCodeParam
            {
                CustId = message.CustId,
                SubAccId = message.SubAccId,
                UserId = message.UserId,
                IsMemberLevel = custLevel.IsMemberLevel(),
                NewSecurityCode = message.SecurityCode,
                Language = message.Language,
                IP = message.IP,
                CurrentSecurityCode = message.OldSecurityCode,
                Salt = sessionManager.Get<string>(SecurityCodeTokenSessionKey),
                MustVerifyCurrentSecurityCode = true
            });

            if (result.IsSuccessful)
            {
                userSession.SecurityCode = message.SecurityCode;
                ResetToken();
            }

            return result;
        }

        public async Task<CustomResult> Handle(ChangeUserOwnSecurityCodeCommand message, CancellationToken cancellationToken)
        {
            message.SecurityCode = Decrypt(message.SecurityCode);
            var validateResult = HasUpdatePermission();

            if (!validateResult.IsOk)
            {
                return validateResult;
            }

            var custLevel = Enumeration.FromValue<UserLevel>(userSession.RoleId);
            var param = new ChangeSecurityCodeParam
            {
                CustId = message.CustId,
                SubAccId = message.SubAccId,
                UserId = message.UserId,
                IsMemberLevel = custLevel.IsMemberLevel(),
                NewSecurityCode = message.SecurityCode,
                Language = message.Language,
                IP = message.IP,
                CurrentSecurityCode = message.OldSecurityCode,
                Salt = sessionManager.Get<string>(SecurityCodeTokenSessionKey),
                MustVerifyCurrentSecurityCode = true
            };

            var result = await ChangeSecurityCode(param);

            if (result.IsSuccessful)
            {
                ResetToken();
            }

            return result;
        }

        public async Task<CustomResult> Handle(ResetSecurityCodeCommand message, CancellationToken cancellationToken)
        {
            message.SecurityCode = Decrypt(message.SecurityCode);
            bool isResetSubAccount = message.SubAccId.HasValue && message.SubAccId.Value > 0;

            var validateResult = HasUpdatePermission(isResetSubAccount);

            if (!validateResult.IsOk)
            {
                return validateResult;
            }

            var result = new CustomResult();
            if (message.CustId.HasValue && message.CustId.Value > 0)
            {
                result = await ResetDownlineSecurityCode(message);
            }
            else if (isResetSubAccount)
            {
                result = await ResetSubAccountSecurityCode(message);
            }

            return result;
        }

        public async Task<CustomResult> Handle(VerifySecurityCodeCommand message, CancellationToken cancellationToken)
        {
            var result = await securityCodeService.VerifySecurityCode(
                new VerifySecurityCodeParam
                {
                    CustId = message.CustId,
                    SecurityCode = message.SecurityCode,
                    IP = message.IP,
                    Language = userSession.Language,
                    Salt = sessionManager.Get<string>(SecurityCodeTokenSessionKey),
                    SubAccId = message.SubAccId
                });

            if (result.IsSuccessful)
            {
                ResetToken();
            }

            return result;
        }

        private CustomResult HasUpdatePermission(bool isResetSubAccount = false)
        {
            if (userSession.IsInternal
                && !isResetSubAccount)
            {
                return new CustomResult { Code = CustomResult.ErrorCode, Message = Security.NotAuthorized };
            }

            if (isResetSubAccount && !user.IsAdminCanUpdateSubAccount)
            {
                return new CustomResult { Code = CustomResult.ErrorCode, Message = Security.NotAuthorized };
            }

            return new CustomResult();
        }

        private void ResetToken()
        {
            sessionManager.Add(SecurityCodeTokenSessionKey, string.Empty);
            userSession.IsForcedSecurityCode = false;
            SecurityCodeSessionManager.SecurityCodeSession.IsAlreadyInputSecurityCode = true;
        }

        private async Task<CustomResult> ChangeSecurityCode(ChangeSecurityCodeParam param)
        {
            var result = param.SubAccId.Equals(0)
                ? await securityCodeService.ChangeMainAccountSecurityCode(param)
                : await securityCodeService.ChangeSubAccountSecurityCode(param);

            if (result.IsSuccessful)
            {
                result.Message = Security.alertSecCodeChangedOK;
            }

            return result;
        }

        private async Task<CustomResult> ResetDownlineSecurityCode(ResetSecurityCodeCommand command)
        {
            var custLevel = Enumeration.FromValue<UserLevel>(userSession.RoleId);
            var result = await securityCodeService.ChangeMainAccountSecurityCode(
                new ChangeSecurityCodeParam
                {
                    CustId = command.CustId.Value,
                    UserId = userSession.UserId,
                    SubAccId = userSession.SubAccId,
                    IsMemberLevel = custLevel.IsMemberLevel(),
                    IsResetMember = custLevel.Equals(UserLevel.Agent),
                    NewSecurityCode = command.SecurityCode,
                    Language = command.Language,
                    IP = command.IP,
                    MustVerifyCurrentSecurityCode = false
                });

            if (result.IsSuccessful)
            {
                result.Message = Security.alertSecCodeChangedOK;
            }

            return result;
        }

        private async Task<CustomResult> ResetSubAccountSecurityCode(ResetSecurityCodeCommand command)
        {
            var custLevel = Enumeration.FromValue<UserLevel>(userSession.RoleId);
            var result = await securityCodeService.ResetSubAccountSecurityCode(
                new ResetSecurityCodeParam
                {
                    CustId = userSession.CustId,
                    UserId = userSession.UserId,
                    SubAccId = command.SubAccId.Value,
                    IsMemberLevel = custLevel.IsMemberLevel(),
                    NewSecurityCode = command.SecurityCode,
                    Language = command.Language,
                    IP = command.IP
                });

            if (result.IsSuccessful)
            {
                result.Message = Security.alertSecCodeChangedOK;
            }

            return result;
        }

        private static string BuildUsernameShower(bool isSubAcc, string subUsername, string nickName, string username)
        {
            if (isSubAcc)
            {
                return subUsername;
            }

            return string.IsNullOrWhiteSpace(nickName) ? username : nickName;
        }

        private static string BuildAlternateUsername(ForceChangeSecurityCodeQuery message)
        {
            if (message.IsSubAcc)
            {
                return $"({Resources.subAcc.ToUpper()} {message.SubUserName})";
            }

            return string.IsNullOrWhiteSpace(message.NickName) ? string.Empty : $"({message.NickName})";
        }

        private string GenerateSecuritySalt()
        {
            var actualCustId = userSession.SubAccId > 0 ? userSession.SubAccId : userSession.CustId;

            return securityCodeService.GenerateSalt(actualCustId);
        }

        private static string BuildSecurityCodeTitle(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return Security.titleResetSecCode;
            }

            return $"{Security.titleResetSecCode} - {username}";
        }

        public async Task<CustomResult> Handle(UpdateSecurityCodeFunctionCommand message, CancellationToken cancellationToken)
        {
            var result = HasUpdatePermission();

            if (result.IsOk)
            {
                result = await securityCodeService.UpdateSecurityCodeFunction(new UpdateSecurityCodeFunctionParam(
                   message.IsOn,
                   message.Function,
                   message.CustId,
                   message.SubAccId,
                   message.LangKey,
                   message.IP));

                if (result.IsOk)
                {
                    UpdateSecurityCodeFunctionSession(message.IsOn, message.Function);
                }
            }

            return result;
        }

        private void UpdateSecurityCodeFunctionSession(bool isOn, string function)
        {
            if ("BetList" == function)
            {
                userSession.IsEnabledSecurityCodeInBetLists = isOn;
            }
            else if ("Statement" == function)
            {
                userSession.IsEnabledSecurityCodeInStatement = isOn;
            }
        }
    }
}
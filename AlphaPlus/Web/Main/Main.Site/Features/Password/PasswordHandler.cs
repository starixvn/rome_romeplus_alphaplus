using AlphaPlus.Core.Models;
using AlphaPlus.Main.Site.AppResources;
using MediatR;
using CustomResult = AlphaPlus.Core.Models.Result;
using AlphaPlus.SecurityCode.Services;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Utils;
using AlphaPlus.Main.Site.Features.CustomerProfile;
using AlphaPlus.Main.Site.Shared.Models;

namespace AlphaPlus.Main.Site.Features.Password
{
    public class PasswordHandler :
         IRequestHandler<ChangePasswordCommand, CustomResult>,
         IRequestHandler<ResetMainAccountPasswordCommand, CustomResult>,
         IRequestHandler<ResetSubAccountPasswordCommand, CustomResult>,
         IRequestHandler<ChangePasswordQuery, PasswordViewModel>,
         IRequestHandler<ForceChangePasswordQuery, ForceChangePasswordViewModel>,
         IRequestHandler<ResetMainAccountPasswordQuery, PasswordViewModel>,
         IRequestHandler<ResetSubAccountPasswordQuery, PasswordViewModel>,
         IRequestHandler<SkipForceChangePasswordCommand, CustomResult>
    {
        private const string HashKeySessionKey = "Alpha.Main.PasswordHandler.PasswordHash";

        private readonly IPasswordService passwordService;
        private readonly IUserSession userSession;
        private readonly ISessionManager sessionManager;
        private readonly IEncryption encryption;
        private readonly IProfilePictureService customerProfileService;
        private readonly ISecurityCodeChecker securityCodeChecker;
        private readonly IUser user;

        public PasswordHandler(
            IPasswordService passwordService,
            IUserSession userSession,
            IProfilePictureService customerProfileService,
            ISessionManager sessionManager,
            IEncryption encryption,
            ISecurityCodeChecker securityCodeChecker,
            IUser user)
        {
            this.passwordService = passwordService;
            this.userSession = userSession;
            this.sessionManager = sessionManager;
            this.encryption = encryption;
            this.customerProfileService = customerProfileService;
            this.securityCodeChecker = securityCodeChecker;
            this.user = user;
        }

        public Task<CustomResult> Handle(ResetSubAccountPasswordCommand message, CancellationToken cancellationToken)
        {
            if (!HasUpdatePermission(true, true))
            {
                return Task.FromResult(CustomResult.CreateErrorResult(Security.NotAuthorized));
            }

            return passwordService.ResetSubAccountPassword(
                new ResetSubAccountPasswordParam
                {
                    ActorId = message.ActorId,
                    ActorName = message.ActorName,
                    CustId = message.CustId,
                    HashKey = sessionManager.Get<string>(HashKeySessionKey),
                    IP = message.IP,
                    LanguageKey = message.LanguageKey,
                    NewPassword = message.NewPassword,
                    SubAccountId = message.SubAccountId,
                    CustLevel = Enumeration.FromValue<UserLevel>(message.CustRoleId),
                    ServerId = message.ServerId,
                    IndirectCustId = message.IndirectCustId,
                    IsIndirectLoggedIn = message.IsIndirectLoggedIn,
                    IndirectSubAccId = message.IndirectSubAccId,
                    IndirectUsername = message.IndirectUsername
                });
        }

        public Task<PasswordViewModel> Handle(ChangePasswordQuery message, CancellationToken cancellationToken)
        {
            return Task.FromResult(new PasswordViewModel
            {
                CustRoleId = message.CustRoleId,
                HashKey = GetHasKey(),
                EnableSecurityCode = securityCodeChecker.IsEnableSecurityCode(),
                IsOfferProtectedMode = message.IsOfferProtectedMode,
            });
        }

        private string GetHasKey()
        {
            var hashKey = encryption.RandomString(4);
            sessionManager.Add(HashKeySessionKey, hashKey);
            return hashKey;
        }

        public Task<CustomResult> Handle(ResetMainAccountPasswordCommand message, CancellationToken cancellationToken)
        {
            if (!HasUpdatePermission(true))
            {
                return Task.FromResult(CustomResult.CreateErrorResult(Security.NotAuthorized));
            }

            var custLevel = Enumeration.FromValue<UserLevel>(message.CustRoleId);

            return passwordService.ResetMainAccountPassword(
                new ResetMainAccountPasswordParam
                {
                    ActorId = message.ActorId,
                    ActorName = message.ActorName,
                    CustId = message.CustId,
                    HashKey = sessionManager.Get<string>(HashKeySessionKey),
                    IP = message.IP,
                    LanguageKey = message.LanguageKey,
                    NewPassword = message.NewPassword,
                    SubAccountId = message.SubAccountId,
                    IsResetMemberPassword = custLevel.IsMemberLevel(),
                    CustLevel = custLevel,
                    ServerId = message.ServerId,
                    IndirectCustId = message.IndirectCustId,
                    IsIndirectLoggedIn = message.IsIndirectLoggedIn,
                    IndirectSubAccId = message.IndirectSubAccId,
                    IndirectUsername = message.IndirectUsername
                });
        }

        public async Task<CustomResult> Handle(ChangePasswordCommand message, CancellationToken cancellationToken)
        {
            if (!HasUpdatePermission())
            {
                return CustomResult.CreateErrorResult(Security.NotAuthorized);
            }

            var result = message.IsSubAccount ? await ChangeSubAccountPassword(message, CancellationToken cancellation) : await ChangeMainAccountPassword(message, CancellationToken cancellation);

            if (result.IsSuccessful)
            {
                //// Update is first login status
                userSession.IsLogged = true;

                //// Update last change pwd date
                userSession.IsPasswordExpired = false;
            }

            return result;
        }

        public async Task<CustomResult> Handle(SkipForceChangePasswordCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsShowSkipForceChangePassword ||
                (await passwordService.SkipForceChangePassword(new SkipForceChangePasswordParam
                {
                    CustId = message.CustId,
                    SubAccountId = message.SubAccId,
                    LanguageKey = message.Language,
                    IP = message.IP
                })).IsNotOk)
            {
                return CustomResult.CreateErrorResult("Can not skip force change Password");
            }

            //// Update is first login status
            this.userSession.IsLogged = true;

            //// Update last change pwd date
            this.userSession.IsPasswordExpired = false;

            return new CustomResult { Code = 0 };
        }

        private Task<CustomResult> ChangeMainAccountPassword(ChangePasswordCommand message, CancellationToken cancellation)
        {
            return passwordService.ChangeMainAccountPassword(new ChangeMainAccountPasswordParam
            {
                ActorId = message.ActorId,
                ActorName = message.Username,
                CurrentPassword = message.CurrentPassword,
                CustId = message.CustId,
                CustLevel = Enumeration.FromValue<UserLevel>(message.RoleId),
                HashKey = sessionManager.Get<string>(HashKeySessionKey),
                IP = message.IP,
                LanguageKey = message.LanguageKey,
                NewPassword = message.NewPassword,
                NickName = message.NickName,
                IsIndirectLoggedIn = message.IsIndirectLoggedIn,
                IndirectCustId = message.IndirectCustId,
                IndirectSubAccId = message.IndirectSubAccId,
                IndirectUsername = message.IndirectUsername
            });
        }

        private Task<CustomResult> ChangeSubAccountPassword(ChangePasswordCommand message, CancellationToken cancellation)
        {
            return passwordService.ChangeSubAccountPassword(
            new ChangeSubAccountPasswordParam
            {
                CustId = message.CustId,
                CustLevel = Enumeration.FromValue<UserLevel>(message.RoleId),
                HashKey = this.sessionManager.Get<string>(HashKeySessionKey),
                IP = message.IP,
                LanguageKey = message.LanguageKey,
                NewPassword = message.NewPassword,
                SubAccountId = message.SubAccountId,
                CurrentPassword = message.CurrentPassword,
                Nickname = message.NickName,
                SubUsername = message.SubAccountUsername,
                Username = message.Username,
                IsIndirectLoggedIn = message.IsIndirectLoggedIn,
                IndirectCustId = message.IndirectCustId,
                IndirectSubAccId = message.IndirectSubAccId,
                IndirectUsername = message.IndirectUsername,
                ActorName = message.SubAccountUsername,
                ActorId = message.ActorId
            });
        }

        public async Task<ForceChangePasswordViewModel> Handle(ForceChangePasswordQuery message, CancellationToken cancellationToken)
        {
            var profile = await customerProfileService.GetActorProfilePicture(userSession.CustId);

            if (profile == null || string.IsNullOrEmpty(profile.ImageBase64String))
            {
                var result = await customerProfileService.UpdateActorProfilePicture(
                    new UpdateActorProfilePictureParam(userSession.CustId, userSession.UserLoginIp));

                profile = result.IsOk ? await customerProfileService.GetActorProfilePicture(userSession.CustId) : new ActorProfilePictureModel();
            }

            return new ForceChangePasswordViewModel
            {
                HashKey = GetHasKey(),
                IsDefaultAvatar = string.IsNullOrEmpty(profile.ImageBase64String),
                Username = userSession.UserName,
                AlternateUsername = BuildAlternateUsername(message, CancellationToken cancellation),
                UsernameShower = BuildUsernameShower(message, CancellationToken cancellation),
                EmbedImage = profile.ImageBase64String,
                ProfileImageId = profile.ImageId,
                IsShowSkipForceChangePassword = message.IsShowSkipForceChangePassword
            };
        }

        public async Task<PasswordViewModel> Handle(ResetMainAccountPasswordQuery message, CancellationToken cancellationToken)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message, CancellationToken cancellation), "Message cannot be null");
            }

            var viewModel = await Handle(new ChangePasswordQuery(userSession), cancellationToken);

            viewModel.CustId = message.CustId;
            viewModel.CustRoleId = message.CustRoleId;
            viewModel.Title = BuildResetPasswordTitle(message.Username);

            return viewModel;
        }

        public async Task<PasswordViewModel> Handle(ResetSubAccountPasswordQuery message, CancellationToken cancellationToken)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message, CancellationToken cancellation), "Message cannot be null");
            }

            var viewModel = await Handle(new ChangePasswordQuery(userSession), cancellationToken);

            viewModel.SubAccountId = message.SubAccountId;
            viewModel.Title = BuildResetPasswordTitle(message.Username);

            return viewModel;
        }

        private static string BuildResetPasswordTitle(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return Resources.lblresetpassword;
            }

            return $"{Resources.lblresetpassword} - {username}";
        }

        private static string BuildUsernameShower(ForceChangePasswordQuery message, CancellationToken cancellation)
        {
            if (message.IsSubAcc)
            {
                return message.SubUserName;
            }
            else
            {
                return string.IsNullOrEmpty(message.NickName) ? message.UserName : message.NickName;
            }
        }

        private string BuildAlternateUsername(ForceChangePasswordQuery message, CancellationToken cancellation)
        {
            if (message.IsSubAcc)
            {
                return $"({Resources.subAcc.ToUpper()} {message.SubUserName})";
            }
            else
            {
                return string.IsNullOrEmpty(message.NickName) ? string.Empty : $"({message.NickName})";
            }
        }

        private bool HasUpdatePermission(bool isResetPassword = false, bool isResetSubAccount = false)
        {
            if (userSession.IsInternal && !isResetSubAccount)
            {
                return false;
            }

            if (userSession.IsLimitedHostPermissionOnSubAccountFunction)
            {
                return false;
            }

            if (isResetPassword
                && (userSession.IsSubAcc && !userSession.SubPageAccess.UpdateAccount))
            {
                return false;
            }

            if (isResetSubAccount && !user.IsAdminCanUpdateSubAccount)
            {
                return false;
            }

            return true;
        }
    }
}
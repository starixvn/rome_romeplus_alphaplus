using AlphaPlus.Settings.Site.Features.MemberInfo.Features.QRCodeMemberSignIn.Helpers;
using AlphaPlus.Settings.Site.Features.MemberInfo.Features.QRCodeMemberSignIn.Requests;
using AlphaPlus.Settings.Site.Features.MemberInfo.Features.QRCodeMemberSignIn.ViewModels;
using AlphaPlus.Settings.MemberInfo.API.Features.QRCodeMemberSignIn;
using AlphaPlus.Settings.MemberInfo.API.Features.QRCodeMemberSignIn.Criteria;
using AlphaPlus.Settings.MemberInfo.API.Features.Shared;
using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
using AlphaPlus.Settings.MemberInfo.Domain.Enumerations;
using AlphaPlus.Settings.MemberInfo.Domain.Features.QRCodeMemberSignIn.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Features.Shared.Params;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.QRCodeMemberSignIn
{
    public class QRCodeMemberSignInHandler :
        IRequestHandler<QRCodeMemberSignInRequest, QRCodeMemberSignInViewModel>,
        IRequestHandler<CreateAccountByQRCodeRequest, ResultCreateMemberQRCode>,
        IRequestHandler<GetMemberCreditAndStatusRequest, CreditAndStatusModel>,
        IRequestHandler<ShowQRCodeRequest, ShowQRCodeViewModel>,
        IRequestHandler<GenerateQRCodeRequest, QRCodeLinkViewModel>,
        IRequestHandler<CheckExistedCustomerRequest, bool>
    {
        private readonly QRCodeLoginConfigsHelper qrCodeLoginConfigsHelper;
        private readonly QrCodeAdapter qrCodeAdapter;
        private readonly QRCodeMemberSignInService qrCodeMemberSignInService;
        private readonly PasswordService passwordService;
        private readonly CheckBelongingService checkBelongingService;

        public QRCodeMemberSignInHandler(
           QrCodeAdapter qrCodeAdapter,
           QRCodeMemberSignInService qrCodeMemberSignInService,
           PasswordService passwordService,
           CheckBelongingService checkBelongingService)
            : this(qrCodeAdapter, qrCodeMemberSignInService, passwordService, checkBelongingService, null)
        {
        }

        internal QRCodeMemberSignInHandler(
            QrCodeAdapter qrCodeAdapter,
            QRCodeMemberSignInService qrCodeMemberSignInService,
            PasswordService passwordService,
            CheckBelongingService checkBelongingService,
            QRCodeLoginConfigsHelper qrCodeLoginConfigsHelper)
        {
            this.qrCodeAdapter = qrCodeAdapter;
            this.qrCodeMemberSignInService = qrCodeMemberSignInService;
            this.passwordService = passwordService;
            this.checkBelongingService = checkBelongingService;
            this.qrCodeLoginConfigsHelper = qrCodeLoginConfigsHelper;
        }

        public async Task<QRCodeMemberSignInViewModel> Handle(QRCodeMemberSignInRequest message, CancellationToken cancellation)
        {
            var onlyShowErrorMessage = false;

            if (string.IsNullOrWhiteSpace(message.ErrorMessage))
            {
                var limitNewDownlineResult = qrCodeMemberSignInService.CheckLimitNewDownline(
                    message.ViewerId,
                    message.ViewerLevelId,
                    message.Language,
                    message.ActorUsername);
                onlyShowErrorMessage = limitNewDownlineResult.IsFailed;
                message.ErrorMessage = limitNewDownlineResult.Message;
            }

            return new QRCodeMemberSignInViewModel(
                message,
                qrCodeMemberSignInService.GetSuggestUsername(message.CustName, message.CustId),
                onlyShowErrorMessage);
        }

        public async Task<ResultCreateMemberQRCode> Handle(CreateAccountByQRCodeRequest message, CancellationToken cancellation)
        {
            var creditAndStatusModel = qrCodeMemberSignInService.GetCreditAndStatus(message.UsernameCopyFrom);

            if (creditAndStatusModel.Status != null &&
              !creditAndStatusModel.Status.Equals("OPEN", StringComparison.OrdinalIgnoreCase))
            {
                return new ResultCreateMemberQRCode
                {
                    Success = false,
                    ErrCode = 100,
                    ErrMsg = Resources.CannotCreateClosedMemberQRCode
                };
            }

            var model = qrCodeMemberSignInService.CheckBelongingByUsername(message.CustId, message.UsernameCopyFrom, message.SubAccId);
            var custIdCopyFrom = model.CustId;

            if (custIdCopyFrom > 0 &&
                Enumeration.FromValue<UserLevel>((byte)model.RoleId).IsMemberLevel() &&
                qrCodeMemberSignInService.IsEnableCustomer(custIdCopyFrom))
            {
                var passwordModel = passwordService.CreateMemberPasswordRandom();

                var result = await qrCodeMemberSignInService.CreateMemberByQRCode(
                    new InsertMemberCommand(
                        message.Username,
                        message.FirstName ?? string.Empty,
                        message.CustId,
                        custIdCopyFrom,
                        message.IP,
                        message.Language,
                        message.SubAccId,
                        message.Credit,
                        passwordModel.RawPassword,
                        message.UsernameCopyFrom,
                        message.CreditMax,
                        message.SubUsername,
                        message.SiteDBGroupIp,
                        message.ActorUsername)).ConfigureAwait(false);

                result.CustIdCopyFrom = custIdCopyFrom;

                if (result.Success)
                {
                    result.RawPassword = passwordModel.RawPassword;
                }

                return result;
            }

            return new ResultCreateMemberQRCode
            {
                Success = false,
                ErrCode = 100,
                ErrMsg = Resources.UsernameIsInvalid
            };
        }

        public async Task<CreditAndStatusModel> Handle(GetMemberCreditAndStatusRequest message, CancellationToken cancellation)
            => IsValidBelongingByUsername(
                message.CustId,
                message.SearchAccountName,
                message.SubAccId) ?
                qrCodeMemberSignInService.GetCreditAndStatus(message.SearchAccountName) :
                new CreditAndStatusModel();

        public async Task<ShowQRCodeViewModel> Handle(ShowQRCodeRequest message, CancellationToken cancellation)
        {
            var belongingInfo = checkBelongingService.GetBelongingInfo(
                new CheckBelongingParam(message.ViewerId, message.SearchAccountId, message.SubAccId));

            if (belongingInfo.IsValid(message.ViewerLevelId)
                && qrCodeMemberSignInService.IsNotFirstLoginByQRCode(message.SearchAccountId))
            {
                var rawPassword = message.RawPassword;

                if (!message.CreateMode)
                {
                    var passwordModel = passwordService.CreateMemberPasswordRandom();
                    rawPassword = passwordModel.RawPassword;

                    qrCodeMemberSignInService.ResetMemberPassword(new ResetMainAccountPasswordCommand
                    {
                        CustId = message.SearchAccountId,
                        UserPwd = passwordModel.EncodePassword,
                        UserId = message.CustId,
                        Language = message.Language,
                        SubAccId = message.SubAccId,
                        LoginIP = message.IP
                    });
                }

                var config = qrCodeLoginConfigsHelper.GetQRCodeMemberSiteLoginConfigs();
                var link = qrCodeAdapter.CreateMobileLoginUrl(
                         qrCodeLoginConfigsHelper.LocalTest(),
                        config.Domains[0],
                        config.SiteId,
                        message.SearchAccountName,
                        config.ExpireSeconds);

                return new ShowQRCodeViewModel(
                    message.Credit,
                    message.CreditMin,
                    message.CreditMax,
                    message.SearchAccountName,
                    message.SearchAccountId,
                    message.FirstName,
                    link,
                    qrCodeAdapter.Generate(link),
                    config.Domains,
                    message.CreateMode,
                    message.ShowPageTitle,
                    rawPassword);
            }

            return null;
        }

        public async Task<QRCodeLinkViewModel> Handle(GenerateQRCodeRequest message, CancellationToken cancellation)
        {
            if (qrCodeMemberSignInService.IsNotFirstLoginByQRCode(message.SearchAccountId))
            {
                var config = qrCodeLoginConfigsHelper.GetQRCodeMemberSiteLoginConfigs();

                var link = qrCodeAdapter.CreateMobileLoginUrl(
                    qrCodeLoginConfigsHelper.LocalTest(),
                    message.Domain,
                    config.SiteId,
                    message.SearchAccountName,
                    config.ExpireSeconds);

                return new QRCodeLinkViewModel(link, qrCodeAdapter.Generate(link));
            }

            return new QRCodeLinkViewModel();
        }

        public async Task<bool> Handle(CheckExistedCustomerRequest message, CancellationToken cancellation)
            => IsValidBelongingByUsername(
                message.CustId,
                message.Username,
                message.SubAccId) &&
            qrCodeMemberSignInService.CheckExistedCustomer(message.Username);

        internal bool IsValidBelongingByUsername(int custId, string username, int subAccountId)
        {
            var model = qrCodeMemberSignInService.CheckBelongingByUsername(custId, username, subAccountId);

            return model.CustId > 0 &&
                Enumeration.FromValue<UserLevel>((byte)model.RoleId).IsMemberLevel();
        }
    }
}
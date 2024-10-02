using AlphaPlus.Settings.Site.Features.MemberInfo.Features.QRCodeMemberSignIn.Helpers;
using AlphaPlus.Settings.Site.Features.MemberInfo.Features.QRCodeMemberSignIn.Requests;
using AlphaPlus.Settings.Site.Features.MemberInfo.Features.QRCodeMemberSignIn.ViewModels;
using AlphaPlus.Settings.MemberInfo.API.Features.QRCodeMemberSignIn;
using AlphaPlus.Settings.MemberInfo.API.Features.QRCodeMemberSignIn.Criteria;
using AlphaPlus.Settings.MemberInfo.API.Features.Shared;
using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
using AlphaPlus.Settings.MemberInfo.Domain.Features.QRCodeMemberSignIn.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Features.Shared.Params;
using MediatR;
using AlphaPlus.Core.Models;
using Fanex.Customer.Client.Services;
using Fanex.Customer.Client.Params;

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
        private readonly ICustomerService customerService;
        private readonly PasswordService passwordService;
        private readonly CheckBelongingService checkBelongingService;

        public QRCodeMemberSignInHandler(
           QrCodeAdapter qrCodeAdapter,
           ICustomerService customerService,
           QRCodeMemberSignInService qrCodeMemberSignInService,
           PasswordService passwordService,
           CheckBelongingService checkBelongingService)
            : this(qrCodeAdapter, customerService, qrCodeMemberSignInService, passwordService, checkBelongingService, null)
        {
        }

        internal QRCodeMemberSignInHandler(
            QrCodeAdapter qrCodeAdapter,
            ICustomerService customerService,
            QRCodeMemberSignInService qrCodeMemberSignInService,
            PasswordService passwordService,
            CheckBelongingService checkBelongingService,
            QRCodeLoginConfigsHelper qrCodeLoginConfigsHelper)
        {
            this.qrCodeAdapter = qrCodeAdapter;
            this.customerService = customerService;
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
                await customerService.GetSuggestUsername(message.CustName, message.CustLevelId),
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

            var model = await customerService.GetDownlineByUsername(message.CustId, message.SubAccId, message.UsernameCopyFrom);
            var custIdCopyFrom = model.CustId;

            if (custIdCopyFrom > 0 &&
                Enumeration.FromValue<UserLevel>((byte)model.RoleId).IsMemberLevel() &&
                qrCodeMemberSignInService.IsEnableCustomer(custIdCopyFrom))
            {
                var passwordModel = passwordService.CreateMemberPasswordRandom();

                var param = new CopyCustomerParam(
                    message.ViewerId,
                    message.SubAccId,
                    message.CustId,
                    message.CustLevel.GetDirectDownLineLevel().Value,
                    message.ActorUsername,
                    message.Username,
                    passwordModel.EncodePassword,
                    message.Credit,
                    message.CreditMax,
                    custIdCopyFrom,
                    message.Site,
                    true,
                    message.IP,
                    message.Language)
                {
                    FirstName = message.FirstName
                };

                var result = await customerService.CopyCustomer(param).ConfigureAwait(false);

                return new ResultCreateMemberQRCode
                {
                    CustIdCopyFrom = custIdCopyFrom,
                    CustId = result.CustId,
                    ErrCode = result.Code,
                    ErrMsg = result.Message,
                    Success = result.IsSuccessful,
                    RawPassword = result.IsSuccessful ? passwordModel.RawPassword : string.Empty,
                    MsgType = result.MsgType,
                    MessageContent = result.MessageContent,
                };
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
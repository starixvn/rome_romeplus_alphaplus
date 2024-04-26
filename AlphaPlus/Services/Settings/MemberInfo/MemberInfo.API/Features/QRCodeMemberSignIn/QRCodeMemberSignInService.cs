namespace AlphaPlus.Settings.MemberInfo.API.Features.QRCodeMemberSignIn
{
    using System.Threading.Tasks;
    using Alpha.CopyAccount.Params;
    using Alpha.CopyAccount.Services;
    using AlphaPlus.Core.Models;
    using AlphaPlus.Settings.MemberInfo.API.Features.QRCodeMemberSignIn.Criteria;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.QRCodeMemberSignIn.Models;
    using Fanex.Data.Repository;

    public interface QRCodeMemberSignInService
    {
        CreditAndStatusModel GetCreditAndStatus(string SearchAccountName);

        Task<ResultCreateMemberQRCode> CreateMemberByQRCode(InsertMemberCommand command);

        CheckBelongingByUsernameModel CheckBelongingByUsername(int actorId, string username, int subAccountId);

        bool IsEnableCustomer(int custId);

        string GetSuggestUsername(string prefix, int custId);

        SelectRnGStatusModel GetRnGStatus(string username);

        bool IsNotFirstLoginByQRCode(int custId);

        Result ResetMemberPassword(ResetMainAccountPasswordCommand command);

        bool CheckExistedCustomer(string username);

        Result CheckLimitNewDownline(int actorId, byte actorRoleId, string language, string actorUsername);
    }

    public class QRCodeMemberSignInServiceImpl : QRCodeMemberSignInService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly ICopyAccountService copyAccountService;
        private const byte AlphaSite = 2;

        public QRCodeMemberSignInServiceImpl(IDynamicRepository dynamicRepository, ICopyAccountService copyAccountService)
        {
            this.dynamicRepository = dynamicRepository;
            this.copyAccountService = copyAccountService;
        }

        public CreditAndStatusModel GetCreditAndStatus(string SearchAccountName)
            => dynamicRepository.Get<CreditAndStatusModel>(new SelectCreditAndStatusCriteria(SearchAccountName));

        public async Task<ResultCreateMemberQRCode> CreateMemberByQRCode(InsertMemberCommand command)
        {
            var result = await copyAccountService.CopyAsync(new CopyAccountParams(
                command.ActorId,
                UserLevel.Agent.Value,
                command.SubAccId,
                command.SubUsername,
                command.Username,
                command.Password,
                command.Phone,
                command.MobilePhone,
                command.Fax,
                command.Credit,
                command.CreditMax,
                command.CopyFromCustId,
                command.CopyFromUsername,
                command.Language,
                command.IP,
                AlphaSite,
                isQrCode: true,
                actorUsername: command.ActorUsername,
                siteDBGroupIp: command.SiteDBGroupIp,
                firstName: command.FirstName)).ConfigureAwait(false);

            if (result != null)
            {
                return new ResultCreateMemberQRCode
                {
                    CustId = result.CustId,
                    ErrCode = result.Code,
                    ErrMsg = result.Message,
                    RawPassword = result.Password,
                    Success = result.IsSuccess,
                    CustIdCopyFrom = command.CopyFromCustId
                };
            }

            return new ResultCreateMemberQRCode();
        }

        public CheckBelongingByUsernameModel CheckBelongingByUsername(int actorId, string username, int subAccountId)
        {
            var criteria = new CheckBelongingByUsernameCritieria(actorId, username, subAccountId);
            dynamicRepository.Execute(criteria);
            return new CheckBelongingByUsernameModel(criteria.IsActive, criteria.CustId, criteria.IsSubAcc, criteria.RoleId);
        }

        public bool IsEnableCustomer(int custId)
        {
            var criteria = new CheckEnableCustomerCritieria(custId);
            dynamicRepository.Execute(criteria);
            return criteria.ErrCode.Equals(0);
        }

        public string GetSuggestUsername(string prefix, int custId)
        {
            var criteria = new SelectSuggestUsernameCriteria(prefix, custId);
            dynamicRepository.Execute(criteria);
            return criteria.SeqName ?? string.Empty;
        }

        public SelectRnGStatusModel GetRnGStatus(string username)
            => dynamicRepository.Get<SelectRnGStatusModel>(new SelectRnGStatusCriteria(username));

        public Result ResetMemberPassword(ResetMainAccountPasswordCommand command)
        {
            dynamicRepository.Execute(command);
            return Result.CreateErrorResult(command.ErrMsg, command.ErrCode);
        }

        public bool IsNotFirstLoginByQRCode(int custId)
          => dynamicRepository.Get<bool>(new SelectFirstLoginByQRCodeCriteria(custId));

        public bool CheckExistedCustomer(string username)
        {
            var criteria = new CheckExistedCustomerCritieria(username);
            dynamicRepository.Execute(criteria);
            return criteria.IsExists;
        }

        public Result CheckLimitNewDownline(int actorId, byte actorRoleId, string language, string actorUsername)
        {
            var limitNewDownlineResult = copyAccountService.CheckLimitNewDownline(
                new CheckLimitNewDownlineParam(actorId, actorRoleId, language, actorUsername));

            return new Result { Code = limitNewDownlineResult.Code, Message = limitNewDownlineResult.Message };
        }
    }
}

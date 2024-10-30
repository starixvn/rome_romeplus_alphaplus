using AlphaPlus.Core.Models;
using AlphaPlus.Settings.MemberInfo.API.Features.QRCodeMemberSignIn.Criteria;
using AlphaPlus.Settings.MemberInfo.Domain.Features.QRCodeMemberSignIn.Models;
using Fanex.Customer.Client.Services;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Features.QRCodeMemberSignIn
{
    public interface QRCodeMemberSignInService
    {
        CreditAndStatusModel GetCreditAndStatus(string SearchAccountName);

        bool IsNotFirstLoginByQRCode(int custId);

        Result ResetMemberPassword(ResetMainAccountPasswordCommand command);
    }

    public class QRCodeMemberSignInServiceImpl : QRCodeMemberSignInService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly ICustomerService customerService;

        public QRCodeMemberSignInServiceImpl(IDynamicRepository dynamicRepository,
            ICustomerService customerService)
        {
            this.dynamicRepository = dynamicRepository;
            this.customerService = customerService;
        }

        public CreditAndStatusModel GetCreditAndStatus(string SearchAccountName)
            => dynamicRepository.Get<CreditAndStatusModel>(new SelectCreditAndStatusCriteria(SearchAccountName));

        public Result ResetMemberPassword(ResetMainAccountPasswordCommand command)
        {
            dynamicRepository.Execute(command);
            return Result.CreateErrorResult(command.ErrMsg, command.ErrCode);
        }

        public bool IsNotFirstLoginByQRCode(int custId) => dynamicRepository.Get<bool>(new SelectFirstLoginByQRCodeCriteria(custId));
    }
}

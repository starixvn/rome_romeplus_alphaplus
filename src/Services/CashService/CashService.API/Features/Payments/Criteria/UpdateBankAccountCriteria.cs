using Fanex.Data;
using AlphaPlus.CashService.API.Features.Payments.Requests;
using AlphaPlus.CashService.API.Features.Shared.Criteria;
using AlphaPlus.CashService.API.Features.Shared.Helpers;

namespace AlphaPlus.CashService.API.Features.Payments.Criteria
{
    public class UpdateBankAccountCriteria(UpdateBankAccountRequest request) 
        : BaseDBCommand(request.ActorId, request.SubAccId, request.IP, request.Language)
    {
        [SpParam(SqlParamName = "AgentBankAccountID")]
        public int Id { get; } = request.Id;

        [SpParam(SqlParamName = "BankAccountStatus")]
        public int Status { get; } = request.Status;

        public string AccountNickname { get; } = request.AccountNickname;

        [SpParam(SqlParamName = "TransTypeList")]
        public string TransactionTypeIDs {get;} = request.Agent_TransTypeList;

        public int DepositMatchingMode {get;} = request.Agent_DepositMatchingMode;

        [SpParam(SqlParamName = "PromptPayEnabled")]
        public bool PromptPay { get; } = request.PromptPay;

        [SpParam(SqlParamName = "PromptPayMobile")]
        public string Mobile { get; } = request.PromptPay ? request.CountryCode + "-" + request.Mobile : string.Empty;

        [SpParam(Ignored = true)]
        public string CountryCode { get; } = request.CountryCode;
        [SpParam(Ignored = true)]
        public string MobileNumber { get; }= request.Mobile;

        public override string GetSettingKey() => "Age_CashService_BankAccountAgent_Upd";

        public override bool IsValid() {
            var isValid = base.IsValid() && 
            Id > 0 &&
            ValidatorHelper.IsValidAccountNickname(AccountNickname);

            if(!isValid || !PromptPay) {
                return isValid;
            }

            return ValidatorHelper.IsValidCountryCode(CountryCode) && ValidatorHelper.IsValidMobileNumber(MobileNumber);
        }
    }
} 
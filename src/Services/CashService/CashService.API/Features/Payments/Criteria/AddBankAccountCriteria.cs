using AlphaPlus.CashService.API.Features.Payments.Requests;
using AlphaPlus.CashService.API.Features.Shared.Criteria;
using AlphaPlus.CashService.API.Features.Shared.Helpers;
using Fanex.Data;

namespace AlphaPlus.CashService.API.Features.Payments.Criteria
{
    public class AddBankAccountCriteria : BaseDBCommand
    {
        public AddBankAccountCriteria(AddBankAccountRequest request) :
            base(request.ActorId, request.SubAccId, request.IP, request.Language)
        {
            CreatorName = request.Creator;
            BankId = request.BankId;
            Status = request.Status;
            AccountNickname = request.AccountNickname;
            TransactionTypeIDs = request.Agent_TransTypeList;
            PromptPay = request.PromptPay;

            AccountHolderName = request.AccountHolderName;
            AccountNumber = request.AccountNumber;
            Mobile = PromptPay ? request.CountryCode + "-" + request.Mobile : string.Empty;
            DepositMatchingMode = request.Agent_DepositMatchingMode;

            CountryCode = request.CountryCode;
            MobileNumber = request.Mobile;
        }

        [SpParam(SqlParamName = "Username")]
        public string CreatorName { get; }

        [SpParam(SqlParamName = "BankId")]
        public int BankId { get; }

        [SpParam(SqlParamName = "BankAccountStatus")]
        public int Status { get; }

        public string AccountHolderName { get; }

        [SpParam(SqlParamName = "BankAccountNumber")]
        public string AccountNumber { get; }

        public string AccountNickname { get; }

        [SpParam(SqlParamName = "TransTypeList")]
        public string TransactionTypeIDs { get; }

        public int DepositMatchingMode { get; }

        [SpParam(SqlParamName = "PromptPayEnabled")]
        public bool PromptPay { get; }

        [SpParam(SqlParamName = "PromptPayMobile")]
        public string Mobile { get; }

        [SpParam(Ignored = true)]
        public string CountryCode { get; }

        [SpParam(Ignored = true)]
        public string MobileNumber { get; }

        public override string GetSettingKey() => "Age_CashService_BankAccountAgent_Ins";

        public override bool IsValid()
        {
            var isValid = base.IsValid() &&
                IsValidBankAccountInfo() &&
                ValidatorHelper.IsValidAccountNickname(AccountNickname);

            if (!isValid || !PromptPay)
            {
                return isValid;
            }

            return ValidatorHelper.IsValidCountryCode(CountryCode) && 
                   ValidatorHelper.IsValidMobileNumber(MobileNumber);
        }

        private bool IsValidBankAccountInfo() =>
            BankId > 0 &&
            ValidatorHelper.IsValidAccountHolderName(AccountHolderName) &&
            ValidatorHelper.IsValidAccountNumber(AccountNumber);
    }
}
using Fanex.Data;
using AlphaPlus.CashService.API.Features.Payments.Requests;
using AlphaPlus.CashService.API.Features.Shared.Criteria;
using System.Text.RegularExpressions;

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
            Mobile = request.CountryCode + "-" + request.Mobile;
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
        public string TransactionTypeIDs {get; }
        public int DepositMatchingMode {get; }

        [SpParam(SqlParamName = "PromptPayEnabled")]
        public bool PromptPay { get; }
        [SpParam(SqlParamName = "PromptPayMobile")]
        public string Mobile { get; }

        [SpParam(Ignored = true)]
        public string CountryCode { get; }
        [SpParam(Ignored = true)]
        public string MobileNumber { get; }

        public override string GetSettingKey() => "Age_CashService_BankAccountAgent_Ins";

        public override bool IsValid() {
            var isValid = base.IsValid() &&
            BankId > 0 && 
            !string.IsNullOrWhiteSpace(AccountNumber) && 
            !string.IsNullOrWhiteSpace(AccountHolderName) &&
            Regex.IsMatch(AccountHolderName, @"^[a-zA-Z\s.]+$") &&
            Regex.IsMatch(AccountNumber, @"^\d+$") &&
            AccountHolderName.Length <= 255 &&
            AccountNumber.Length <= 100 &&
            AccountNickname.Length <= 255;

            if(!isValid || !PromptPay) {
                return isValid;
            }

            return !string.IsNullOrWhiteSpace(CountryCode) 
                && !string.IsNullOrWhiteSpace(MobileNumber) 
                && Regex.IsMatch(CountryCode, @"^\d+$") 
                && Regex.IsMatch(MobileNumber, @"^\d+$");
        }
    }
} 
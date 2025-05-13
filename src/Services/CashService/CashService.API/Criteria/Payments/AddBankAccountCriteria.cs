using Fanex.Data;
using AlphaPlus.CashService.API.Requests.Payments;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class AddBankAccountCriteria : BaseActionCommand
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

        public override string GetSettingKey() => "Age_CashService_BankAccountAgent_Ins";

        public override bool IsValid() => 
            base.IsValid() &&
            BankId > 0 && 
            !string.IsNullOrWhiteSpace(AccountNumber) && 
            !string.IsNullOrWhiteSpace(AccountHolderName) &&
            !string.IsNullOrWhiteSpace(AccountNickname);
    }
} 
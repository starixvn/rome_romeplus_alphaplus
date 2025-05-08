using Fanex.Data;
using Fanex.Data.Repository;
using AlphaPlus.CashService.API.Models.Payments;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class AddBankAccountCriteria : NonQueryCommand
    {
        public AddBankAccountCriteria(BankAccountModel bankAccModel)
        {
            Creator = bankAccModel.CreatorId;
            BankId = bankAccModel.BankId;
            Status = bankAccModel.Status;
            AccountNickname = bankAccModel.AccountNickname;
            TransactionTypeIDs = bankAccModel.Agent_TransTypeList;
            PromptPay = bankAccModel.PromptPay;

            AccountHolderName = bankAccModel.AccountHolderName;
            AccountNumber = bankAccModel.AccountNumber;
            Mobile = bankAccModel.CountryCode + "-" + bankAccModel.Mobile;
        }

        [SpParam(SqlParamName = "CustId")]
        public int Creator { get; }

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

        public int ErrCode { get; set; }
        public string ErrMsg { get; set; } = string.Empty;

        public override string GetSettingKey() => "Age_CashService_BankAccountAgent_Ins";

        public override bool IsValid() => 
            BankId > 0 && 
            !string.IsNullOrWhiteSpace(AccountNumber) && 
            Creator > 0 &&
            !string.IsNullOrWhiteSpace(AccountHolderName) &&
            !string.IsNullOrWhiteSpace(AccountNickname);
    }
} 
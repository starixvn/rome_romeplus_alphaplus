using Fanex.Data;
using Fanex.Data.Repository;
using AlphaPlus.CashService.API.Models.Payments;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class AddBankAccountCriteria(BankAccountModel bankAccModel) : NonQueryCommand
    {
        [SpParam(SqlParamName = "CustId")]
        public int Creator { get; } = bankAccModel.CreatorId;

        [SpParam(SqlParamName = "BankAccountStatus")]
        public int Status { get; } = bankAccModel.Status;

        [SpParam(SqlParamName = "BankId")]
        public int BankId { get; } = bankAccModel.BankId;

        public string AccountHolderName { get; } = bankAccModel.AccountHolderName;

        [SpParam(SqlParamName = "BankAccountNumber")]
        public string AccountNumber { get; } = bankAccModel.AccountNumber;

        public string AccountNickname { get; } = bankAccModel.AccountNickname;

        [SpParam(SqlParamName = "TransactionTypeID")]
        public string TransactionTypeIDs {get;} = bankAccModel.TransactionTypes;

        public int DepositMatchingMode {get;}

        [SpParam(SqlParamName = "PromptPayEnabled")]
        public bool PromptPay { get; } = bankAccModel.PromptPay;

        [SpParam(SqlParamName = "PromptPayMobile")]
        public string Mobile { get; } = bankAccModel.Mobile;

        public string CountryCode { get; }


        public int ErrCode { get; set; }
        public string ErrMsg { get; set; } = string.Empty;

        public override string GetSettingKey() => "Age_CashService_BankAccountAgent_Ins";

        public override bool IsValid() => 
            BankId > 0 && 
            !string.IsNullOrWhiteSpace(AccountNumber) && 
            Creator > 0 &&
            !string.IsNullOrWhiteSpace(AccountHolderName) &&
            !string.IsNullOrWhiteSpace(AccountNickname) &&
            !string.IsNullOrWhiteSpace(TransactionTypeIDs);
    }
} 
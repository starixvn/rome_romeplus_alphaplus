using Fanex.Data;
using Fanex.Data.Repository;
using AlphaPlus.CashService.API.Models.Payments;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class UpdateBankAccountCriteria : NonQueryCommand
    {
        public UpdateBankAccountCriteria(BankAccountModel bankAccModel)
        {
            BankAccountId = bankAccModel.Id;
            Status = bankAccModel.Status;
            BankId = bankAccModel.BankId;

            DepositType = bankAccModel.DepositType;
            WithdrawalType = bankAccModel.WithdrawalType;
            DepositMatchingMode = bankAccModel.DepositMatchingMode;

            AccountNumber = bankAccModel.AccountNumber;
            Creator = bankAccModel.CreatorId;

            AccountHolderName = bankAccModel.AccountHolderName;
            AccountNickname = bankAccModel.AccountNickname;
            
            PromptPay = bankAccModel.PromptPay;
            Mobile = bankAccModel.Mobile;
        }

        [SpParam(SqlParamName = "BankAccountId")]
        public int BankAccountId { get; }

        [SpParam(SqlParamName = "Creator")]
        public int Creator { get; }

        [SpParam(SqlParamName = "Status")]
        public int Status { get; }

        [SpParam(SqlParamName = "BankId")]
        public int BankId { get; }

        public int DepositType { get; }
        public int WithdrawalType { get; }
        public int DepositMatchingMode { get; }

        public string AccountHolderName { get; }
        public string AccountNumber { get; }
        public string AccountNickname { get; }

        public bool PromptPay { get; }
        public string Mobile { get; }

        public override string GetSettingKey() => "Age_ManagementPayments_UpdBankAccount";

        public override bool IsValid() => 
            BankAccountId > 0 &&
            BankId > 0 && 
            !string.IsNullOrWhiteSpace(AccountNumber) && 
            Status > 0 && 
            Creator > 0 &&
            !string.IsNullOrWhiteSpace(AccountHolderName) &&
            !string.IsNullOrWhiteSpace(AccountNickname) &&
            DepositType > 0 &&
            WithdrawalType > 0 &&
            DepositMatchingMode > 0;
    }
} 
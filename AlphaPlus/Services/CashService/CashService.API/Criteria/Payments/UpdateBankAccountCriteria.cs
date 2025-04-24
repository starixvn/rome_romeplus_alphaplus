using Fanex.Data;
using Fanex.Data.Repository;
using AlphaPlus.CashService.API.Models.Payments;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class UpdateBankAccountCriteria : NonQueryCommand
    {
        public UpdateBankAccountCriteria(BankAccountModel bankAccModel)
        {
            Creator = bankAccModel.CreatorId;
            BankId = bankAccModel.BankId;
            Status = bankAccModel.Status;

            AccountNickname = bankAccModel.AccountNickname;

            TransactionTypeIDs = bankAccModel.TransactionTypes;
            DepositMatchingMode = bankAccModel.DepositMatchingMode;

            PromptPay = bankAccModel.PromptPay;
            Mobile = bankAccModel.Mobile;
        }

       [SpParam(SqlParamName = "CustId")]
        public int Creator { get; }

        [SpParam(SqlParamName = "Status")]
        public int Status { get; }

        [SpParam(SqlParamName = "BankId")]
        public int BankId { get; }

        public string AccountNickname { get; }

        [SpParam(SqlParamName = "TransactionTypeID")]
        public string TransactionTypeIDs {get;}
        public int DepositMatchingMode {get;}

        [SpParam(SqlParamName = "PromptPayEnabled")]
        public bool PromptPay { get; }
        [SpParam(SqlParamName = "PromptPayMobile")]
        public string Mobile { get; }

        public int ErrCode { get; set; }
        public string ErrMsg { get; set; } = string.Empty;

        public override string GetSettingKey() => "Age_CashService_BankAccountAgent_Upd";

        public override bool IsValid() => 
            BankId > 0 && 
            Status > 0 && 
            Creator > 0 &&
            !string.IsNullOrWhiteSpace(AccountNickname) &&
            !string.IsNullOrWhiteSpace(TransactionTypeIDs) &&
            DepositMatchingMode > 0;
    }
} 
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
            Id = bankAccModel.Id;
            Status = bankAccModel.Status;
            AccountNickname = bankAccModel.AccountNickname;

            TransactionTypeIDs = bankAccModel.Agent_TransTypeList;
            DepositMatchingMode = bankAccModel.Agent_DepositMatchingMode;

            MobilePrefix = bankAccModel.MobilePrefix;
            PromptPay = bankAccModel.PromptPay;
            Mobile = bankAccModel.Mobile;
        }

       [SpParam(SqlParamName = "CustId")]
        public int Creator { get; }

        [SpParam(SqlParamName = "AgentBankAccountID")]
        public int Id { get; }

        [SpParam(SqlParamName = "BankAccountStatus")]
        public int Status { get; }

        public string AccountNickname { get; }

        [SpParam(SqlParamName = "TransTypeList")]
        public string TransactionTypeIDs {get;}
        public int DepositMatchingMode {get;}

        [SpParam(SqlParamName = "CountryCode")]
        public string MobilePrefix { get; }
        [SpParam(SqlParamName = "PromptPayEnabled")]
        public bool PromptPay { get; }
        [SpParam(SqlParamName = "PromptPayMobile")]
        public string Mobile { get; }

        public int ErrCode { get; set; }
        public string ErrMsg { get; set; } = string.Empty;

        public override string GetSettingKey() => "Age_CashService_BankAccountAgent_Upd";

        public override bool IsValid() => 
            Id > 0 && 
            Status > 0 && 
            Creator > 0 &&
            !string.IsNullOrWhiteSpace(AccountNickname) &&
            !string.IsNullOrWhiteSpace(TransactionTypeIDs) &&
            DepositMatchingMode > 0;
    }
} 
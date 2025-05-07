using Fanex.Data;
using Fanex.Data.Repository;
using AlphaPlus.CashService.API.Models.Payments;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class UpdateBankAccountCriteria(BankAccountModel bankAccModel) : NonQueryCommand
    {
        [SpParam(SqlParamName = "CustId")]
        public int Creator { get; } = bankAccModel.CreatorId;

        [SpParam(SqlParamName = "AgentBankAccountID")]
        public int Id { get; } = bankAccModel.Id;

        [SpParam(SqlParamName = "BankAccountStatus")]
        public int Status { get; } = bankAccModel.Status;

        public string AccountNickname { get; } = bankAccModel.AccountNickname;

        [SpParam(SqlParamName = "TransTypeList")]
        public string TransactionTypeIDs {get;} = bankAccModel.Agent_TransTypeList;

        public int DepositMatchingMode {get;} = bankAccModel.Agent_DepositMatchingMode;

        public string CountryCode { get; } = bankAccModel.CountryCode;

        [SpParam(SqlParamName = "PromptPayEnabled")]
        public bool PromptPay { get; } = bankAccModel.PromptPay;

        [SpParam(SqlParamName = "PromptPayMobile")]
        public string Mobile { get; } = bankAccModel.Mobile;

        public int ErrCode { get; set; }
        public string ErrMsg { get; set; } = string.Empty;

        public override string GetSettingKey() => "Age_CashService_BankAccountAgent_Upd";

        public override bool IsValid() => 
            Id > 0 && 
            Creator > 0 &&
            !string.IsNullOrWhiteSpace(AccountNickname);
    }
} 
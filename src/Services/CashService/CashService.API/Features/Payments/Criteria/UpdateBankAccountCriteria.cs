using Fanex.Data;
using AlphaPlus.CashService.API.Features.Payments.Requests;
using AlphaPlus.CashService.API.Features.Shared.Criteria;

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
        public string Mobile { get; } = request.CountryCode + "-" + request.Mobile;

        public override string GetSettingKey() => "Age_CashService_BankAccountAgent_Upd";

        public override bool IsValid()
            => base.IsValid() && Id > 0;
    }
} 
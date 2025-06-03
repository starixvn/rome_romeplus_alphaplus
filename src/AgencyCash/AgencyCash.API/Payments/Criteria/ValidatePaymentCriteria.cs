using AgencyCash.API.Shared.Helpers;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Criteria;

public class ValidatePaymentCriteria(int agentId, int custId, byte transType, short custDBGroupId) : NonQueryCommand
{
    public int AgentId { get; } = agentId;

    public int CustId { get; } = custId;

    public byte TransType { get; } = transType;

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public override string GetSettingKey()
        => SPNameHelper.CustDBPrefix("API_CashService_CustPaymentValidate", custDBGroupId);

    public override bool IsValid() => AgentId > 0 && CustId > 0;
}
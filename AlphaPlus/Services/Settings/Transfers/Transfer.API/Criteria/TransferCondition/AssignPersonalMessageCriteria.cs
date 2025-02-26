using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.Criteria.TransferCondition;

public class AssignPersonalMessageCriteria(long messageId, int custId, string custName) : NonQueryCommand
{
    public long MessageId { get; } = messageId;

    [SpParam(SqlParamName = "TargetId")]
    public int CustId { get; } = custId;

    [SpParam(SqlParamName = "TargetName")]
    public string CustName { get; } = custName;

    public string BetTransId { get; } = "0";

    public override string GetSettingKey() => "Age_Msg_SetUsrMessage";

    public override bool IsValid() => MessageId > 0;
}
using Fanex.Data;
using Fanex.Data.Repository;

namespace AAgencyCash.API.Notifications.Criteria;

public class AssignPersonalMsgCriteria(int messageId, int custId, string custName) : NonQueryCommand
{
    public int MessageId { get; } = messageId;

    [SpParam(SqlParamName = "TargetId")]
    public int CustId { get; } = custId;

    [SpParam(SqlParamName = "TargetName")]
    public string CustName { get; } = custName;

    public override string GetSettingKey() => "Age_Msg_SetUsrMessage";

    public override bool IsValid() => MessageId > 0 && CustId > 0;
}

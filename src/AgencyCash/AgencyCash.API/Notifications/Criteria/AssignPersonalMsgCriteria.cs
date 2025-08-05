using Fanex.Data.Repository;

namespace AgencyCash.API.Notifications.Criteria;

public class AssignPersonalMsgCriteria(int messageId, int targetId, string targetName) : NonQueryCommand
{
    public int MessageId { get; } = messageId;

    public int TargetId { get; } = targetId;

    public string TargetName { get; } = targetName;

    public override string GetSettingKey() => "Age_Msg_SetUsrMessage";

    public override bool IsValid() => MessageId > 0 && TargetId > 0;
}

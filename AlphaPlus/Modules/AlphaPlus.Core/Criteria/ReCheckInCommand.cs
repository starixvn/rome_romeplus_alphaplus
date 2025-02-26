using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Core.Criteria;

public class ReCheckInCommand(int onlineUserId) : NonQueryCommand
{
    [SpParam(SqlParamName = "on_userid")]
    public int OnlineUserId { get; } = onlineUserId;

    public bool IsKickedOut { get; set; } = false;

    public byte KickOutReason { get; set; } = 0;

    public override string GetSettingKey() => "Age_ReCheckin";

    public override bool IsValid() => true;
}

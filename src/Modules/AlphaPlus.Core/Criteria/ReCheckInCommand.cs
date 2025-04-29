using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Core.Criteria;

public class ReCheckInCommand(int onlineUserId) : NonQueryCommand
{
    [SpParam(SqlParamName = "on_userid")]
    public int OnlineUserId { get; } = onlineUserId;

    public bool IsKickedOut { get; set; }

    public byte KickOutReason { get; set; }

    public override string GetSettingKey() => "Age_ReCheckin";

    public override bool IsValid() => true;
}

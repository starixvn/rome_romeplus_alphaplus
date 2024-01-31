using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Identity.API.Features.Auth
{
    public class ReCheckInCommand(int onlineUserId) : INonQueryCommand
    {
        [SpParam(SqlParamName = "on_userid")]
        public int OnlineUserId { get; } = onlineUserId;

        public bool IsKickedOut { get; set; }

        public byte KickOutReason { get; set; }

        public string GetSettingKey() => "Age_RecheckIn";

        public bool IsValid() => OnlineUserId > 0;
    }
}
using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Identity.API.Features.Auth
{
    public class CheckOutCommand(int onlineUserId, string username, byte roleId, int serverId, string nickname, string reason) : INonQueryCommand
    {
        [SpParam(SqlParamName = "On_UserID")]
        public int OnlineUserId { get; } = onlineUserId;

        public string Username { get; } = username;

        public byte RoleId { get; } = roleId;

        public int ServerId { get; } = serverId;

        public string Nickname { get; } = nickname;

        public string Reason { get; } = reason;

        public string GetSettingKey() => "Age_CheckOut";

        public bool IsValid() => OnlineUserId > 0;
    }
}
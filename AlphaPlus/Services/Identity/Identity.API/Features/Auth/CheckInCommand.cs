using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Identity.API.Features.Auth
{
    public class CheckInCommand : INonQueryCommand
    {
        public int CustId { get; init; }

        public int SubAccId { get; init; }

        public string NetSessionId { get; init; }

        public int ServerId { get; init; }

        public string IP { get; init; }

        [SpParam(SqlParamName = "on_userid")]
        public int OnlineUserId { get; set; }

        public string GetSettingKey() => "Acc_Checkin";

        public bool IsValid() => CustId > 0 && SubAccId >= 0 && !string.IsNullOrWhiteSpace(NetSessionId);
    }
}
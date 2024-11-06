using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.MyWatchlistAccounts.Criteria
{
    public class DeletePersonalPresetCommand : NonQueryCommand
    {
        public DeletePersonalPresetCommand(int agentId, int id, int sporttype = 0)
        {
            AgentId = agentId;
            ID = id;
            Sporttype = sporttype;
        }

        public int AgentId { get; }

        public int ID { get; }

        public int Sporttype { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Acc_WLMonitored_Preset_Del";

        public override bool IsValid() => AgentId > 0;
    }
}

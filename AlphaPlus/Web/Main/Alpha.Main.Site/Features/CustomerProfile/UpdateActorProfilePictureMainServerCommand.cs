using Fanex.Data;
using Fanex.Data.Repository;

namespace Alpha.Main.Site.Features.CustomerProfile
{
    public class UpdateActorProfilePictureMainServerCommand : NonQueryCommand
    {
        public UpdateActorProfilePictureMainServerCommand(int actorId, int mode, string ip)
        {
            ActorId = actorId;
            Mode = mode;
            IP = ip;
            IsMainServer = true;
        }

        [SpParam(SqlParamName = "CustId")]
        public int ActorId { get; }

        public bool IsDefault { get; }

        public bool IsMainServer { get; }

        public int Mode { get; }

        public string IP { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override bool IsValid() => ActorId > 0;

        public override string GetSettingKey() => "Age_CustomerUpdProfilePicture";
    }
}
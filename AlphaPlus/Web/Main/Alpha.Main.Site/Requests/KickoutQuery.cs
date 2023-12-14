namespace Alpha.Main.Site.Requests
{
    using AlphaPlus.Core.Models;
    using MediatR;

    public class KickOutQuery : IRequest<bool>
    {
        public KickOutQuery(IUserSession userSession)
        {
            OnlineUserId = userSession.OnUserId;
            ActorName = userSession.UserName;
            ActorLevelId = userSession.RoleId;
            SubUserName = userSession.SubUserName;
            NickName = userSession.NickName;
            ServerId = userSession.ServerId;
        }

        public int OnlineUserId { get; }

        public string ActorName { get; }

        public byte ActorLevelId { get; }

        public string SubUserName { get; }

        public string NickName { get; }

        public int ServerId { get; }
    }
}
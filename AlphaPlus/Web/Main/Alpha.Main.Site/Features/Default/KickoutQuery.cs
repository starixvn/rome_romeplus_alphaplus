namespace Alpha.Main.Site.Features.Default
{
    using AlphaPlus.Core.Models;
    using MediatR;

    public class KickOutQuery(IUserSession userSession) : IRequest<bool>
    {
        public int OnlineUserId { get; } = userSession.OnUserId;

        public string ActorName { get; } = userSession.UserName;

        public byte ActorLevelId { get; } = userSession.RoleId;

        public string SubUserName { get; } = userSession.SubUserName;

        public string NickName { get; } = userSession.NickName;

        public int ServerId { get; } = userSession.ServerId;
    }
}
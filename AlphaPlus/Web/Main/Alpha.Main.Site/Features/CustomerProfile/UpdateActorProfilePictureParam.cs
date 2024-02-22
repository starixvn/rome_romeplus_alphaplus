namespace Alpha.Main.Site.Features.CustomerProfile
{
    public class UpdateActorProfilePictureParam(int actorId, string ip)
    {
        public int ActorId { get; } = actorId;

        public string IP { get; } = ip;
    }
}
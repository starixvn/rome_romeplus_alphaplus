namespace Alpha.MemberInfo.Site.Features.CreateAccount.ViewModels
{
    public class PTTemplateViewModel
    {
        public PTTemplateViewModel(byte actorLevelId)
        {
            ActorLevelId = actorLevelId;
        }

        public byte ActorLevelId { get; }
    }
}
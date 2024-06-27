namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.CreateAccount.ViewModels
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
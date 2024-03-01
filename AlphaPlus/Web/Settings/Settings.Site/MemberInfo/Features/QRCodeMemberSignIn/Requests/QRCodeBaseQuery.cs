namespace Alpha.MemberInfo.Site.Features.QRCodeMemberSignIn.Requests
{
    using Alpha.MemberInfo.Site.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;

    public class QRCodeBaseQuery : MemberInfoBaseQuery
    {
        public QRCodeBaseQuery(IUser viewer) : base(viewer)
        {
        }

        public override bool IsValid() => 
            base.IsValid() && 
            ActorLevel.IsAgentLevel();
    }
}
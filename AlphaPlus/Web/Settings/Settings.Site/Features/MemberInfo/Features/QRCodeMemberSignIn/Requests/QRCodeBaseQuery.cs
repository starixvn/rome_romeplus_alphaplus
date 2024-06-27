namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.QRCodeMemberSignIn.Requests
{
    using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;

    public class QRCodeBaseQuery : MemberInfoBaseQuery
    {
        public QRCodeBaseQuery() : this(null){}
        
        public QRCodeBaseQuery(IUser viewer) : base(viewer)
        {
        }

        public override bool IsValid() => 
            base.IsValid() && 
            ActorLevel.IsAgentLevel();
    }
}
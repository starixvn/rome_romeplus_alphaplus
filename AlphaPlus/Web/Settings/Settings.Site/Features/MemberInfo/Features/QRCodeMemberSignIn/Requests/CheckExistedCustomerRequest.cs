namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.QRCodeMemberSignIn.Requests
{
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class CheckExistedCustomerRequest : QRCodeBaseQuery, IRequest<bool>
    {
        public CheckExistedCustomerRequest() : this(null){}
        
        public CheckExistedCustomerRequest(IUser viewer) : base(viewer)
        {
        }

        public string Username { get; set; }

        public override bool IsValid() => 
            base.IsValid() && 
            !string.IsNullOrEmpty(Username);
    }
}
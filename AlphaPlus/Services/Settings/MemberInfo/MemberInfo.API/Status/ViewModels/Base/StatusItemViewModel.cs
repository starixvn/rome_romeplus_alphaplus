namespace AlphaPlus.Settings.MemberInfo.API.Status.ViewModels.Base
{
    using AlphaPlus.Settings.MemberInfo.API.Requests;
    using Alpha.Products.Core.Models;
    using Alpha.Products.Core.Shared;
    using System;

    public abstract class StatusItemViewModel : IStatusItemViewModel
    {
        protected readonly ISearchAccountInfoRequest Request;

        protected StatusItemViewModel(ISearchAccountInfoRequest request, int displayOrder = 0)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            Request = request;
#pragma warning disable RECS0021 // Warns about calls to virtual member functions occuring in the constructor
            DisplayOrder = displayOrder;
#pragma warning restore RECS0021 // Warns about calls to virtual member functions occuring in the constructor
        }

        protected ICustomer Actor => Request.Actor;

        protected ICustomer SearchAccount => Request.SearchAccount;

        public virtual string Text => Request.GetResource($"{GetType().Name}Text");

        public abstract string Url { get; }

        public virtual int DisplayOrder { get; }

        public virtual bool Visible => true;

        public virtual bool ShowInCustomerList => false;

        public virtual bool Disabled => !Request.GetData<bool>("Actor.HasUpdateAccountPermission");

        public abstract bool Checked { get; }

        protected virtual string AccountInfoUrl => Request.GetMicroSiteUrl(MicroSiteConstants.AccountInfo, "~/");

        protected virtual string MemberInfoUrl => Request.GetMicroSiteUrl(MicroSiteConstants.MemberInfo, "~/");
    }
}
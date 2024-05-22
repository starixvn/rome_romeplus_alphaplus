using AlphaPlus.Products.Models;
using AlphaPlus.Settings.Site.Features.CustomerList.Requests;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Status.Base
{
    public abstract class StatusItemViewModel : IStatusItemViewModel
    {
        protected readonly ISearchAccountInfoRequest Request;

        protected StatusItemViewModel(ISearchAccountInfoRequest request, int displayOrder = 0)
        {
            Request = request;
            DisplayOrder = displayOrder;
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
    }
}
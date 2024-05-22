using AlphaPlus.Core.Configurations;
using AlphaPlus.Products.Models;
using AlphaPlus.Settings.Site.Features.CustomerList.Requests;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting.Base
{
    public abstract class SettingItemViewModel : ISettingItemViewModel
    {
        protected readonly ISearchAccountInfoRequest Request;

        protected SettingItemViewModel(
            ISearchAccountInfoRequest request,
            int displayOrder = 0)
        {

            Request = request;
            DisplayOrder = displayOrder;
        }

        protected ICustomer Actor => Request.Actor;

        protected ICustomer SearchAccount => Request.SearchAccount;

        public virtual string Text => Request.GetResource($"{GetType().Name}Text");

        public int DisplayOrder { get; }

        public virtual string Url
        {
            get
            {
                var queryString =
                     string.Join("&", QueryStringParamaters.Select(query => $"{query.Key}={query.Value}"));

                return $"{BaseAddress}?{queryString}";
            }
        }

        public virtual string Icon
            => Request.GetData<bool>(DataKey.Actor.HasUpdateAccountPermission)
            ? "icon-edit Enable"
            : "icon-eye View";

        public abstract Dictionary<string, string> QueryStringParamaters { get; }

        protected abstract string BaseAddress { get; }

        public virtual bool Visible => Request.ActorIsEdittingDirectDownline;

        public virtual bool AddEditTextToPopupTitle => true;

        public virtual string Method => DataKey.Method.GET;

        public virtual bool Enabled => true;

        public virtual bool ShowGuideLine => false;

        protected virtual string TransferUrl => AppSettings.TransferAppPath;
    }
}
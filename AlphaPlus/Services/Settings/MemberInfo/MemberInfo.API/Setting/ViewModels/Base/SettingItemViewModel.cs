namespace AlphaPlus.Settings.MemberInfo.API.Setting.ViewModels.Base
{
    using AlphaPlus.Settings.MemberInfo.API.Requests;
    using Alpha.Products.Core.Models;
    using Alpha.Products.Core.Shared;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AlphaPlus.Core.Configurations;

    public abstract class SettingItemViewModel : ISettingItemViewModel
    {
        protected readonly ISearchAccountInfoRequest Request;

        protected SettingItemViewModel(
            ISearchAccountInfoRequest request,
            int displayOrder = 0)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

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

        protected virtual string OldMainUrl => Request.GetMicroSiteUrl(MicroSiteConstants.OldMain, "~/");

        protected virtual string MainUrl => Request.GetMicroSiteUrl(MicroSiteConstants.Main, "~/");

        protected virtual string AccountInfoUrl => Request.GetMicroSiteUrl(MicroSiteConstants.AccountInfo, "~/");

        protected virtual string MemberInfoUrl => Request.GetMicroSiteUrl(MicroSiteConstants.MemberInfo, "~/");

        protected virtual string MessageUrl => Request.GetMicroSiteUrl(MicroSiteConstants.Message, "~/");

        public virtual bool Visible => Request.ActorIsEdittingDirectDownline;

        public virtual bool AddEditTextToPopupTitle => true;

        public virtual string Method => DataKey.Method.GET;

        public virtual bool Enabled => true;

        public virtual bool ShowGuideLine => false;

        protected virtual string TransferUrl => AppSettings.TransferAppPath;
    }
}
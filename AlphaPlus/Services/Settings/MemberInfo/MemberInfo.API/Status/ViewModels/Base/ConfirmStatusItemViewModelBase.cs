namespace AlphaPlus.Settings.MemberInfo.API.Status.ViewModels
{
    using AlphaPlus.Settings.MemberInfo.API.Requests;
    using Base;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class ConfirmStatusItemViewModelBase : StatusItemViewModel
    {
        protected ConfirmStatusItemViewModelBase(ISearchAccountInfoRequest request, int displayOrder)
            : base(request, displayOrder)
        {
        }

        public override string Url => "UpdateCustomerStatus";

        public virtual string ConfirmMessage => Request.GetResource($"{GetType().Name}{SuffixMessage}");

        public string JsonParams =>
            string.Format("{0}{1}{2}", "{", string.Join(",", Params.Select(x => $"\"{x.Key}\":\"{x.Value.ToString()}\"")), "}");

        public virtual bool ReloadCustomerInfo => false;

        public virtual bool ReloadCustomerList => false;

        public virtual bool IsPopup => false;

        public virtual string PopupHeaderText => string.Empty;

        internal virtual string? SuffixMessage { get; }

        internal virtual string? StatusName { get; }

        internal virtual bool StatusValue { get; }

        internal IDictionary<string, object> Params =>
            new Dictionary<string, object>
                    {
                        {"SearchAccountId", SearchAccount.Id.ToString()},
                        {"SearchAccountName", SearchAccount.Username},
                        {"SearchAccountRoleId", SearchAccount.CusomerLevelValue.ToString()},
                        {"StatusName", StatusName ?? string.Empty},
                        {"StatusValue", StatusValue}
                    };
    }
}
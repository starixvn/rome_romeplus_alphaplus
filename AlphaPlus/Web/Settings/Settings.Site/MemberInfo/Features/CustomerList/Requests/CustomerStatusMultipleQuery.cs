namespace AlphaPlus.Settings.Site.Features.CustomerList.Requests
{
    using System.Collections.Generic;
    using AlphaPlus.Settings.MemberInfo.API.Setting.ViewModels.Base;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using Fanex.Common;
    using MediatR;

    public class CustomerStatusMultipleQuery : CustomerStatusQueryBase, IRequest<IEnumerable<ISettingItemMultipleViewModel>>
    {
        public CustomerStatusMultipleQuery(IUser viewer) : base(viewer)
        {
        }

        public string CustIds { get; set; }

        public string Usernames { get; set; }

        public override string SearchAccountName
        {
            get
            {
                var usernames = Usernames.Split(',');

                return usernames[0];
            }
        }

        public override int SearchAccountId
        {
            get
            {
                var customerIds = CustIds.Split(',');

                return Converter.ToInt(customerIds[0]);
            }
        }

        public override bool IsValid() =>
            base.IsValid() &&
            !string.IsNullOrEmpty(CustIds) &&
            !string.IsNullOrEmpty(Usernames);
    }
}
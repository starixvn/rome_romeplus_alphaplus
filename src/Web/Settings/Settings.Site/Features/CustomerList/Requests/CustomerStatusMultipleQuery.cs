using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting.Base;
using Fanex.Common;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.CustomerList.Requests
{
    public class CustomerStatusMultipleQuery : CustomerStatusQueryBase, IRequest<IEnumerable<ISettingItemMultipleViewModel>>
    {

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
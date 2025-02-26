using AlphaPlus.Settings.Site.Features.Shared;

namespace AlphaPlus.Settings.Site.Features.Customer.Requests
{
    public class EditMultipleCustomersQuery : BaseQuery
    {
        public string CustIds { get; set; }

        public string Usernames { get; set; }

        public byte RoleId { get; set; }

        public int ProductGroupId { get; set; }
    }
}
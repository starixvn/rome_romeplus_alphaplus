using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.SubAccount.Requests
{
    public class UpdateSubAccountInfoRequest : SubAccountBaseRequest, IRequest<Result>
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public bool Closed { get; set; }
    }
}
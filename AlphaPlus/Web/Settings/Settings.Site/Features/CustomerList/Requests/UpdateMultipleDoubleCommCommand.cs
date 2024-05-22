using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.CustomerList.Requests
{
    public class UpdateMultipleDoubleCommCommand : CustomerStatusQueryBase, IRequest<IEnumerable<Result>>
    {

        public override bool IsValid()
        => !string.IsNullOrEmpty(SearchAccountIds) && CanUpdateAccount && !IsInternal && viewer.LevelId == SearchAccountRoleId + 1;

        public string SearchAccountIds { get; set; }

        public bool EnabledDoubleComm { get; set; }
    }
}
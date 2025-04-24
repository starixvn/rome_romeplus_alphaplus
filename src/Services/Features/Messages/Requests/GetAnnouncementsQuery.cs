using MediatR;
using AlphaPlus.Features.Messages.API.ViewModels;

namespace AlphaPlus.Features.Messages.API.Requests
{
    public class GetAnnouncementsQuery : IRequest<IEnumerable<AnnouncementViewModel>>
    {
        public long FromTime { get; set; }

        public long ToTime { get; set; }

        public IEnumerable<CustomerInfo> Customers { get; set; } = Enumerable.Empty<CustomerInfo>();
    }
}
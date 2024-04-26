namespace AlphaPlus.Features.Messages.API.Requests
{
    using System.Collections.Generic;
    using MediatR;
    using ViewModels;

    public class GetAnnouncementsQuery : IRequest<IEnumerable<AnnouncementViewModel>>
    {
        public long FromTime { get; set; }

        public long ToTime { get; set; }

        public IEnumerable<CustomerInfo> Customers { get; set; } = Enumerable.Empty<CustomerInfo>();
    }
}
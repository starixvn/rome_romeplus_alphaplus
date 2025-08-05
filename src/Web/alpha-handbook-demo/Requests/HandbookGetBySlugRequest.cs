using MediatR;
using Alpha.Handbook.Demo.ViewModels;

namespace Alpha.Handbook.Demo.Requests
{
    public class HandbookGetBySlugRequest : BaseHandbookRequest, IRequest<GetBySlugViewModel>
    {
        public string Slug { get; set; }
        public string Id { get; set; }
    }
}

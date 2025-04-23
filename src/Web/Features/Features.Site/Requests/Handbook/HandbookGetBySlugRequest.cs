using MediatR;
using AlphaPlus.Features.Site.Handbook.ViewModels;

namespace AlphaPlus.Features.Site.Handbook.Requests
{
    public class HandbookGetBySlugRequest : BaseHandbookRequest, IRequest<GetBySlugViewModel>
    {
        public string Slug { get; set; }
    }
}

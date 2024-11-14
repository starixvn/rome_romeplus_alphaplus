using MediatR;
using AlphaPlus.Features.Site.Handbook.ViewModels;

namespace AlphaPlus.Features.Site.Handbook.Requests
{
    public class GetCategoryBySlugRequest : BaseHandbookRequest, IRequest<CategoryViewModel>
    {
        public string Slug { get; set; }
    }
}

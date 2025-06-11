using MediatR;
using Alpha.Handbook.Demo.ViewModels;

namespace Alpha.Handbook.Demo.Requests
{
    public class GetCategoryBySlugRequest : BaseHandbookRequest, IRequest<CategoryViewModel>
    {
        public string Slug { get; set; }
    }
}

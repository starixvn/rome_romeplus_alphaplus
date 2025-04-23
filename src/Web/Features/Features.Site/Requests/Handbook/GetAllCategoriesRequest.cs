using MediatR;
using AlphaPlus.Features.Handbook.API.Models;

namespace AlphaPlus.Features.Site.Handbook.Requests
{
    public class GetAllCategoriesRequest : BaseHandbookRequest, IRequest<IEnumerable<CategoryModel>>
    {
    }
}

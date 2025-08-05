using MediatR;
using Alpha.Handbook.Demo.Models;

namespace Alpha.Handbook.Demo.Requests
{
    public class GetAllCategoriesRequest : BaseHandbookRequest, IRequest<IEnumerable<CategoryModel>>
    {
    }
}

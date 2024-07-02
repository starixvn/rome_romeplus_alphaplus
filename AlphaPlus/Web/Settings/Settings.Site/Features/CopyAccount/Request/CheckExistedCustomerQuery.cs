using MediatR;
using AlphaPlus.Settings.Site.Features.Shared;

namespace AlphaPlus.Settings.Site.Features.CopyAccount.Request
{
    public class CheckExistedCustomerQuery : BaseQuery, IRequest<bool>
    {
    }
}
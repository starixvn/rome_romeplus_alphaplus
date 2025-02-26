using MediatR;
using AlphaPlus.Settings.Site.Features.Shared;
using Fanex.Customer.Client.Models;

namespace AlphaPlus.Settings.Site.Features.CopyAccount.Request
{
    public class CreditQuery : BaseQuery, IRequest<CreditInfo>
    {
    }
}
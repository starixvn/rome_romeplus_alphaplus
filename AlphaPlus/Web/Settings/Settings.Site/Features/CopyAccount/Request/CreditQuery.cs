using AlphaPlus.Settings.Site.Features.CopyAccount.Models;
using MediatR;
using AlphaPlus.Settings.Site.Features.Shared;

namespace AlphaPlus.Settings.Site.Features.CopyAccount.Request
{
    public class CreditQuery : BaseQuery, IRequest<CreditInfo>
    {
    }
}
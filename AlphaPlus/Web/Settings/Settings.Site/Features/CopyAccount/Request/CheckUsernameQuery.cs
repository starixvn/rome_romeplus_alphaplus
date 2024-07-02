using MediatR;
using AlphaPlus.Settings.Site.Features.Shared;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.CopyAccount.Request
{
    public class CheckUsernameQuery : BaseQuery, IRequest<Result>
    {
    }
}
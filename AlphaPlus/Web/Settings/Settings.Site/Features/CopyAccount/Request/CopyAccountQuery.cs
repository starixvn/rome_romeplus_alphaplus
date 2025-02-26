using MediatR;
using AlphaPlus.Settings.Site.Features.CopyAccount.ViewModels;
using AlphaPlus.Settings.Site.Features.Shared;

namespace AlphaPlus.Settings.Site.Features.CopyAccount.Request
{
    public class CopyAccountQuery : BaseQuery, IRequest<CopyAccountViewModel>
    {
    }
}
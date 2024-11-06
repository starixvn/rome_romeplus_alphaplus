using AlphaPlus.Core.Models;
using AlphaPlus.Features.Site.Messages.ViewModels;
using MediatR;

namespace AlphaPlus.Features.Site.Messages.Requests.BackupDomains
{
    public class BackupDomainsQuery : IRequest<BackupDomainsViewModel>
    {
        public BackupDomainsQuery() : this(null) { }

        public BackupDomainsQuery(IUserSession userSession)
        {
            userSession = userSession ?? UserSession.Current;
            Site = userSession.Site;
            RoleId = userSession.RoleId;
        }

        public string Site { get; }

        public byte RoleId { get; }
    }
}
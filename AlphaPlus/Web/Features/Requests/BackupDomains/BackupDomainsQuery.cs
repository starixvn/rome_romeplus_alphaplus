namespace AlphaPlus.Features.Site.Messages.Requests.BackupDomains
{
    using AlphaPlus.Core.Models;
    using AlphaPlus.Features.Site.Messages.ViewModels;
    using MediatR;

    public class BackupDomainsQuery : IRequest<BackupDomainsViewModel>
    {
        public BackupDomainsQuery(IUserSession userSession)
        {
            Site = userSession.Site;
            RoleId = userSession.RoleId;
        }

        public string Site { get; }

        public byte RoleId { get; }
    }
}
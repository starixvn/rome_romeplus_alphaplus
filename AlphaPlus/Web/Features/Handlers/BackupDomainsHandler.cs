namespace AlphaPlus.Features.Site.Messages.Handlers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AlphaPlus.Features.Messages.Services;
    using AlphaPlus.Features.Site.Messages.Requests.BackupDomains;
    using AlphaPlus.Features.Site.Messages.ViewModels;
    using MediatR;

    public class BackupDomainsHandler : IRequestHandler<BackupDomainsQuery, BackupDomainsViewModel>
    {
        private readonly BackupDomainsService backupDomainsService;

        public BackupDomainsHandler(BackupDomainsService backupDomainsService)
        {
            this.backupDomainsService = backupDomainsService;
        }

        public async Task<BackupDomainsViewModel> Handle(BackupDomainsQuery message, CancellationToken cancellation)
        {
            var domains = await backupDomainsService.GetBackupDomains(message.Site);
            var agencyDomains = new List<string>();
            var memberDomains = new List<string>();

            foreach (var item in domains)
            {
                if (item.IsAgency)
                {
                    agencyDomains.Add(item.Domain);
                }
                else
                {
                    memberDomains.Add(item.Domain);
                }
            }

            return new BackupDomainsViewModel(message.RoleId, agencyDomains, memberDomains);
        }
    }
}
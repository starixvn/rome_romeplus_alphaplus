namespace AlphaPlus.Features.Messages.API.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AlphaPlus.Features.Messages.API.Criteria.BackupDomains;
    using AlphaPlus.Features.Messages.API.Models.BackupDomains;
    using Fanex.Data.Repository;

    public interface BackupDomainsService
    {
        Task<IEnumerable<BackupDomainModel>> GetBackupDomains(string site);
    }

    public class BackupDomainsServiceImpl : BackupDomainsService
    {
        private readonly IDynamicRepository dynamicRepository;

        public BackupDomainsServiceImpl(IDynamicRepository dynamicRepository)
            => this.dynamicRepository = dynamicRepository;

        public Task<IEnumerable<BackupDomainModel>> GetBackupDomains(string site)
            => dynamicRepository.FetchAsync<BackupDomainModel>(new GetGetBackupDomainsCriteria(site));
    }
}

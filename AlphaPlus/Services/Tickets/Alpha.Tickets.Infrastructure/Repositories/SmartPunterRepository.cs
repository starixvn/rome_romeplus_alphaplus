using Alpha.Tickets.Infrastructure.Criteria.SmartPunter;
using Alpha.Tickets.Infrastructure.Models.SmartPunter;
using Fanex.Data.Repository;

namespace Alpha.Tickets.Infrastructure.Repositories;

public interface ISmartPunterRepository
{
    Task<bool> IsInternalAdmin(int adminId);

    Task<SmartPunter> IsSmartPunter(int? custId, string username);

    Task<bool> HasSmartPunterPermission(int adminId);

    Task WriteLogNoData(int? custId, bool isRestricted, int adminId, string ip);

    Task WriteLog(string usernames, bool isRestricted, int adminId, string ip);

    Task CheckSmartPunterAndWriteLog(string usernames, bool isRestricted, int userId, string ip);
}

public class SmartPunterRepository : ISmartPunterRepository
{
    private readonly IDynamicRepository repository;

    public SmartPunterRepository(IDynamicRepository repository)
        => this.repository = repository;

    public async Task<bool> IsInternalAdmin(int adminId)
    {
        var command = new CheckInternalAdminCommand(adminId);
        await repository.ExecuteAsync(command);

        return command.IsInternalAdmin;
    }

    public async Task<SmartPunter> IsSmartPunter(int? custId, string username)
    {
        var command = new CheckSmartPunterCommand(custId, username);
        await repository.ExecuteAsync(command);

        return new SmartPunter(command.IsSmartPunter, command.HasDownlineSmartPunter);
    }

    public async Task<bool> HasSmartPunterPermission(int adminId)
    {
        const string SmartPunterFunctionName = "[SmartPunter]";
        var permissions = await repository.FetchAsync<NapUserFunctionPermission>(new NAPUserPermissionCriteria(adminId));

        return permissions.Any(p => p.FuncName.Equals(SmartPunterFunctionName, StringComparison.OrdinalIgnoreCase));
    }

    public Task WriteLogNoData(int? custId, bool isRestricted, int adminId, string ip)
        => repository.ExecuteAsync(new WriteSmartPunterLogNoDataCommand(custId, isRestricted, adminId, ip));

    public Task WriteLog(string usernames, bool isRestricted, int adminId, string ip)
        => repository.ExecuteAsync(new WriteSmartPunterLogCommand(usernames, isRestricted, adminId, ip));

    public Task CheckSmartPunterAndWriteLog(string usernames, bool isRestricted, int userId, string ip)
        => repository.ExecuteAsync(new CheckSmartPunterAndWriteLogCommand(usernames, isRestricted, userId, ip));
}

using Fanex.Data.Repository;

namespace AlphaPlus.Identity.API.Features.Auth;

public interface IAdminPermissionService
{
    Task<bool> HasUpdateSubAccountPermission(int adminId);

    Task<bool> HasHostPermission(int adminId, int? functionId = null, string functionName = null);
}

public class AdminPermissionService(IDynamicRepository dynamicRepository) : IAdminPermissionService
{
    private readonly IDynamicRepository dynamicRepository = dynamicRepository;
    

    public async Task<bool> HasUpdateSubAccountPermission(int adminId)
        => await HasHostPermission(adminId, functionName: "[UpdateSubAccount]");

    public async Task<bool> HasHostPermission(int adminId, int? functionId = null, string functionName = null)
    {
        var command = new GetAdminHostPermissionCriteria(adminId, functionId, functionName);
        await dynamicRepository.ExecuteAsync(command);

        return command.ErrCode == 0;
    }
}
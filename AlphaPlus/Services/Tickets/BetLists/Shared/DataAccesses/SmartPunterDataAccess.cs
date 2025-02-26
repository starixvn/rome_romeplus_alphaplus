using AlphaPlus.BetLists.Shared.Criteria;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.SmartPunter.Criteria;
using AlphaPlus.SmartPunter.DataAccesses;
using AlphaPlus.SmartPunter.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.Shared.DataAccesses;

public class SmartPunterDataAccess : ISmartPunterDataAccess
{
    private readonly IDynamicRepository repository;

    public SmartPunterDataAccess(IDynamicRepository repository)
        => this.repository = repository;

    public bool CheckInternalAdmin(InternalAdminChecker commandData)
    {
        var command = new InternalAdminCheckerCommand(commandData.CustId);

        repository.Execute(command);

        return command.IsInternalAdmin;
    }

    public bool CheckPermission(NAPUserPermission criteriaData, string functionName)
    {
        var criteria = new NAPUserPermissionCriteria(criteriaData.CustId);

        var permissions = repository.Fetch<NapUserFunctionPermission>(criteria);

        return permissions.Any(p => p.FuncName.Equals(functionName, StringComparison.OrdinalIgnoreCase));
    }

    public SMACustomer CheckSmartPunter(SmartPunterSMAChecker commandData)
    {
        var command = new SmartPunterSMACheckerCommand(commandData.CustId, commandData.Username);

        repository.Execute(command);

        return new SMACustomer(command.IsSmartPunter, command.HasDownlineSmartPunter);
    }

    public void CheckSmartPunterAndWriteLog(SmartPunterSMACheckerWriteLog commandData)
    {
        var command = new SmartPunterSMACheckerWriteLogCommand(
            commandData.ListUserName,
            commandData.ActionId,
            commandData.IsRestricted,
            commandData.UserId,
            commandData.IP);

        repository.Execute(command);
    }

    public void WriteLog(SmartPunterWriteLog commandData)
    {
        var command = new SmartPunterWriteLogCommand(
            commandData.ListUserName,
            commandData.ActionId,
            commandData.IsRestricted,
            commandData.UserId,
            commandData.IP);

        repository.Execute(command);
    }

    public void WriteLogNoData(SmartPunterWriteLogNoData commandData)
    {
        var command = new SmartPunterWriteLogNoDataCommand(
            commandData.CustId,
            commandData.ActionId,
            commandData.IsRestricted,
            commandData.UserId,
            commandData.IP);

        repository.Execute(command);
    }
}
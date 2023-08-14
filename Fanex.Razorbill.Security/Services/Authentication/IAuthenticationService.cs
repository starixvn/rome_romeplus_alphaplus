namespace Fanex.Razorbill.Security.Services.Authentication
{
    using Fanex.Razorbill.Security.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IAuthenticationService
    {
        ClosedLockedStatus CheckClosedOrLockedStatus();

        bool CheckBelonging(int custId, int uplineId, byte uplineRoleId, long transId, bool isDataWarehouse);

        bool IsAuthorized();

        bool IsMultipleCustomersBelonging(string custIds);
    }
}

using AlphaPlus.SmartPunter.Criteria;
using AlphaPlus.SmartPunter.Models;

namespace AlphaPlus.SmartPunter.DataAccesses
{
    public interface ISmartPunterDataAccess
    {
        bool CheckInternalAdmin(InternalAdminChecker commandData);

        SMACustomer CheckSmartPunter(SmartPunterSMAChecker commandData);

        void WriteLog(SmartPunterWriteLog commandData);

        void CheckSmartPunterAndWriteLog(SmartPunterSMACheckerWriteLog commandData);

        void WriteLogNoData(SmartPunterWriteLogNoData commandData);

        bool CheckPermission(NAPUserPermission criteriaData, string functionName);
    }
}
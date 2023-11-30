namespace Alpha.Auth.DataAccesses.Impl
{
    using Alpha.Auth.Criteria;
    using Fanex.Data.Repository;
    using Models;

    public class AuthorizationDataAccess : IAuthorizationDataAccess
    {
        private readonly IDynamicRepository dynamicRepository;

        public AuthorizationDataAccess(IDynamicRepository dynamicRepository = null)
            => this.dynamicRepository = dynamicRepository ?? new DynamicRepository();
        public AccountStatus GetAccountStatus(int customerId, int subAccountId)
        {
            var criteria = new GetCustomerStatusCommand(customerId, subAccountId);
            dynamicRepository.Execute(criteria);

            return new AccountStatus(criteria.IsActive, criteria.Locked);
        }

        public bool IsActive(int customerId, int subAccountId)
            => GetAccountStatus(customerId, subAccountId).IsActive;

        public string GetSubAccountPermission(int subAccountId)
        {
            var criteria = new GetSubAccPermissionCommand(subAccountId);
            dynamicRepository.Execute(criteria);

            return criteria.PageAccess;
        }

        public bool IsBelongingOld(int custId, int currentCustId, long transId)
        {
            var criteria = new CheckBelongingOldCommand(currentCustId, custId == 0 ? default(int?) : custId, transId);
            dynamicRepository.Execute(criteria);

            return criteria.RoleId > 0;
        }

        public BelongingInfo IsBelonging(int custId, int currentCustId, int subAccountId, long transId, bool isInternal)
        {
            var criteria = new CheckBelongingCommand(
                currentCustId,
                custId == 0 ? default(int?) : custId,
                subAccountId,
                transId);
            dynamicRepository.Execute(criteria);

            return new BelongingInfo
            {
                RoleId = criteria.RoleId,
                IsActive = isInternal || (criteria.RoleId > 0 && criteria.IsActive)
            };
        }

        public BelongingInfo IsCustomerConsolidatedGroupBelonging(int custId, int currentCustId, int subAccountId, long transId, bool isInternal)
        {
            var criteria = new CheckBelongingConsolidatedGroupCommand(
                currentCustId,
                custId == 0 ? default(int?) : custId,
                subAccountId,
                transId);
            dynamicRepository.Execute(criteria);

            return new BelongingInfo
            {
                RoleId = criteria.RoleId,
                IsActive = isInternal || (criteria.RoleId > 0 && criteria.IsActive)
            };
        }

        public BelongingInfo IsMultipleCustomersBelonging(string custIds, int currentCustId, int subAccountId, bool isInternal)
        {
            var criteria = new CheckBelongingMultipleCommand(currentCustId, custIds, subAccountId);
            dynamicRepository.Execute(criteria);

            return new BelongingInfo
            {
                RoleId = criteria.RoleId,
                IsActive = isInternal || (criteria.RoleId > 0 && criteria.IsActive)
            };
        }

        public KickoutInfo IsKickedOut(int onlineUserId)
        {
            var criteria = new RecheckInCommand(onlineUserId);
            dynamicRepository.Execute(criteria);

            return new KickoutInfo(criteria.IsKickedOut, criteria.KickOutReason);
        }
    }
}
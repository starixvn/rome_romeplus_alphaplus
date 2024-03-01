namespace AlphaPlus.Settings.MemberInfo.API.Services
{
    using AlphaPlus.Settings.MemberInfo.API.Criteria;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using Fanex.Data.Repository;

    public interface AccountService
    {
        string GetTransferOption(int custId, int roleId);
    }

    public class AccountServiceImpl : AccountService
    {
        private readonly IDynamicRepository repository;
        private const string DailyTransferCondition = "00000000";

        public AccountServiceImpl(IDynamicRepository dynamicRepository)
        {
            repository = dynamicRepository;
        }

        public string GetTransferOption(int custId, int roleId)
        {
            if(roleId == UserLevel.Super.Value)
            {
                return DailyTransferCondition;
            }

            var criteria = new GetTransferOptionCriteria(custId);

            repository.Execute(criteria);

            return criteria.TransferOption;
        }
    }
}

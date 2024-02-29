namespace AlphaPlus.Settings.MemberInfo.API.Features.Shared
{
    using System.Threading.Tasks;
    using AlphaPlus.Settings.MemberInfo.API.Features.Shared.Criteria;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.Shared.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.Shared.Params;
    using Fanex.Data.Repository;

    public interface CheckBelongingService
    {
        BelongingInfo GetBelongingInfo(CheckBelongingParam param);

        Task<BelongingInfo> GetBelongingInfoAsync(CheckBelongingParam param);
    }

    public class CheckBelongingServiceImpl : CheckBelongingService
    {
        private readonly IDynamicRepository dynamicRepository;

        public CheckBelongingServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public BelongingInfo GetBelongingInfo(CheckBelongingParam param)
        {
            var command = new CheckBelongingInDBCustCommand(
                param.ViewerId, param.CustId, param.SubAccId, param.TransId);
            dynamicRepository.Execute(command);

            return new BelongingInfo(command.IsActive, command.RoleId, command.Username);
        }

        public async Task<BelongingInfo> GetBelongingInfoAsync(CheckBelongingParam param)
        {
            var command = new CheckBelongingInDBCustCommand(
                param.ViewerId, param.CustId, param.SubAccId, param.TransId);
            await dynamicRepository.ExecuteAsync(command);

            return new BelongingInfo(command.IsActive, command.RoleId, command.Username);
        }
    }
}

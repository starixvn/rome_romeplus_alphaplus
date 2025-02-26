using AlphaPlus.Core.Configurations;
using Fanex.Data.Repository;
using Microsoft.Extensions.Options;

namespace AlphaPlus.AccountInfo.Site.Features.Shared
{
    public interface IKickOutCustomerService
    {
        Task KickOutDueToProtectedStatus(int custId, byte roleId, int serverId, string ip);

        Task KickOut(KickOutCustomerParam param);
    }

    public class KickOutCustomerService : IKickOutCustomerService
    {
        private readonly AuthorizationOptions options;
        private readonly Fanex.Logging.ILogger logger;
        private readonly IDynamicRepository dynamicRepository;

        public KickOutCustomerService(IOptions<AuthorizationOptions> options, Fanex.Logging.ILogger logger, IDynamicRepository dynamicRepository)
        {
            this.options = options.Value;
            this.logger = logger;
            this.dynamicRepository = dynamicRepository;
        }

        public async Task KickOut(KickOutCustomerParam param)
        {
            try
            {
                if (!options.DisabledCheckDuplicationLogin)
                {
                    await dynamicRepository.ExecuteAsync(new KickOutCustomerCriteria(
                        param.CustId,
                        param.CustLevelId,
                        param.ServerId,
                        param.IP,
                        param.SubAccId,
                        param.Mode));
                }
            }
            catch (Exception ex)
            {
                await logger.ErrorAsync(ex.Message, ex);
            }
        }

        public Task KickOutDueToProtectedStatus(int custId, byte roleId, int serverId, string ip)
            => KickOut(new KickOutCustomerParam(custId, roleId, serverId, ip, 0, "ChangeProtectionMode"));
    }

    public class KickOutCustomerParam
    {
        public KickOutCustomerParam(
            int custId,
            byte custLevelId,
            int serverId,
            string ip,
            int subAccId,
            string mode)
        {
            CustId = custId;
            CustLevelId = custLevelId;
            ServerId = serverId;
            IP = ip;
            SubAccId = subAccId;
            Mode = mode;
        }

        public int CustId { get; }

        public byte CustLevelId { get; }

        public int ServerId { get; }

        public string IP { get; }

        public int SubAccId { get; }

        public string Mode { get; }
    }

    public class KickOutCustomerCriteria : NonQueryCommand
    {
        public KickOutCustomerCriteria(
            int custId,
            byte roleId,
            int serverId,
            string ip,
            int subAccId,
            string mode)
        {
            CustId = custId;
            RoleId = roleId;
            ServerId = serverId;
            IP = ip;
            SubAccId = subAccId;
            Mode = mode;
        }

        public int CustId { get; }

        public byte RoleId { get; }

        public int ServerId { get; }

        public string IP { get; }

        public int SubAccId { get; }

        public string Mode { get; }

        public override string GetSettingKey()
            => "Age_OnUser_ForceKickedout";

        public override bool IsValid()
            => CustId > 0 && RoleId > 0 && ServerId > 0;
    }
}
using Alpha.Main.Criteria;
using Fanex.Data.Repository;
using Alpha.Main.Params;
using Microsoft.Extensions.Options;
using AlphaPlus.Core.Configurations;

namespace Alpha.Main.Services.Impl
{
    public class KickoutCustomerServiceImpl : KickoutCustomerService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly AuthorizationOptions options;

        public KickoutCustomerServiceImpl(IDynamicRepository dynamicRepository, IOptions<AuthorizationOptions> options)
        {
            this.dynamicRepository = dynamicRepository;
            this.options = options.Value;
        }

        public async Task Kickout(KickoutCustomerParam param)
        {
            if (!options.DisabledCheckDuplicationLogin)
            {
                await dynamicRepository.ExecuteAsync(new KickoutCustomerCriteria(param));
            }
        }
    }
}

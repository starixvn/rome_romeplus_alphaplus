namespace AlphaPlus.Settings.MemberInfo.API.Features.CreateAccount
{
    using AlphaPlus.Settings.MemberInfo.API.Features.CreateAccount.Criteria;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.CreateAccount.Models;
    using Fanex.Data.Repository;
    using Fanex.Caching;
    using AlphaPlus.Settings.MemberInfo.Domain.Configurations;

    public interface IProductSettingInfoService
    {
        Task<IEnumerable<ProductSettingInfoModel>> GetProductsSettingInfo(int cusId);
    }

    public class ProductSettingInfoService : IProductSettingInfoService
    {
        private readonly IDynamicRepository repository;

        public ProductSettingInfoService(IDynamicRepository dynamicRepository, ICacheService cacheService)
        {
            repository = dynamicRepository ?? new DynamicRepository();
        }

        public async Task<IEnumerable<ProductSettingInfoModel>> GetProductsSettingInfo(int cusId)
        {
            return await repository.FetchAsync<ProductSettingInfoModel>
            (
                new ProductSettingInfoCriteria
                {
                    CreationStatus = AppSettings.CreationStatus,
                    CustId = cusId
                }
            );
        }
    }
}
using AlphaPlus.Settings.MemberInfo.API.Features.CreateAccount.Criteria;
using AlphaPlus.Settings.MemberInfo.Domain.Configurations;
using AlphaPlus.Settings.MemberInfo.Domain.Features.CreateAccount.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Features.CreateAccount
{
    public interface IProductSettingInfoService
    {
        Task<IEnumerable<ProductSettingInfoModel>> GetProductsSettingInfo(int cusId);
    }

    public class ProductSettingInfoService : IProductSettingInfoService
    {
        private readonly IDynamicRepository repository;

        public ProductSettingInfoService(IDynamicRepository dynamicRepository)
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
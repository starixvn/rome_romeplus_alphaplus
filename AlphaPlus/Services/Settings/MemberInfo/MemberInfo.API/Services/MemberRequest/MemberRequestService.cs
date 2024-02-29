namespace AlphaPlus.Settings.MemberInfo.API.Services.MemberRequest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Models.MemberRequests;
    using AlphaPlus.Settings.MemberInfo.Domain.Params;
    using Criteria.MemberRequests;
    using Fanex.Data.Repository;

    public interface MemberRequestService
    {
        IEnumerable<Product> GetPlayForFunProduct(int custId);

        IEnumerable<MemberRequestModel> GetMemberRequest(MemberRequestsParam param);

        GetSettingResult GetSetting(ProductSettingParam productSettingParam);

        bool IsSync(int productId);
    }

    public class MemberRequestServiceImpl : MemberRequestService
    {
        private readonly IDynamicRepository repository;
        private readonly IUser user;
        private readonly Dictionary<int, Func<ProductSettingParam, GetSettingResult>> getSettingFuncs;

        public MemberRequestServiceImpl(
            IDynamicRepository dynamicRepository,
            IUser user)
        {
            this.user = user;
            repository = dynamicRepository;

            getSettingFuncs = new Dictionary<int, Func<ProductSettingParam, GetSettingResult>>
            {
            };
        }

        public IEnumerable<Product> GetPlayForFunProduct(int custId)
        {
            var playForFunProducts = repository.Fetch<Product>(new PlayForFunProductCriteria(custId));

            return playForFunProducts
                .Where(product => IsShowProduct(product.Id))
                .Select(product => product)
                .OrderBy(product => product.Id);
        }

        public IEnumerable<MemberRequestModel> GetMemberRequest(MemberRequestsParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var criteria = new MemberRequestCriteria(param.CustId, param.ProductId);
            return repository.Fetch<MemberRequestModel>(criteria).OrderBy(member => member.Username);
        }

        public GetSettingResult GetSetting(ProductSettingParam productSettingParam)
        {
            return getSettingFuncs[productSettingParam.ProductId](productSettingParam);
        }

        public bool IsSync(int productId)
        {
            return user.GetSyncedProduct(productId);
        }

        internal bool IsShowProduct(int productId)
        {
            return !user.GetHideProduct(productId);
        }
    }
}
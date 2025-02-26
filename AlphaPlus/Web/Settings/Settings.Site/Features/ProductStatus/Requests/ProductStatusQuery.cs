using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.ProductStatus.ViewModels;
using AlphaPlus.Settings.Site.Features.Shared;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.ProductStatus.Requests
{
    public class ProductStatusQuery : BaseQuery, IRequest<ProductStatusViewModel>
    {
        public ProductStatusQuery()
        {
            UplineId = viewer.Id;
            UplineLevel = Enumeration.FromValue<UserLevel>(viewer.LevelId);
            IsViewOnly = viewer.IsInternal || !viewer.CanUpdateAccount;
            OfferedProductGroupIds = viewer.OfferedProductGroupIds;
        }

        public bool IsGroup { get; set; }

        public int ProductId { get; set; }

        public byte CustRoleId { get; set; }

        public UserLevel CustRole => Enumeration.FromValue<UserLevel>(CustRoleId);

        public int UplineId { get; }

        public UserLevel UplineLevel { get; }

        public bool IsViewOnly { get; }

        public int[] OfferedProductGroupIds { get; }

        public override bool IsValid()
            => ProductId > 0
            && CustId > 0
            && UserLevel.Member.Value <= CustRoleId
            && CustRoleId <= UserLevel.Master.Value
            && !AppSettings.IsSiteLabel789y;
    }
}
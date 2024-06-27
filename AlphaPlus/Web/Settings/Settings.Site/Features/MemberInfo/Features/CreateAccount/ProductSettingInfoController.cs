namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.CreateAccount
{
    using System.Linq;
    using System.Threading.Tasks;
    using AlphaPlus.Core;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using AlphaPlus.Settings.MemberInfo.API.Features.CreateAccount;
    using AlphaPlus.Settings.MemberInfo.API.Services;
    using Microsoft.AspNetCore.Mvc;
    using AlphaPlus.Settings.MemberInfo.Domain.Configurations;

    public class ProductSettingInfoController : BaseController
    {
        private readonly IUser viewer;
        private readonly IProductSettingInfoService service;
        private readonly CacheRefService refService;

        private const string ProductSettingInformationViewPath =
            "~/Features/MemberInfo/Features/CreateAccount/Views/ProductSettingInfo.cshtml";
        
        private const int RNGGameGroupdId = 2;
        private const int LiveRNGCasinoGroupdId = 3;
        private const int BBINProductID = 47;
        private const int NumberGameProductID = 3;
        private const int RNGKENOProductID = 8;

        public ProductSettingInfoController(IUser viewer, IProductSettingInfoService service,
            CacheRefService refService)
        {
            this.viewer = viewer;
            this.service = service;
            this.refService = refService;
        }

        public ActionResult Index() => View(ProductSettingInformationViewPath);

        [HttpGet]
        public async Task<ActionResult> GetProductSettingInfo()
        {
            var products = await service.GetProductsSettingInfo(viewer.Id);
            
            var groupedData = products
                .GroupBy(d => d.GroupId)
                .Select(group => new
                {
                    Id = group.Key,
                    Name = refService.GetProductGroupName(group.Key),
                    Products = (group.GroupBy(d => d.SubGroupId)
                        .Select(subGroup => new
                        {
                            Id = subGroup.Key,
                            Name = refService.GetSubGroupName(subGroup.Key),
                            Products =(subGroup.Select(d => new
                            {
                                Id=d.RealProductId,
                                Name = refService.GetProductName(d.RealProductId),
                                Products = ""
                            }).ToList())
                        })
                        .ToList())
                })
                .ToList();
            
            var jsonResponse = new
            {
                ProductSettingInfo = groupedData,
                RoleId = viewer.LevelId,
                Is789y = AppSettings.IsSiteLabel789y,
                ShowBBin = products
                    .Where(gr => gr.GroupId == LiveRNGCasinoGroupdId)
                    .Any(e => e.RealProductId == BBINProductID),
                ShowRNGKenoNumberGame = products
                    .Where(gr => gr.GroupId == RNGGameGroupdId)
                    .Any(e => e.RealProductId == NumberGameProductID || e.RealProductId == RNGKENOProductID),
            };

            return Json(jsonResponse);
        }
    }
}
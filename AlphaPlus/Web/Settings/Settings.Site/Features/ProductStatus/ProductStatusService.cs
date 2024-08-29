using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.ProductStatus.Criteria;
using AlphaPlus.Settings.Site.Features.ProductStatus.Models;
using AlphaPlus.Settings.Site.Features.ProductStatus.Params;
using AlphaPlus.Shared.AppResources.Settings;
using AlphaPlus.Settings.Site.Features.Shared.Services;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Site.Features.ProductStatus
{
    public interface IProductStatusService
    {
        Task<IEnumerable<ProductStatusModel>> GetProductStatus(ProductStatusAuthorizationParam param);

        Task<IEnumerable<Result>> UpdateProductStatus(ProductStatusCommandParam commandParam);

        IEnumerable<ProductGroup> GetOfferedProductGroup(int custId, int roleId);
    }

    public class ProductStatusService(
        Fanex.Logging.ILogger logger,
        IDynamicRepository repository,
        ICacheRefService cacheRefService,
        Func<int, bool> canViewProduct) : IProductStatusService
    {
        private const int DoNothingErrorCode = 90017;
        private const string UMDateTimeFormat = "dd/MM/yyyy HH:mm";
        private const int ParentChildViolated = 92;
        private readonly Fanex.Logging.ILogger logger = logger;
        private readonly IDynamicRepository repository = repository;
        private readonly ICacheRefService cacheRefService = cacheRefService;
        private readonly Func<int, bool> canViewProduct = canViewProduct;

        public Task<IEnumerable<ProductStatusModel>> GetProductStatus(ProductStatusAuthorizationParam param)
        {
            if (!HasPermission(param))
            {
                return Task.FromResult(Enumerable.Empty<ProductStatusModel>());
            }

            if (param.IsGroup)
            {
                return GetProductGroupStatus(param.ProductId, param.CustId, param.CustRole.Value);
            }

            return GetProductStatus(param.ProductId, param.CustId, param.CustRole);
        }

        private async Task<IEnumerable<ProductStatusModel>> GetProductGroupStatus(int groupId, int custId, byte roleId)
        {
            ICriteria criteria = new SelectProductGroupStatusCriteria(custId, roleId, groupId);
            var rawData = await repository.FetchAsync<ProductStatusModel>(criteria).ConfigureAwait(false);

            return SetProductGroupStatusName(rawData);
        }

        private async Task<IEnumerable<ProductStatusModel>> GetProductStatus(int productId, int custId, UserLevel custRole)
        {
            ICriteria criteria = CreateSelectCriteria(productId, custId, custRole.Value);
            var rawData = await repository.FetchAsync<ProductStatusModel>(criteria).ConfigureAwait(false);

            return SetProductStatusName(productId, rawData);
        }

        public ICriteria CreateSelectCriteria(int productId, int customerId, byte roleId)
        {
            return productId switch
            {
                ProductIdConstants.SabaVirtualSports => new SelectDCSProductStatusFor4Criteria(productId, customerId),
                ProductIdConstants.CashOut => new SelectCashOutStatusCriteria(productId, customerId, roleId),
                _ => new SelectDCSProductStatusCriteria(productId, customerId, roleId),
            };
        }

        private IEnumerable<ProductStatusModel> SetProductStatusName(
            int productId,
            IEnumerable<ProductStatusModel> rawData)
        {
            var hasMultipleRow = rawData.Count() > 1;

            foreach (var item in rawData)
            {
                item.Name = GetProductStatusName(productId, item.SportType, item.BetType, hasMultipleRow);

                yield return item;
            }
        }

        private string GetProductStatusName(int productId, int sportType, int betType, bool hasMultipleRow)
        {
            var sportShowName = new List<int>
            {
                SportTypeConstants.AllBet,
                SportTypeConstants.BBIN,
                SportTypeConstants.SexyGaming,
                SportTypeConstants.SAGaming,
                SportTypeConstants.GTCasino,
                SportTypeConstants.Lottery,
                SportTypeConstants.TableGaming,
                SportTypeConstants.ION,
                SportTypeConstants.RNGWar,
                SportTypeConstants.WM,
                SportTypeConstants.GDG,
                SportTypeConstants.BigGame,
                SportTypeConstants.SportsLottery,
                SportTypeConstants.PPLiveCasino,
                SportTypeConstants.MacauGames,
                SportTypeConstants.Jackpot,
                SportTypeConstants.RNGSlot,
                SportTypeConstants.VirtualGames,
                SportTypeConstants.FGG,
                SportTypeConstants.NumberGame,
                SportTypeConstants.RNGKeno,
                SportTypeConstants.YeeBet
            };

            if (sportShowName.Contains(sportType))
            {
                return cacheRefService.GetSportTypeName(sportType);
            }

            if (hasMultipleRow)
            {
                return cacheRefService.GetBetTypeName(betType);
            }

            return cacheRefService.GetProductName(productId);
        }

        private IEnumerable<ProductStatusModel> SetProductGroupStatusName(IEnumerable<ProductStatusModel> rawData)
        {
            foreach (var item in rawData)
            {
                item.Name = cacheRefService.GetSportTypeName(item.SportType);

                yield return item;
            }
        }

        public Task<IEnumerable<Result>> UpdateProductStatus(ProductStatusCommandParam commandParam)
        {
            var authorizationParam = new ProductStatusAuthorizationParam(
                commandParam.IsGroup,
                commandParam.ProductId,
                commandParam.CustId,
                commandParam.CustRole,
                commandParam.UplineId,
                commandParam.UplineLevel);

            if (!HasPermission(authorizationParam))
            {
                IEnumerable<Result> results = [new Result((int)HttpStatusCode.Forbidden, Resources.NotAuthorized)];
                return Task.FromResult(results);
            }

            return UpdateStatus(commandParam);
        }

        private async Task<IEnumerable<Result>> UpdateStatus(ProductStatusCommandParam param)
        {
            var criteria = param.IsGroup
                ? new UpdateProductGroupStatusCriteria(param.CustId, param.ProductId, JsonSerializer.Serialize(param.ProductStatusParams), param.SubAccId, param.ActorId, param.IP)
                : CreateUpdateCriteria(param);
            await repository.ExecuteAsync(criteria).ConfigureAwait(false);

            if (!criteria.Success && criteria.ErrCode != DoNothingErrorCode)
            {
                return [Result.CreateErrorResult(ParseDCSXmlErrorMessage(criteria.MsgType, criteria.ErrMsg, param.Language), criteria.ErrCode)];
            }

            IList<Result> results = [];

            if (criteria.Success)
            {
                results.Add(Result.CreateSuccessfulResult(GetResource("StatusUpdatedSuccessfully", param.Language)));
            }

            var umMessages = await GetProductStatusUM(param);

            if (!string.IsNullOrWhiteSpace(umMessages))
            {
                results.Add(Result.CreateErrorResult(umMessages, DoNothingErrorCode));
            }

            return results;
        }

        private async Task<string> GetProductStatusUM(ProductStatusCommandParam param)
        {
            var umMessages = new List<string>();
            var criteria = param.IsGroup ? new CheckProductStatusUMCriteria(param.CustId, groupId: param.ProductId) : new CheckProductStatusUMCriteria(param.CustId, param.ProductId);
            var productStatusUM = await repository.FetchAsync<ProductStatusUM>(criteria);

            foreach (var item in productStatusUM)
            {
                if (item.ProductId == 0 || item.SportType == 0)
                {
                    umMessages.Add(BuildProductStatusUMMessage(item, param.Language));
                    continue;
                }

                if (param.ProductStatusParams.Any(p => p.SportType == item.SportType))
                {
                    umMessages.Add(BuildProductStatusUMMessage(item, param.Language));
                }
            }

            return umMessages.Count == 0
                ? string.Empty
                : $"{GetResource("ProductStatusUM", param.Language)}<br>{string.Join("<br>", umMessages)}";
        }

        private string BuildProductStatusUMMessage(ProductStatusUM message, string language)
           => string.Format(
               GetResource("ProductStatusUMPattern", language),
               GetProductStatusName(message, language),
               message.SettingType == 0 ? GetResource("CustomerSettings", language) : GetResource("_ProductStatus", language),
               ToUMDateTime(message.BeginTime),
               ToUMDateTime(message.EndTime));

        private static string GetResource(string name, string language)
            => AlphaPlus.Shared.AppResources.Settings.ProductStatus.ProductStatus.ResourceManager.GetString(name, new CultureInfo(language));

        private static string ToUMDateTime(DateTime dateTime)
           => dateTime.ToString(UMDateTimeFormat);

        public NonQueryProductStatusCriteria CreateUpdateCriteria(ProductStatusCommandParam param)
        {
            return param.ProductId switch
            {
                ProductIdConstants.SabaVirtualSports => new UpdateProductStatusForSettingType4Criteria(param.CustId, param.SubAccId, param.ActorId, param.IP, BuildProductStatusXml(param.ProductStatusParams)),
                ProductIdConstants.CashOut => new UpdateProductStatusForSettingType4Criteria(param.CustId, param.SubAccId, param.ActorId, param.IP, BuildProductStatusXml(param.ProductStatusParams)),
                ProductIdConstants.RNGSlot => CreateUpdateMacauGamesStatusCriteria(param),
                _ => CreateUpdateDCSProductStatusCriteria(param),
            };
        }

        private string GetProductStatusName(ProductStatusUM message, string language)
        {
            if (message.ProductId == 0)
            {
                return GetResource("AllProducts", language);
            }

            if (message.SportType == 0)
            {
                cacheRefService.GetProductName(message.ProductId, language);
            }

            return GetProductStatusName(message.ProductId, message.SportType, 0, false);
        }

        private static NonQueryProductStatusCriteria CreateUpdateDCSProductStatusCriteria(ProductStatusCommandParam param)
           => new UpdateDCSProductStatusCriteria(
               param.CustId,
               param.SubAccId,
               param.ActorId,
               param.IP,
               BuildProductStatusXml(param.ProductStatusParams));
        private static NonQueryProductStatusCriteria CreateUpdateMacauGamesStatusCriteria(ProductStatusCommandParam param)
            => new UpdateMacauGamesStatusCriteria(
                param.CustId,
                param.SubAccId,
                param.ActorId,
                param.IP,
                BuildProductStatusXml(param.ProductStatusParams));

        private static string BuildProductStatusXml(IEnumerable<BetTypeStatusParam> statusParams)
        {
            if (statusParams != null && statusParams.Any())
            {
                var builder = new StringBuilder();

                foreach (var item in statusParams)
                {
                    builder.Append(BuildProductStatusElement(item));
                }

                return $"<ROOT>{builder}</ROOT>";
            }

            return string.Empty;
        }

        private static string BuildProductStatusElement(BetTypeStatusParam param)
            => $"<ProductStatus SportType=\"{param.SportType}\" BetType=\"{param.BetType}\" IsDisabled=\"{Convert.ToInt32(param.IsDisabled)}\"/>";

        private bool HasPermission(ProductStatusAuthorizationParam param)
        {
            var customerInformation = GetCustomerInformation(param.CustId);

            return IsMasterAgentMemberOfViewerUser(customerInformation, param.UplineId, param.UplineLevel) &&
                IsDirectDownlineOfViewerUser(customerInformation, param.UplineLevel) &&
                canViewProduct(param.ProductId);
        }

        private static bool IsMasterAgentMemberOfViewerUser(CustomerInformation customerInformation, int uplineId, UserLevel uplineLevel)
        {
            bool isValidCustomer = customerInformation != null;

            if (isValidCustomer)
            {
                if (uplineLevel.IsSuperLevel())
                {
                    isValidCustomer = uplineId == customerInformation.SuperId;
                }
                else if (uplineLevel.IsMasterLevel())
                {
                    isValidCustomer = uplineId == customerInformation.MasterId;
                }
                else if (uplineLevel.IsAgentLevel())
                {
                    isValidCustomer = uplineId == customerInformation.AgentId;
                }
                else
                {
                    isValidCustomer = false;
                }
            }

            return isValidCustomer;
        }

        private static bool IsDirectDownlineOfViewerUser(
            CustomerInformation customerInformation,
            UserLevel uplineLevel)
        {
            bool isValidRole = customerInformation != null;

            if (isValidRole)
            {
                var custRole = Enumeration.FromValue<UserLevel>(customerInformation.RoleId);

                isValidRole = custRole.IsDirectDownlineOf(uplineLevel);
            }

            return isValidRole;
        }

        private CustomerInformation GetCustomerInformation(int custId)
           => repository.Get<CustomerInformation>(new SelectCustomerInformationCriteria(custId));

        private string ParseDCSXmlErrorMessage(int msgType, string errMsg, string language)
        {
            if (IsValidXml(errMsg))
            {
                if (msgType == ParentChildViolated)
                {
                    return ParseParentChildViolatedMessage(errMsg, language);
                }

                var xmlRoot = XElement.Parse(errMsg);
                var errCode = xmlRoot.Attributes("MsgID").FirstOrDefault().Value;
                var errMsgFromResource = GetResource($"Msg{errCode}", language);

                var errors = new List<string>
                {
                    errMsgFromResource ?? xmlRoot.Attributes("Msg").First().Value
                };

                return string.Join("<br>", errors);
            }

            return errMsg;
        }

        private bool IsValidXml(string xml)
        {
            if (!string.IsNullOrEmpty(xml) && xml.TrimStart().StartsWith("<", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    XDocument.Parse(xml);
                    return true;
                }
                catch (Exception ex)
                {
                    logger.Error($"Invalid DCS Xml: {xml}", ex);
                }
            }

            return false;
        }

        private string ParseParentChildViolatedMessage(string message, string language)
        {
            var xmlRoot = XElement.Parse(message);
            var errCode = xmlRoot.Attributes("MsgID").FirstOrDefault().Value;
            string parentSportType = string.Empty;
            string parentBetType = string.Empty;
            var errMsgTemplate = GetResource("Msg" + errCode, language);
            List<string> childSportTypesNames = new List<string>();
            StringBuilder errorMessages = new StringBuilder();
            foreach (var xmlElement in xmlRoot.Elements())
            {
                var currentParentSportType = xmlElement.Attribute("ParentSportType").Value;
                var currentParentBetType = xmlElement.Attribute("ParentBetType").Value;
                var currentSportType = xmlElement.Attribute("SportType").Value;
                if (currentParentSportType != parentSportType || currentParentBetType != parentBetType)
                {
                    if (!string.IsNullOrWhiteSpace(parentSportType))
                    {
                        errorMessages.Append(string.Format(errMsgTemplate, string.Join(", ", childSportTypesNames), cacheRefService.GetSportTypeName(Convert.ToInt32(parentSportType))));
                    }

                    parentSportType = currentParentSportType;
                    parentBetType = currentParentBetType;
                    childSportTypesNames = new List<string>();
                }

                childSportTypesNames.Add(cacheRefService.GetSportTypeName(Convert.ToInt32(currentSportType), language));
            }

            errorMessages.Append(string.Format(errMsgTemplate, string.Join(", ", childSportTypesNames), cacheRefService.GetSportTypeName(Convert.ToInt32(parentSportType))));
            return errorMessages.ToString();
        }

        public IEnumerable<ProductGroup> GetOfferedProductGroup(int custId, int roleId)
        {
            var productSettings = repository.Fetch<ProductSetting>(new GetProductSettingCriteria(custId, roleId));

            if (productSettings == null || !productSettings.Any())
            {
                return Enumerable.Empty<ProductGroup>();
            }

            return productSettings.Select(x => x.GroupId).Distinct().Select(x => new ProductGroup(x.Value, cacheRefService.GetProductGroupName(x.Value)));
        }
    }
}
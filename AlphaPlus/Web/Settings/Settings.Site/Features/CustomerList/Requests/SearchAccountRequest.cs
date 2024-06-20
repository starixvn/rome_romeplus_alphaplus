using AlphaPlus.Core.Models;
using AlphaPlus.Products.Configurations;
using AlphaPlus.Products.Models;
using AlphaPlus.Shared.AppResources.Settings.ProductResources;

namespace AlphaPlus.Settings.Site.Features.CustomerList.Requests
{
    /// <summary>
    /// should implementation be located in consumer sites.
    /// </summary>
    public abstract class SearchAccountRequest : ExtendedRequest, ISearchAccountInfoRequest
    {
        protected SearchAccountRequest(
            ICustomer actor,
            ICustomer searchAccount,
            string language,
            bool isDarkMode,
            Func<int, string> getProductGroupName,
            Func<int, string> getProductName)
        {
            Actor = actor;
            SearchAccount = searchAccount;
            Language = language;
            IsDarkMode = isDarkMode;
            GetProductGroupName = getProductGroupName;
            GetProductName = getProductName;
            IsSubAcc = actor.IsSubAccount;
        }

        public ICustomer Actor { get; }

        public ICustomer SearchAccount { get; }

        public string Language { get; }

        public bool IsDarkMode { get; }

        public virtual bool IsValid =>
            Actor != null
            && SearchAccount != null
            && Actor.IsUplineOf(SearchAccount);

        public bool ActorIsEditingDirectDownline => Actor.IsDirectUplineOf(SearchAccount);

        public bool ActorIsNotEditingDirectDownline => !ActorIsEditingDirectDownline;

        public Func<string, string> GetResource => ProductResources.ResourceManager.GetString;

        public Func<int, string> GetProductGroupName { get; }

        public Func<int, string> GetProductName { get; }

        public abstract Func<ProductItem, bool> Predicate { get; }

        public bool IsSubAcc { get; }

        public PageAccess SubPageAccess { get; set; }

        public bool SupportAutoTransfer { get; set; }

        public bool AutoTransferAffect { get; set; }
    }

    public class SettingSearchAccountRequest : SearchAccountRequest
    {
        public SettingSearchAccountRequest(
            ICustomer actor,
            ICustomer searchAccount,
            string language,
            bool isDarkMode,
            Func<int, string> getProductGroupName,
            Func<int, string> getProductName)
            : base(actor, searchAccount, language, isDarkMode, getProductGroupName, getProductName)
        {
        }

        public override Func<ProductItem, bool> Predicate => config => config.ExtendProductProperty.Setting;
    }

    public class StatusSearchAccountRequest : SearchAccountRequest
    {
        public StatusSearchAccountRequest(
            ICustomer actor,
            ICustomer searchAccount,
            string language,
            bool isDarkMode,
            Func<int, string> getProductGroupName,
            Func<int, string> getProductName)
            : base(actor, searchAccount, language, isDarkMode, getProductGroupName, getProductName)
        {
        }

        public override Func<ProductItem, bool> Predicate => config => config.ExtendProductProperty.Status;
    }
}
using AlphaPlus.Core.Models;
using AlphaPlus.Products.Configurations;
using AlphaPlus.Products.Models;

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
            Func<string, string> getResource = null,
            Func<int, string> getProductName = null,
            Func<string, string, string> getMicroSiteUrl = null)
        {
            Actor = actor;
            SearchAccount = searchAccount;
            Language = language;
            IsDarkMode = isDarkMode;
            GetProductGroupName = getProductGroupName;
            GetResource = getResource;
            GetProductName = getProductName;
            GetMicroSiteUrl = getMicroSiteUrl;

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

        public bool ActorIsEdittingDirectDownline => Actor.IsDirectUplineOf(SearchAccount);

        public bool ActorIsNotEdittingDirectDownline => !ActorIsEdittingDirectDownline;

        public Func<string, string> GetResource { get; }

        public Func<int, string> GetProductGroupName { get; }

        public Func<int, string> GetProductName { get; }

        public Func<string, string, string> GetMicroSiteUrl { get; }

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
            Func<string, string> getResource = null,
            Func<int, string> getProductName = null,
            Func<string, string, string> getMicroSiteUrl = null)
            : base(actor, searchAccount, language, isDarkMode, getProductGroupName, getResource, getProductName, getMicroSiteUrl)
        {
        }

        public override Func<ProductItem, bool> Predicate
            => config => (config.ExtendProductProperty as SearchAccountProperties).Setting;
    }

    public class StatusSearchAccountRequest : SearchAccountRequest
    {
        public StatusSearchAccountRequest(
            ICustomer actor,
            ICustomer searchAccount,
            string language,
            bool isDarkMode,
            Func<int, string> getProductGroupName,
            Func<string, string> getResource = null,
            Func<int, string> getProductName = null,
            Func<string, string, string> getMicroSiteUrl = null)
            : base(actor, searchAccount, language, isDarkMode, getProductGroupName, getResource, getProductName, getMicroSiteUrl)
        {
        }

        public override Func<ProductItem, bool> Predicate
            => config => (config.ExtendProductProperty as SearchAccountProperties).Status;
    }
}
using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.Shared.Models
{
    public interface IUser
    {
        int LoggedInUserId { get; }

        int Id { get; }

        string Name { get; }

        string Site { get; }

        string Language { get; }

        string LanguageKey { get; }

        byte LevelId { get; }

        int SubAccId { get; }

        string SubAccName { get; }

        string IP { get; }

        string LangKey { get; }

        bool CanUpdateAccount { get; }

        bool IsEnabledJackpot { get; }

        IEnumerable<int> EnableJackpotProductIds { get; }

        bool GetHideProduct(int productId);

        bool GetSyncedProduct(int productId);

        bool CanViewBetLists { get; }

        bool IsAllowedTotalBalanceTransfer { get; }

        bool CanTransfer { get; }

        DateTime LimitedDateToShowInfo { get; }

        bool IsAdminCanViewVNIP { get; }

        bool IsInternal { get; }

        int Srecommend { get; }

        // IEnumerable<int> IndirectLoginSuperIds { get; }

        // bool IsAllowedIndirectLogin { get; }

        bool GetDisabledProduct(int productId);

        bool AllowedTotalBalanceTransfer { get; }

        bool CanViewReports { get; }

        bool IsOfferProtectedMode { get; }

        string DarkModeName { get; }

        string AssetAppPath { get; }

        string Currency { get; }

        int CurrencyId { get; }

        bool IsSubAcc { get; }

        PageAccess SubPageAccess { get; }

        bool IsEnabledCustomerDateLimit { get; }

        int[] OfferedProductGroupIds { get; }

        int CustDBGroupId { get; }
    }
}
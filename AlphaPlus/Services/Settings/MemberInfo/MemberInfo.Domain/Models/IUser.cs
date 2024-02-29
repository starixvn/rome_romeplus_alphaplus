namespace AlphaPlus.Settings.MemberInfo.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using AlphaPlus.Core.Models;

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

        bool CanUpdateAccount { get; }

        bool IsEnabledJackpot { get; }

        IList<int> EnableJackpotProductIds { get; }

        bool GetHideProduct(int productId);

        bool GetSyncedProduct(int productId);

        bool CanViewBetLists { get; }

        bool IsAllowedTotalBalanceTransfer { get; }

        bool CanTransfer { get; }

        DateTime LimitedDateToShowInfo { get; }

        bool IsAdminCanViewVNIP { get; }

        bool IsInternal { get; }

        int Srecommend { get; }

        IEnumerable<int> IndirectLoginSuperIds { get; }

        bool IsAllowedIndirectLogin { get; }

        bool GetDisabledProduct(int productId);

        bool IsDisabledESports { get; }

        bool AllowedTotalBalanceTransfer { get; }

        bool IsShowMyanmarOdds { get; }

        bool CanViewReports { get; }

        bool IsOfferProtectedMode { get; }

        string DarkModeName { get; }

        string AssetAppPath { get; }

        string SiteDBGroupIp { get; }
        string Currency { get; }
        
        int CurrencyId { get; }
        
        bool IsSubAcc { get; }
        
        PageAccess SubPageAccess { get; }

        string AGIP { get; }
    }
}
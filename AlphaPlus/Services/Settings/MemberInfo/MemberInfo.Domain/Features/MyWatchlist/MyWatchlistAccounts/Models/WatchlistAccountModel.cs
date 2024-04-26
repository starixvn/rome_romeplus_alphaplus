namespace AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Models
{
    using System;
    using AlphaPlus.Settings.MemberInfo.Domain.AppResources;
    using AlphaPlus.Settings.MemberInfo.Domain.Extensions;

    public class WatchlistAccountModel
    {
        public WatchlistAccountModel()
        {
        }

        public WatchlistAccountModel(
            int rowNumber,
            int custId,
            int sporttype, 
            string username, 
            DateTime lastLogged, 
            string status, 
            decimal? winningThreshold, 
            decimal? losingThreshold, 
            byte closedReason, 
            byte locked,
            bool isPA)
        {
            RowNumber = rowNumber;
            CustId = custId;
            Sporttype = sporttype;
            Username = username;
            LastLogged = lastLogged;
            Status = status;
            WinningThreshold = winningThreshold;
            LosingThreshold = losingThreshold;
            ClosedReason = closedReason;
            Locked = locked;
            IsPA = isPA;
        }

        public int RowNumber { get; set; }

        public int CustId { get; set; }

        public int Sporttype { get; set; }

        public string? Username { get; set; }

        public DateTime LastLogged { get; set; }

        public string LastLoggedString => LastLogged.ToLongDateTime12hWithHyphen();

        public string? Status { get; set; }

        public string? TranslatedStatus => Resources.ResourceManager.GetString(Status ?? string.Empty);

        public decimal? WinningThreshold { get; set; } 

        public string WinningThresholdString => WinningThreshold.ToMoneyFormatWithUnlimited();

        public decimal? LosingThreshold { get; set; }

        public string LosingThresholdString => LosingThreshold.ToMoneyFormatWithUnlimited();

        public byte ClosedReason { get; set; }

        public byte Locked { get; set; }

        public bool IsPA { get; set; }

        public string IsPAText => IsPA ? Resources.Yes : Resources.No2;
    }
}

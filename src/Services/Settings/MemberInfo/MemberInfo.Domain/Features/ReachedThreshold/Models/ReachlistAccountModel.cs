using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
using AlphaPlus.Settings.MemberInfo.Domain.Extensions;

namespace AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.ReachedThreshold.Models
{
    public class ReachlistAccountModel
    {
        public ReachlistAccountModel()
        {
        }

        public ReachlistAccountModel(
            int rowNumber,
            int custId,
            int sporttype,
            string username,
            DateTime lastLogged,
            DateTime lastReachedDate,
            string lastLimitTypeReach,
            string status,
            decimal winLossAmount,
            decimal? winningThreshold,
            decimal? losingThreshold,
            int reachedCnt,
            byte closedReason,
            byte locked,
            int winningReachedCnt,
            int losingReachedCnt,
            bool isPA)
        {
            RowNumber = rowNumber;
            CustId = custId;
            Sporttype = sporttype;
            Username = username;
            LastLogged = lastLogged;
            LastReachedDate = lastReachedDate;
            LastLimitTypeReach = lastLimitTypeReach;
            Status = status;
            WinLossAmount = winLossAmount;
            WinningThreshold = winningThreshold;
            LosingThreshold = losingThreshold;
            ReachedCnt = reachedCnt;
            ClosedReason = closedReason;
            Locked = locked;
            WinningReachedCnt = winningReachedCnt;
            LosingReachedCnt = losingReachedCnt;
            IsPA = isPA;
        }

        public int RowNumber { get; set; }

        public int CustId { get; set; }

        public int Sporttype { get; set; }

        public string Username { get; set; } = string.Empty;

        public DateTime LastLogged { get; set; }

        public string LastLoggedString => LastLogged.ToLongDateTime12hWithHyphen();

        public DateTime LastReachedDate { get; set; }

        public string LastReachedDateString => LastReachedDate.ToLongDateTime12hWithHyphen();

        public string LastLimitTypeReach { get; set; } = string.Empty;

        public string TranslatedLastLimitTypeReach => Resources.ResourceManager.GetString(LastLimitTypeReach);

        public string Status { get; set; } = string.Empty;

        public string TranslatedStatus => Resources.ResourceManager.GetString(Status);

        public bool IsOpen => Status.Equals("Open", StringComparison.OrdinalIgnoreCase);

        public bool IsSuspended => Status.Equals(Resources.Suspended, StringComparison.OrdinalIgnoreCase);

        public decimal WinLossAmount { get; set; }

        public string WinLossAmountString => WinLossAmount.ToMoneyFormat();

        public decimal? WinningThreshold { get; set; }

        public string WinningThresholdString => WinningThreshold.ToMoneyFormatWithUnlimited();

        public decimal? LosingThreshold { get; set; }

        public string LosingThresholdString => LosingThreshold.ToMoneyFormatWithUnlimited();

        public int ReachedCnt { get; set; }

        public byte ClosedReason { get; set; }

        public byte Locked { get; set; }

        public int WinningReachedCnt { get; set; }

        public int LosingReachedCnt { get; set; }

        public string ReachedCountString
        {
            get
            {
                var winOrLoseText = string.Empty;

                if (WinningReachedCnt > 0 && LosingReachedCnt > 0)
                {
                    winOrLoseText = $"({WinningReachedCnt}W, {LosingReachedCnt}L)";
                }
                else if (WinningReachedCnt > 0)
                {
                    winOrLoseText = $"({WinningReachedCnt}W)";
                }
                else if (LosingReachedCnt > 0)
                {
                    winOrLoseText = $"({LosingReachedCnt}L)";
                }

                return $"{ReachedCnt} {winOrLoseText}";
            }
        }

        public bool IsPA { get; set; }

        public string IsPAText => IsPA ? Resources.Yes : Resources.No2;
    }
}

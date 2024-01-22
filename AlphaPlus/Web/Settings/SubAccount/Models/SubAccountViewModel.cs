namespace AlphaPlus.Settings.Site.SubAccount.Models
{
    public class SubAccountViewModel
    {
        private const string FullPermission = "[A][B][H][D][E][C][G][L]";
        /*
            [A]: Create
            [B]: Update
            [C]: Transfer
            [D]: Total Bets
            [E]: Reports
            [H]: Member Information
            [G]: View Log
            [L]: Bet Lists
        */
        public int SubAccId { get; set; }

        public string Username { get; set; }

        public string Nickname { get; set; }

        public string Password { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public DateTime CreateDate { get; set; }

        public bool Closed { get; set; }

        public string PageAccess { get; set; }

        private bool HasFullPermission => PageAccess.Equals(FullPermission, StringComparison.InvariantCultureIgnoreCase);

        public bool HasCreateAccountPermission => HasFullPermission || PageAccess.Contains("[A][B][H]");

        public bool HasUpdatePermission => HasFullPermission || PageAccess.Contains("[B][H]");

        public bool HasViewMemberInfoPermission => HasFullPermission || PageAccess.Contains("[H]");

        public bool HasNonMemberInfoPermission => !HasViewMemberInfoPermission;

        public bool HasBetsForecastPermission => HasFullPermission || PageAccess.Contains("[D]");

        public bool HasReportPermission => HasFullPermission || PageAccess.Contains("[E]");

        public bool HasBetlistPermission => HasFullPermission || PageAccess.Contains("[L]");

        public bool HasViewLogPermission => HasFullPermission || PageAccess.Contains("[G]");
        
        public bool HasTransferPermission => HasFullPermission || PageAccess.Contains("[C]");
    }
}
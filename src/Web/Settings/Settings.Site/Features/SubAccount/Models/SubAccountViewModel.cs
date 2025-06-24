using AlphaPlus.Core.Models;
using AlphaPlus.Shared.AppResources.Settings.SubAccount;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Settings.Site.Features.SubAccount.Models
{
    public class SubAccountViewModel
    {
        /*
            [A]: Create
            [B]: Update
            [C]: Transfer
            [D]: Total Bets
            [E]: Reports
            [H]: Member Information
            [G]: View Log
            [L]: Bet Lists,
            [M]: Cash Service,
            [N]: Cash Promotion
        */

        private const string CreatePermission = "[A][B][H]";

        private const string UpdatePermission = "[B][H]";

        private const string ViewPermission = "[H]";

        private const string NonePermission = "";

        public int SubAccId { get; set; }

        public string Username { get; set; }

        public string Nickname { get; set; }

        public string Password { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public DateTime CreateDate { get; set; }

        public bool Closed { get; set; }

        public string PageAccess { get; set; }

        private bool HasFullPermission
        {
            get
            {
                if (string.IsNullOrWhiteSpace(PageAccess))
                {
                    return false;
                }

                foreach (var accessType in PageAccessType.AccessTypes.Keys)
                {
                    if (!PageAccess.Contains(accessType.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public bool HasCreateAccountPermission => HasFullPermission || PageAccess.Contains("[A][B][H]", StringComparison.InvariantCultureIgnoreCase);

        public bool HasUpdatePermission => HasFullPermission || PageAccess.Contains("[B][H]", StringComparison.InvariantCultureIgnoreCase);

        public bool HasViewMemberInfoPermission => HasFullPermission || PageAccess.Contains("[H]", StringComparison.InvariantCultureIgnoreCase);

        public bool HasNonMemberInfoPermission => !HasViewMemberInfoPermission;

        public bool HasBetsForecastPermission => HasFullPermission || PageAccess.Contains("[D]", StringComparison.InvariantCultureIgnoreCase);

        public bool HasReportPermission => HasFullPermission || PageAccess.Contains("[E]", StringComparison.InvariantCultureIgnoreCase);

        public bool HasBetlistPermission => HasFullPermission || PageAccess.Contains("[L]", StringComparison.InvariantCultureIgnoreCase);

        public bool HasViewLogPermission => HasFullPermission || PageAccess.Contains("[G]", StringComparison.InvariantCultureIgnoreCase);

        public bool HasTransferPermission => HasFullPermission || PageAccess.Contains("[C]", StringComparison.InvariantCultureIgnoreCase);

        public bool HasCashServicePermission => PageAccess.Contains("[M]", StringComparison.InvariantCultureIgnoreCase);

        public bool HasCashPromotionPermission => PageAccess.Contains("[N]", StringComparison.InvariantCultureIgnoreCase);

        public string MemberInfoPermission => HasCreateAccountPermission ? CreatePermission :
            HasUpdatePermission ? UpdatePermission :
            HasViewMemberInfoPermission ? ViewPermission : NonePermission;

        public string GetPermission() => HasCreateAccountPermission ? Lang.FullControl :
            HasUpdatePermission ? Lang.Update :
            HasViewMemberInfoPermission ? Lang.view : Lang.None;

        public bool IsCustomerDateLimit => UserSession.Current.GetProperty<DateTime>("CustomerDateLimit") != default;

        public IEnumerable<SelectListItem> FirstOptions =>
        [
            new() { Text = "0", Value = "0" },
            new() { Text = "1", Value = "1" },
            new() { Text = "2", Value = "2" },
            new() { Text = "3", Value = "3" },
            new() { Text = "4", Value = "4" },
            new() { Text = "5", Value = "5" },
            new() { Text = "6", Value = "6" },
            new() { Text = "7", Value = "7" },
            new() { Text = "8", Value = "8" },
            new() { Text = "9", Value = "9" }
        ];

        public IEnumerable<SelectListItem> NextOptions =>
        [
            new() { Text = "1", Value = "1" },
            new() { Text = "2", Value = "2" },
            new() { Text = "3", Value = "3" },
            new() { Text = "4", Value = "4" },
            new() { Text = "5", Value = "5" },
            new() { Text = "6", Value = "6" },
            new() { Text = "7", Value = "7" },
            new() { Text = "8", Value = "8" },
            new() { Text = "9", Value = "9" },
            new() { Text = "0", Value = "0" }
        ];
    }
}
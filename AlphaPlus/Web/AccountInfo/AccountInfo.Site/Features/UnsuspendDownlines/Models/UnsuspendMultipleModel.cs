namespace AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.Models
{
    public class UnsuspendMultipleModel
    {
        public UnsuspendMultipleModel(
            int searchAccountId,
            string username,
            bool suspended,
            IEnumerable<UnsuspendDownlineModel> downlines)
        {
            SearchAccountId = searchAccountId;
            Username = username;
            Suspended = suspended;
            Downlines = downlines;
        }

        public int SearchAccountId { get; private set; }

        public string Username { get; private set; }

        public bool Suspended { get; private set; }

        public IEnumerable<UnsuspendDownlineModel> Downlines { get; private set; }
    }

    public class UnsuspendSearchAccountModel
    {
        public UnsuspendSearchAccountModel()
        {
        }

        public UnsuspendSearchAccountModel(
            int searchAccountId,
            string username,
            bool suspended)
        {
            SearchAccountId = searchAccountId;
            Username = username;
            Suspended = suspended;
        }

        public int SearchAccountId { get; set; }

        public string Username { get; set; }

        public bool Suspended { get; set; }
    }

    public class UnsuspendDownlineModel
    {
        public UnsuspendDownlineModel()
        {
        }

        public UnsuspendDownlineModel(
            int searchAccountId,
            byte downlineRoleId,
            int affectedDownlines,
            int unchangedDownlines)
        {
            SearchAccountId = searchAccountId;
            DownlineRoleId = downlineRoleId;
            AffectedDownlines = affectedDownlines;
            UnchangedDownlines = unchangedDownlines;
        }

        public int SearchAccountId { get; set; }

        public byte DownlineRoleId { get; set; }

        public int AffectedDownlines { get; set; }

        public int UnchangedDownlines { get; set; }
    }
}
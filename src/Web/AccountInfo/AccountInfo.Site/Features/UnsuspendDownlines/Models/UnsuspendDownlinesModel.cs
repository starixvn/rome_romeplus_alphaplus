namespace AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.Models
{
    public class UnsuspendDownlineDto
    {
        public UnsuspendDownlineDto()
        {
        }

        public UnsuspendDownlineDto(string username, byte roleId)
        {
            Username = username;
            RoleId = roleId;
        }

        public string Username { get; set; }

        public byte RoleId { get; set; }
    }

    public class UnsuspendDownlineByLevelModel
    {
        public UnsuspendDownlineByLevelModel(byte roleId, IEnumerable<UnsuspendDownlineDto> unsuspendDownlines)
        {
            DownlineUsernames = unsuspendDownlines.Select(downline => downline.Username);
            RoleId = roleId;
        }

        public IEnumerable<string> DownlineUsernames { get; private set; }

        public byte RoleId { get; private set; }
    }

    public class UnsuspendViewListModel
    {
        public UnsuspendViewListModel(
            IEnumerable<UnsuspendDownlineByLevelModel> levels,
            int totalRows)
        {
            Levels = levels;
            TotalRows = totalRows;
        }

        public IEnumerable<UnsuspendDownlineByLevelModel> Levels { get; private set; }

        public int TotalRows { get; private set; }
    }
}
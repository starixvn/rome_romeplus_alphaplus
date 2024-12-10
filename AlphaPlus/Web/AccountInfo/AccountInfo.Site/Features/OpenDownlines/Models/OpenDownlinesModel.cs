namespace AlphaPlus.AccountInfo.Site.Features.OpenDownlines.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class OpenDownlineDto
    {
        public OpenDownlineDto()
        {
        }

        public OpenDownlineDto(string username, byte roleId)
        {
            Username = username;
            RoleId = roleId;
        }

        public string Username { get; set; }

        public byte RoleId { get; set; }
    }

    public class OpenDownlineByLevelModel
    {
        public OpenDownlineByLevelModel(byte roleId, IEnumerable<OpenDownlineDto> unsuspendDownlines)
        {
            DownlineUsernames = unsuspendDownlines.Select(downline => downline.Username);
            RoleId = roleId;
        }

        public IEnumerable<string> DownlineUsernames { get; }

        public byte RoleId { get; }
    }

    public class OpenViewListModel
    {
        public OpenViewListModel(
            IEnumerable<OpenDownlineByLevelModel> levels,
            int totalRows)
        {
            Levels = levels;
            TotalRows = totalRows;
        }

        public IEnumerable<OpenDownlineByLevelModel> Levels { get; }

        public int TotalRows { get; }
    }
}
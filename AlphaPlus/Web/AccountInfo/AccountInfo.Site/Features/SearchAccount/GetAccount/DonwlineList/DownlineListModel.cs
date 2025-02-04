using AlphaPlus.AccountInfo.Site.Features.Shared;
using AlphaPlus.Core.Models;
using AlphaPlus.Shared.AppResources.Settings;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.DonwlineList
{
    public class DownlineDto
    {
        public DownlineDto() { }

        public DownlineDto(string username)
        {
            Username = username;
        }

        public string Username { get; set; }
    }

    public class DownlineListModel
    {
        public DownlineListModel() { }

        public DownlineListModel(IEnumerable<DownlineDto> downlines, int totalPages)
        {
            Downlines = downlines;
            TotalPages = totalPages;
        }

        public DownlineListModel(IEnumerable<DownlineDto> downlines, DownlineListModel model)
            : this(downlines, model.TotalPages)
        { }

        public IEnumerable<DownlineDto> Downlines { get; set; }

        public int TotalPages { get; set; }
    }

    public class TotalDownlinesSummary
    {
        private readonly Dictionary<UserLevel, string> LevelNames = new() { { UserLevel.Agent, Resources.Agents }, { UserLevel.Member, Resources.Members } };

        public TotalDownlinesSummary() { }

        public TotalDownlinesSummary(int totalActive, int totalClosed, int totalSuspended, byte roleId, int agentId)
        {
            TotalActive = totalActive;
            TotalClosed = totalClosed;
            TotalSuspended = totalSuspended;
            RoleId = roleId;
            AgentId = agentId;
        }

        public int TotalActive { get; set; }

        public int TotalClosed { get; set; }

        public int TotalSuspended { get; set; }

        public byte RoleId { get; set; }

        public int AgentId { get; set; }

        public bool IsDirectMember => AgentId == DirectMemberAgentIds.Super || AgentId == DirectMemberAgentIds.Master;

        public string LevelName => LevelNames[Enumeration.FromValue<UserLevel>(RoleId)];
    }
}
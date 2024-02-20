namespace AlphaPlus.Tickets.Site.Features.BetLists.Builders.PTComm
{
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;

    public class PTComm
    {
        public PTComm(ITicket ticket, Core.Models.UserLevel userLevel)
        {
            Ticket = ticket ?? new Ticket();
            UserLevel = userLevel;
        }

        public virtual bool VisiblePositionTaking => !BetStatusIsRunningOrWaiting();

        public virtual bool VisibleCommision => !BetStatusIsRunningOrWaiting();

        public virtual bool VisibilePositionTakingPercent => true;

        public virtual bool VisibileCommisionPercent => true;

        public virtual string PositionTaking => ConvertByBetType.PtValue(PtByUserLevel[UserLevel]);

        public virtual string PositionTakingPercent => ConvertByBetType.Pt(PtSettingByUserLevel[UserLevel]) + "%";

        public virtual string Commission => ConvertByBetType.Comm(CommByUserLevel[UserLevel]);

        public virtual string CommissionPercent
            => GetCommisionPercentText(ConvertByBetType.Comm_Percentage(CommSettingByUserLevel[UserLevel]));

        protected ITicket Ticket { get; }

        protected Core.Models.UserLevel UserLevel { get; }

        protected Dictionary<Core.Models.UserLevel, object> PtByUserLevel { get; set; }

        protected Dictionary<Core.Models.UserLevel, object> PtSettingByUserLevel { get; set; }

        protected Dictionary<Core.Models.UserLevel, object> CommByUserLevel { get; set; }

        protected Dictionary<Core.Models.UserLevel, object> CommSettingByUserLevel { get; set; }

        public PTComm InitPtCommData()
        {
            InitPtByUserLevel();
            InitPtSettingByUserLevel();
            InitCommByUserLevel();
            InitCommSettingByUserLevel();

            return this;
        }

        protected virtual void InitPtByUserLevel()
        {
            PtByUserLevel = new Dictionary<Core.Models.UserLevel, object>
            {
                [Core.Models.UserLevel.Super] = Ticket.SuperWinlost,
                [Core.Models.UserLevel.Master] = Ticket.MasterWinlost,
                [Core.Models.UserLevel.Agent] = Ticket.AgentWinlost
            };
        }

        protected virtual void InitPtSettingByUserLevel()
        {
            PtSettingByUserLevel = new Dictionary<Core.Models.UserLevel, object>
            {
                [Core.Models.UserLevel.Super] = Ticket.SuperPositionTaking,
                [Core.Models.UserLevel.Master] = Ticket.MasterPositionTaking,
                [Core.Models.UserLevel.Agent] = Ticket.AgentPositionTaking
            };
        }

        protected virtual void InitCommByUserLevel()
        {
            CommByUserLevel = new Dictionary<Core.Models.UserLevel, object>
            {
                [Core.Models.UserLevel.Super] = Ticket.SuperComm,
                [Core.Models.UserLevel.Master] = Ticket.AgentComm,
                [Core.Models.UserLevel.Agent] = Ticket.Comm
            };
        }

        protected virtual void InitCommSettingByUserLevel()
        {
            CommSettingByUserLevel = new Dictionary<Core.Models.UserLevel, object>
            {
                [Core.Models.UserLevel.Super] = Ticket.SuperDiscount,
                [Core.Models.UserLevel.Master] = Ticket.MasterDiscount,
                [Core.Models.UserLevel.Agent] = Ticket.AgentDiscount
            };
        }

        private string GetCommisionPercentText(string commPercent)
            => Ticket.CommStatus == "1" ? $"2x {commPercent}%" : $"{commPercent}%";

        private bool BetStatusIsRunningOrWaiting()
        {
            var status = Ticket.Status.ToLowerInvariant();

            return status.Equals(BetStatus.Running)
            || status.Equals(BetStatus.Waiting)
            || status.Equals(BetStatus.Pending);
        }
    }
}

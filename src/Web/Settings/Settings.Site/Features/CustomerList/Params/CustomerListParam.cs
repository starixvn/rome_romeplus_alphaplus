using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.CustomerList.Params
{
    public class CustomerListParam
    {
        public CustomerListParam(
            int userId,
            UserLevel directUplineCustLevel,
            int directUplineId,
            string username,
            bool? enabled,
            bool? closed,
            bool? suspended,
            bool locked,
            byte doubleCommLosing,
            int pageSize,
            int pageIndex,
            bool limitedDateToShowInfo,
            bool showNickname,
            int allowAutoTransfer,
            byte serviceType,
            bool isDirectMember)
        {
            UserId = userId;
            DirectUplineId = directUplineId;
            DirectUplineCustLevel = directUplineCustLevel;
            Username = username;
            Enabled = enabled;
            Closed = closed;
            Suspended = suspended;
            Locked = locked;
            DoubleCommLosing = doubleCommLosing;
            PageSize = pageSize;
            PageIndex = pageIndex;
            LimitedDateToShowInfo = limitedDateToShowInfo;
            ShowNickname = showNickname;
            AllowAutoTransfer = allowAutoTransfer;
            ServiceType = serviceType;
            IsDirectMember = isDirectMember;
        }

        public int UserId { get; }

        public int DirectUplineId { get; }

        public UserLevel DirectUplineCustLevel { get; }

        public string Username { get; }

        public bool? Enabled { get; }

        public bool? Closed { get; }

        public bool? Suspended { get; }

        public bool Locked { get; }

        public byte DoubleCommLosing { get; }

        public int PageSize { get; }

        public int PageIndex { get; }

        public bool LimitedDateToShowInfo { get; }

        public bool ShowNickname { get; }

        public int AllowAutoTransfer { get; }
        public byte ServiceType { get; }
        public bool IsDirectMember { get; }
    }
}
using AlphaPlus.Settings.Site.Features.Shared;

namespace AlphaPlus.Settings.Site.Features.CustomerList.Requests
{
    public class FetchDownlinesListRequestBase : BaseQuery
    {
        public string Username { get; set; }

        public byte Status { get; set; }

        public byte DoubleComm { get; set; } = 2;

        public byte UnconfirmSetting { get; set; }

        public int PageSize { get; set; } = 100;

        public int PageIndex { get; set; } = 1;

        public bool? Suspended
           => Status.Equals(2) ? true : null;

        public bool? Enabled
             => Status.Equals(4) ? false : null;

        public bool? Closed
            => Status.Equals(1) || Status.Equals(3) ? Status.Equals(3) : null;

        public bool Locked
            => Status.Equals(5);

        public bool LimitedDateToShowInfo => viewer.LimitedDateToShowInfo != default;

        public bool ShowNickname => viewer.LevelId == CustLevelId;

        public int AllowAutoTransfer { get; set; } = 2;

        public bool? EnableAllDayBalanceTransfer { get; set; }
    }
}
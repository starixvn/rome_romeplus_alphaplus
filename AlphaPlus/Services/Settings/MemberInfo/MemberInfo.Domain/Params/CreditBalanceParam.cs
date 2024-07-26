namespace AlphaPlus.Settings.MemberInfo.Domain.Params
{
    using AlphaPlus.Core.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Constants;

    public class CreditBalanceParam
    {
        public CreditBalanceParam(
            int custId,
            UserLevel custLevel,
            string userName,
            byte statusId,
            int pageIndex,
            int pageSize,
            bool isAdminCanViewVNIP,
            string agIP)
        {
            CustId = custId;
            CustLevel = custLevel;
            UserName = userName;
            MapStatusIdToParam(statusId);
            PageIndex = pageIndex;
            PageSize = pageSize;
            IsAdminCanViewVNIP = isAdminCanViewVNIP;
            AGIP = agIP;
        }

        public int CustId { get; }

        public UserLevel CustLevel { get; }

        public string UserName { get; }

        public bool? Enabled { get; private set; }

        public bool? Closed { get; private set; }

        public bool? Suspended { get; private set; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public bool IsAdminCanViewVNIP { get; }

        public string AGIP { get; }

        private void MapStatusIdToParam(byte statusId)
        {
            switch (statusId)
            {
                case StatusIdentity.Open:
                    Closed = false;
                    break;

                case StatusIdentity.Suspended:
                    Suspended = true;
                    break;

                case StatusIdentity.Closed:
                    Closed = true;
                    break;

                case StatusIdentity.Disabled:
                    Enabled = false;
                    break;

                case StatusIdentity.All:
                default:
                    break;
            }
        }
    }
}

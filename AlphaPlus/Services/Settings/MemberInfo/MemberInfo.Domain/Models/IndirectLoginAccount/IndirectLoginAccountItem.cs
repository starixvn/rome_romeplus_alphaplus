namespace AlphaPlus.Settings.MemberInfo.Domain.Models.IndirectLoginAccount
{
    using System;
    using AlphaPlus.Settings.MemberInfo.Domain.AppResources;
    using AlphaPlus.Settings.MemberInfo.Domain.Enumerations;

    public class IndirectLoginAccountItem
    {
        public IndirectLoginAccountItem()
        {
        }

        public IndirectLoginAccountItem(
            int custId,
            int subAccId,
            string username,
            byte level,
            bool enabled,
            bool closed,
            bool suspended,
            DateTime? lastVerifyPwd,
            DateTime? lastChangedPwdDate,
            byte locked)
        {
            CustId = custId;
            SubAccId = subAccId;
            Username = username;
            Level = level;
            Enabled = enabled;
            Closed = closed;
            Suspended = suspended;
            LastVerifyPwd = lastVerifyPwd;
            LastChangedPwdDate = lastChangedPwdDate;
            Locked = locked;
        }

        public int CustId { get; set; }

        public int SubAccId { get; set; }

        public string Username { get; set; }

        public byte Level { get; set; }

        public UserLevel CustLevel => Enumeration.FromValue<UserLevel>(Level);

        public string LevelName
        {
            get
            {
                if (CustLevel.IsSuperLevel())
                {
                    return Resources.Super;
                }

                if (CustLevel.IsMasterLevel())
                {
                    return Resources.Master;
                }

                return CustLevel.IsAgentLevel()
                    ? Resources.Agent
                    : Resources.Member;
            }
        }

        public bool Enabled { get; set; }

        public bool Closed { get; set; }

        public bool Suspended { get; set; }

        public string Status
        {
            get
            {
                if (!Enabled)
                {
                    return Resources.Disabled;
                }

                if (Closed)
                {
                    return Resources.Closed;
                }

                return Suspended
                    ? Resources.Suspended
                    : Resources.Open;
            }
        }

        public DateTime? LastVerifyPwd { get; set; }

        public DateTime? LastChangedPwdDate { get; set; }

        public byte Locked { get; set; }

        public bool IsLock => Locked != 0;

        public string LockedStatus => IsLock ? Resources.Yes : Resources.No2;
    }
}

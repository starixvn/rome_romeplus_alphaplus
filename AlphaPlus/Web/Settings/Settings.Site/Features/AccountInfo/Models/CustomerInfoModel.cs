namespace AlphaPlus.Settings.Site.Features.AccountInfo.Models
{
    public class CustomerInfoModel
    {
        public CustomerInfoModel()
        {
        }

        public CustomerInfoModel(
            int custId,
            byte roleId,
            string userName,
            string userName2,
            string firstName,
            string lastName,
            string phone,
            string mobilePhone,
            string fax,
            bool enabled,
            bool uplineEnabled,
            bool closed,
            bool suspended,
            DateTime creatDate,
            DateTime lastLogged,
            string lastLoginIP,
            DateTime pwdExpiry,
            int memberId,
            string memberName,
            int agentId,
            string agentName,
            int masterId,
            string masterName,
            int superId,
            string superName,
            byte locked)
        {
            CustId = custId;
            RoleId = roleId;

            UserName = userName;
            UserName2 = userName2;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            MobilePhone = mobilePhone;
            Fax = fax;

            Enabled = enabled;
            UplineEnabled = uplineEnabled;
            Closed = closed;
            Suspended = suspended;

            CreatDate = creatDate;
            LastLogged = lastLogged;
            LastLoginIP = lastLoginIP;
            PwdExpiry = pwdExpiry;

            MemberId = memberId;
            MemberName = memberName;

            AgentId = agentId;
            AgentName = agentName;

            MasterId = masterId;
            MasterName = masterName;

            SuperId = superId;
            SuperName = superName;

            Locked = locked;
        }

        public int CustId { get; set; }

        public byte RoleId { get; set; }

        public string UserName { get; set; }

        public string UserName2 { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string MobilePhone { get; set; }

        public string Fax { get; set; }

        public byte ClosedReason { get; set; }

        public bool Enabled { get; set; }

        public bool UplineEnabled { get; set; }

        public bool Closed { get; set; }

        public bool Suspended { get; set; }

        public DateTime CreatDate { get; set; }

        public DateTime LastLogged { get; set; }

        public bool AllowOpenInfoPopup { get; set; }

        public string LastLoginIP { get; set; }

        public DateTime PwdExpiry { get; set; }

        public int MemberId { get; set; }

        public string MemberName { get; set; }

        public int AgentId { get; set; }

        public string AgentName { get; set; }

        public int MasterId { get; set; }

        public string MasterName { get; set; }

        public int SuperId { get; set; }

        public string SuperName { get; set; }

        public byte Locked { get; }
    }
}
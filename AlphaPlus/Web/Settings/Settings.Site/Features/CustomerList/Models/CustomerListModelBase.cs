using AlphaPlus.Shared.AppResources.Settings.CustomerList;

namespace AlphaPlus.Settings.Site.Features.CustomerList.Models
{
    public abstract class CustomerListModelBase
    {
        public CustomerListModelBase()
        { }

        public CustomerListModelBase(
         int custId,
         string username,
         byte roleId,
         string uGroup,
         string firstName,
         string lastName,
         string userName2,
         string phone,
         string mobilePhone,
         string lastLoginIP,
         int currency,
         int matchLimit,
         int virtualMultiplier,
         DateTime createDate,
         bool isInternal,
         bool doubleCommLosing,
         int closedReason,
         int rowNumber,
         bool enabled,
         bool closed,
         bool suspended,
         bool disableOutright,
         decimal discountCS,
         decimal discount1x2,
         byte locked,
         bool autoTransfer)
        {
            CustId = custId;
            Username = username;
            RoleId = roleId;
            UGroup = uGroup;
            FirstName = firstName;
            LastName = lastName;
            UserName2 = userName2;
            Phone = phone;
            MobilePhone = mobilePhone;
            LastLoginIP = lastLoginIP;
            Currency = currency;
            MatchLimit = matchLimit;
            VirtualMultiplier = virtualMultiplier;
            CreateDate = createDate;
            DoubleCommLosing = doubleCommLosing;
            ClosedReason = closedReason;
            RowNumber = rowNumber;
            Enabled = enabled;
            Closed = closed;
            Suspended = suspended;
            DisableOutright = disableOutright;
            DiscountCS = discountCS;
            Discount1x2 = discount1x2;
            IsInternal = isInternal;
            Locked = locked;
            AutoTransfer = autoTransfer;
        }

        public int CustId { get; set; }

        public string Username { get; set; }

        public byte RoleId { get; set; }

        public string UGroup { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName2 { get; set; }

        public string Phone { get; set; }

        public string MobilePhone { get; set; }

        public bool AllowOpenInfoPopup { get; set; }

        public string LastLoginIP { get; set; }

        public int Currency { get; set; }

        public int MatchLimit { get; set; }

        public int VirtualMultiplier { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreateDateFormat { get; set; }

        public bool IsInternal { get; set; }

        public bool DoubleCommLosing { get; set; }

        public int ClosedReason { get; set; }

        public int RowNumber { get; set; }

        public bool Enabled { get; set; }

        public bool Closed { get; set; }

        public bool Suspended { get; set; }

        public bool DisableOutright { get; set; }

        public decimal DiscountCS { get; set; }

        public decimal Discount1x2 { get; set; }

        public string Status
        {
            get
            {
                if (Enabled)
                {
                    if (Closed)
                    {
                        return CustomerListResources.Closed;
                    }
                    else if (Suspended)
                    {
                        return CustomerListResources.Suspended;
                    }
                    else
                    {
                        return CustomerListResources.Open;
                    }
                }
                else
                {
                    return CustomerListResources.Disabled;
                }
            }
        }

        public byte Locked { get; set; }
        public bool AutoTransfer { get; set; }
    }
}

namespace AlphaPlus.Settings.Site.Features.CustomerList.Models
{
    public class MasterListModel : AgentListModel
    {
        public MasterListModel()
        {
        }

        public MasterListModel(
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
         decimal groupA,
         decimal groupB,
         decimal groupC,
         decimal groupD,
         bool disableAutoMPT,
         decimal discountMyanmarOdds,
         byte locked,
         bool autoTransfer)
            : base(custId,
            username,
            roleId,
            uGroup,
            firstName,
            lastName,
            userName2,
            phone,
            mobilePhone,
            lastLoginIP,
            currency,
            matchLimit,
            virtualMultiplier,
            createDate,
            isInternal,
            doubleCommLosing,
            closedReason,
            rowNumber,
            enabled,
            closed,
            suspended,
            disableOutright,
            discountCS,
            discount1x2,
            groupA,
            groupB,
            groupC,
            groupD,
            discountMyanmarOdds,
            locked,
            autoTransfer)
        {
            DisableAutoMPT = disableAutoMPT;
        }

        public bool DisableAutoMPT { get; set; }
    }
}

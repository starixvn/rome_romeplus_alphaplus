using System;

namespace AlphaPlus.Settings.MemberInfo.Domain.Features.CustomerList.Models
{
    public class AgentListModel : CustomerListModelBase
    {
        public AgentListModel()
        {
        }

        public AgentListModel(
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
            locked,
            autoTransfer)
        {
            GroupA = groupA;
            GroupB = groupB;
            GroupC = groupC;
            GroupD = groupD;
            DiscountMyanmarOdds = discountMyanmarOdds;
        }

        public decimal GroupA { get; set; }

        public decimal GroupB { get; set; }

        public decimal GroupC { get; set; }

        public decimal GroupD { get; set; }

        public decimal DiscountMyanmarOdds { get; set; }
    }
}

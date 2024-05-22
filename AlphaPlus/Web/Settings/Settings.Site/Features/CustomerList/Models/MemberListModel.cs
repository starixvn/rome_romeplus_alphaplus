using System;

namespace AlphaPlus.Settings.Site.Features.CustomerList.Models
{
    public class MemberListModel : CustomerListModelBase
    {
        public MemberListModel()
        {
        }

        public MemberListModel(
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
         decimal playerDiscount,
         decimal playerDiscountCS,
         decimal playerDiscount1x2,
         decimal discount,
         decimal playerDiscountMyanmarOdds,
         decimal adiscountMyanmarOdds,
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
            PlayerDiscount = playerDiscount;
            PlayerDiscountCS = playerDiscountCS;
            PlayerDiscount1x2 = playerDiscount1x2;
            Discount = discount;
            PlayerDiscountMyanmarOdds = playerDiscountMyanmarOdds;
            AdiscountMyanmarOdds = adiscountMyanmarOdds;
        }

        public decimal PlayerDiscount { get; set; }

        public decimal PlayerDiscountCS { get; set; }

        public decimal PlayerDiscount1x2 { get; set; }

        public decimal Discount { get; set; }

        public decimal AdiscountMyanmarOdds { get; set; }

        public decimal PlayerDiscountMyanmarOdds { get; set; }
    }
}

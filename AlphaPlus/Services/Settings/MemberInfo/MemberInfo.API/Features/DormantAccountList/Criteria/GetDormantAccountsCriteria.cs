namespace AlphaPlus.Settings.MemberInfo.API.Features.DormantAccountList.Criteria
{
    using AlphaPlus.Core.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Constants;
    using Fanex.Data.Repository;

    public class GetDormantAccountsCriteria : CriteriaBase
    {
        public GetDormantAccountsCriteria(
            int custId,
            string username,
            byte roleId,
            byte statusId,
            int pageIndex,
            int pageSize)
        {
            CustId = custId;
            FilterUsername = username;
            FilterRoleId = roleId;
            FilterStatusId = statusId;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public int CustId { get; }

        public string FilterUsername { get; }

        public byte FilterRoleId { get; }

        public byte FilterStatusId { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalItems { get; set; }

        public override string GetSettingKey()
            => "Acc_DormantAccountList_Sel";

        public override bool IsValid()
            => CustId > 0
            && FilterRoleId < UserLevel.Super.Value
            && (FilterStatusId == StatusIdentity.All ||
            FilterStatusId == StatusIdentity.Closed || 
            FilterStatusId == StatusIdentity.Disabled ||
            FilterStatusId == StatusIdentity.Deleted);
    }
}

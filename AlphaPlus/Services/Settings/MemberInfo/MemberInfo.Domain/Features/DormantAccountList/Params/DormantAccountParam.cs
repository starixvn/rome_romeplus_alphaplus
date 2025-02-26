namespace AlphaPlus.Settings.MemberInfo.Domain.Features.DormantAccountList.Params
{
    public class DormantAccountParam
    {
        public DormantAccountParam(
            int custId,
            string username,
            byte roleId,
            byte statusId,
            int pageIndex,
            int pageSize)
        {
            CustId = custId;
            Username = username;
            RoleId = roleId;
            StatusId = statusId;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public int CustId { get; }

        public string Username { get; }

        public byte RoleId { get; }

        public byte StatusId { get; }

        public int PageIndex { get; }

        public int PageSize { get; }
    }
}

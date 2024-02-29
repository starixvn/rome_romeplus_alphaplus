namespace AlphaPlus.Settings.MemberInfo.Domain.Features.QRCodeMemberSignIn.Models
{
    public class CheckBelongingByUsernameModel
    {
        public CheckBelongingByUsernameModel()
        {
        }

        public CheckBelongingByUsernameModel(
            bool isActive,
            int custId,
            bool isSubAcc,
            int roleId)
        {
            IsActive = isActive;
            CustId = custId;
            IsSubAcc = isSubAcc;
            RoleId = roleId;
        }

        public bool IsActive { get;}

        public int CustId { get; }

        public bool IsSubAcc { get;}

        public int RoleId { get;}
    }
}

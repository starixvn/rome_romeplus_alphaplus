namespace AlphaPlus.Settings.MemberInfo.Domain.Features.Shared.Models
{
    public class BelongingInfo
    {
        public BelongingInfo(bool isActive, byte roleId, string username)
        {
            IsActive = isActive;
            RoleId = roleId;
            Username = username;
        }

        public bool IsActive { get; }

        public byte RoleId { get; }

        public string Username { get; }

        public bool IsValid(byte viewerLevelId)
            => IsActive && viewerLevelId > RoleId;
    }
}

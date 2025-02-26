namespace AlphaPlus.Settings.MemberInfo.Domain.Features.QRCodeMemberSignIn.Models
{
    public class SelectRnGStatusModel
    {
        public SelectRnGStatusModel()
        {
        }

        public SelectRnGStatusModel(bool syncCS)
        {
            SyncCS = syncCS;
        }

        public bool SyncCS { get; set; }
    }
}

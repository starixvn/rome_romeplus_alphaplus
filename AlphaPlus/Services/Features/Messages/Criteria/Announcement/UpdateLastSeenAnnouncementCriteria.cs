namespace AlphaPlus.Features.Messages.API.Criteria.Announcement
{
    using Fanex.Data.Repository;

    public class UpdateLastSeenAnnouncementCriteria : NonQueryCommand
    {
        public UpdateLastSeenAnnouncementCriteria(int custId, string messageType, int subAccId)
        {
            CustId = custId;
            SubAccId = subAccId;
            MessageType = messageType;
        }

        public int SubAccId { get; }

        public int CustId { get; }

        public string MessageType { get; set; } = string.Empty;

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; } = string.Empty;

        public override string GetSettingKey() => "Age_MsgCustReadStatus_Upd";

        public override bool IsValid() => CustId > 0 && !string.IsNullOrEmpty(MessageType);
    }
}

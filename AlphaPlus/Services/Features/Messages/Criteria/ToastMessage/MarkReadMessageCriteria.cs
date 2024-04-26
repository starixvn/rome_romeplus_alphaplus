namespace AlphaPlus.Features.Messages.API.Criteria.ToastMessage
{
    using Fanex.Data.Repository;

    public class MarkReadMessageCriteria : NonQueryCommand
    {
        public MarkReadMessageCriteria(int id, int targetId, int subAccId)
        {
            ToastId = id;
            TargetId = targetId;
            SubAccId = subAccId;
        }

        public int ToastId { get; }

        public int TargetId { get; }

        public int SubAccId { get; }

        public bool IsRead => true;

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; } = string.Empty;

        public override string GetSettingKey() => "Age_ToastMessage_SetRead";

        public override bool IsValid() => ToastId > 0;
    }
}

namespace AlphaPlus.Features.Messages.API.Criteria.NewMessage
{
    public class UpdateMessageAsReadOnUserCritria : OnUserCriteriaBase
    {
        public UpdateMessageAsReadOnUserCritria(int custId, int typeId)
        {
            CustId = custId;
            TypeId = typeId;
            IsViewed = true;
        }

        public int CustId { get; }

        public bool IsViewed { get; }

        public int TypeId { get; }

        public override string GetSettingKey() => "Age_MessageNoticeUpdStatus";

        public override bool IsValid() => base.IsValid() && CustId > 0;
    }
}

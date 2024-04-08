namespace AlphaPlus.Features.Messages.API.Criteria.Message
{
    using Fanex.Data.Repository;

    public class HasLatestPrivateMessageCriteria : CriteriaBase
    {
        public HasLatestPrivateMessageCriteria(int userId, bool isTest)
        {
            UserId = userId;
            IsTest = isTest;
        }

        public int UserId { get; }

        public bool IsTest { get; }

        public override string GetSettingKey() => "Age_Msg_HasPriMessage24";

        public override bool IsValid() => UserId > 0;
    }
}

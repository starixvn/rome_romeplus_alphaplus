namespace AlphaPlus.Features.Messages.API.Criteria.Message
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetLatestPrivateMessageCriteria : CriteriaBase
    {
        public GetLatestPrivateMessageCriteria(int userId, bool isTest)
        {
            UserId = userId;
            IsTest = isTest;
        }

        public int UserId { get; }

        [SpParam(SqlParamName = "latestmsgid")]
        public int LatestMessageId => 0;

        public bool IsTest { get; }

        public override string GetSettingKey() => "Age_Msg_GetTopPriMessage24";

        public override bool IsValid() => UserId > 0;
    }
}

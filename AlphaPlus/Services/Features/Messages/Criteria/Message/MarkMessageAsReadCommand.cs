using Fanex.Data.Repository;

namespace AlphaPlus.Features.Messages.API.Criteria.Message
{
    public class MarkMessageAsReadCommand : NonQueryCommand
    {
        public MarkMessageAsReadCommand(int userId, string messageIds)
        {
            UserId = userId;
            MessageIds = messageIds;
        }

        public int UserId { get; }

        public string MessageIds { get; }

        public override string GetSettingKey() => "Age_CustomerUpdMessagesRead";

        public override bool IsValid() => UserId > 0;
    }
}

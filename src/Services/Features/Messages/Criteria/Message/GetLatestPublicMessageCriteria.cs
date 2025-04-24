using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Features.Messages.API.Criteria.Message
{
    public class GetLatestPublicMessageCriteria : CriteriaBase
    {
        private const int IndexSite3 = 2;

        public GetLatestPublicMessageCriteria(int messageType, string messageSite, bool isTest, string customerSite)
        {
            MessageType = messageType;
            IsTest = isTest;
            Site = customerSite;
            MessageSite = messageSite;
        }

        public string Site { get; }

        private string MessageSite { get; set; }

        public int MessageType { get; }

        public string Site1 => !string.IsNullOrEmpty(MessageSite) ? MessageSite.Split(',')[0] : string.Empty;

        public string Site2
        {
            get
            {
                var site2 = string.Empty;
                if (!string.IsNullOrEmpty(MessageSite))
                {
                    var sites = MessageSite.Split(',');
                    site2 = sites.Length > 1 ? sites[1] : string.Empty;
                }

                return site2;
            }
        }

        public string Site3
        {
            get
            {
                var site3 = string.Empty;
                if (!string.IsNullOrEmpty(MessageSite))
                {
                    var sites = MessageSite.Split(',');
                    site3 = sites.Length > IndexSite3 ? sites[IndexSite3] : string.Empty;
                }

                return site3;
            }
        }

        [SpParam(SqlParamName = "latestmsgid")]
        public int LatestMessageId => 0;

        public bool IsTest { get; }

        public override string GetSettingKey() => "Acc_Msg_GetTopPubMessage";

        public override bool IsValid() => !string.IsNullOrEmpty(Site1) && MessageType >= 0;
    }
}

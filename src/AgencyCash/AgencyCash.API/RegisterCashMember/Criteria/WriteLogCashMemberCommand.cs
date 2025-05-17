using Fanex.Data.Repository;

namespace AgencyCash.API.RegisterCashMember.Criteria
{
    public class WriteLogCashMemberCommand : INonQueryCommand
    {
        public WriteLogCashMemberCommand(string logDataXml)
        {
            LogDataXml = logDataXml;
        }

        public string LogDataXml { get; }

        public string GetSettingKey() => "API_CashMember_WriteLog";

        public bool IsValid() => !string.IsNullOrWhiteSpace(LogDataXml);
    }
}

namespace AlphaPlus.Settings.MemberInfo.API.Features.CustomerList.Criteria
{
    using System.Collections.Generic;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.Shared.Models;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class UpdateCustomerStatusCommand : NonQueryCommand
    {
         private static readonly IDictionary<string, string> CustomerStatusSettingKeys =
             new Dictionary<string, string>
             {
                 [StatusNames.DisableAutoMasterPT]= "Acc_CustomerUpdMaster_DisableAutoMPT",
                 [StatusNames.Closed] = "Acc_CustomerUpdClosed",
                 [StatusNames.DoubleComm] = "Acc_CustomerUpdDoubleCommLosing",
                 [StatusNames.DisableOutright] = "Acc_CustomerUpdDisableOutright",
                 [StatusNames.Suspended] = "Acc_CustomerUpdSuspended",
                 [StatusNames.Unlock] = "Acc_CustomerUpdLocked"
             };

        public UpdateCustomerStatusCommand(
            int searchAccountId,
            int actorId,
            int subAccountId,
            string language,
            string ip,
            string statusName)
        {
            SearchAccountId = searchAccountId;
            ActorId = actorId;
            SubAccountId = subAccountId;
            Language = language;
            IP = ip;
            StatusName = statusName;
          
        }

        [SpParam(SqlParamName = "CustId")]
        public int SearchAccountId { get; }

        [SpParam(SqlParamName = "UserId")]
        public int ActorId { get; }

        [SpParam(SqlParamName = "SubAccId")]
        public int SubAccountId { get; }

        public string Language { get; }

        public string IP { get; }

        public int ErrCode { get; set; }

        public string? ErrMsg { get; set; }

        [SpParam(Ignored = true)]
        public string StatusName { get; }       

        public override string GetSettingKey()
            => CustomerStatusSettingKeys[StatusName];

        public override bool IsValid()
            => SearchAccountId > 0
            && ActorId > 0
            && !string.IsNullOrWhiteSpace(StatusName);
    }
}
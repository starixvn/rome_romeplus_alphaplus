namespace AlphaPlus.Settings.MemberInfo.API.Features.CustomerList.Criteria
{
    public class UpdateDisableAutoMPTStatusCommand : UpdateCustomerStatusCommand
    {
        public UpdateDisableAutoMPTStatusCommand(
            int searchAccountId,
            int actorId,
            int subAccountId,
            string language,
            string ip,
            string statusName,
            bool statusValue) : base(searchAccountId, actorId, subAccountId, language, ip, statusName)
        {
            DisableAutoMPT = statusValue;
        }

        public bool DisableAutoMPT { get; }
    }
}
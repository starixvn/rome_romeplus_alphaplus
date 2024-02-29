namespace AlphaPlus.Settings.MemberInfo.API.Features.CustomerList.Criteria
{
    public class UpdateAllowAutoTransferStatusCommand : UpdateCustomerStatusCommand
    {
        public UpdateAllowAutoTransferStatusCommand(
            int searchAccountId,
            int actorId,
            int subAccountId,
            string language,
            string ip,
            string statusName,
            bool statusValue) : base(searchAccountId, actorId, subAccountId, language, ip, statusName)
        {
            AllowAutoTransfer = statusValue;
        }

        public bool AllowAutoTransfer { get; }
    }
}
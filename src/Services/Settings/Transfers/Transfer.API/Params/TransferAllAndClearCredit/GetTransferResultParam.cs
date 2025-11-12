using AlphaPlus.Settings.Transfer.API.Models.TransferAllAndClearCredit;
using Newtonsoft.Json;

namespace AlphaPlus.Settings.Transfer.API.Params.TransferAllAndClearCredit
{
    public class GetTransferResultParam
    {
        public GetTransferResultParam(
            int custId,
            int userId,
            int subAccId,
            bool isClearCredit,
            string username,
            string ip,
            string language,
            string transferSteps,
            string clearCreditSteps,
            bool isAllDayBalanceTransfer,
            string agIP)
        {
            CustId = custId;
            UserId = userId;
            SubAccId = subAccId;
            IsClearCredit = isClearCredit;
            Username = username;
            IP = ip;
            Language = language;
            IsClearCredit = isClearCredit;
            IsAllDayBalanceTransfer = isAllDayBalanceTransfer;
            TransferSteps = JsonConvert.DeserializeObject<IEnumerable<TransferStep>>(transferSteps.Replace("'", "\""))
                ?? Enumerable.Empty<TransferStep>();
            ClearCreditSteps = JsonConvert.DeserializeObject<IEnumerable<ClearCreditStep>>(clearCreditSteps.Replace("'", "\""))
                ?? Enumerable.Empty<ClearCreditStep>();
            AGIP = agIP;
        }

        public int CustId { get; }

        public int UserId { get; }

        public int SubAccId { get; }

        public bool IsClearCredit { get; }

        public string Username { get; }

        public string IP { get; }

        public string Language { get; }

        public bool IsAllDayBalanceTransfer { get; }

        public IEnumerable<TransferStep> TransferSteps { get; }

        public IEnumerable<ClearCreditStep> ClearCreditSteps { get; }

        public bool SkipTransfer() => !TransferSteps.Any(s => s.TransferableAmount != 0);

        public string AGIP { get; }
    }
}

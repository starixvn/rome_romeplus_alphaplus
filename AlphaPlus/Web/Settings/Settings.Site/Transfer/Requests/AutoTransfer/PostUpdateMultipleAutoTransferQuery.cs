namespace AlphaPlus.Settings.Site.Transfer.Requests.AutoTransfer
{
    using AlphaPlus.Settings.Transfer.API.Models.AutoTransfer;
    using MediatR;
    using Settings.Transfer.API.Models;

    public class PostUpdateMultipleAutoTransferQuery : IRequest<UpdateMultipleAutoTransferResultModel>
    {
        public PostUpdateMultipleAutoTransferQuery(IUser user)
        {
            UplineId = user.Id;
            SubAccId = user.SubAccId;
            IP = user.IP;
            Language = user.LanguageKey;
            AGIP = user.AGIP;
        }

        public int UplineId { get; }
        public int SubAccId { get; }
        public string IP { get; }
        public string Language { get; }
        public string AGIP { get; }

        public string OnIds { get; set; }
        public string OffIds { get; set; }
        public string OnStatus { get; set; }
        public string OffStatus { get; set; }
        public string OnSchedule { get; set; }
        public string OffSchedule { get; set; }
        public string Old_TransferOption { get; set; }
        public string OnNames { get; set; }
        public string OffNames { get; set; }
    }
}
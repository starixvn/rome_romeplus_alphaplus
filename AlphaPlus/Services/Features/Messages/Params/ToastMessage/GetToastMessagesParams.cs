namespace AlphaPlus.Features.Messages.API.Params.ToastMessage
{
    public class GetToastMessagesParams
    {
        public int CustId { get; set; }
        public int SubAccId { get; set; }
        public string Language { get; set; } = string.Empty;
        public int LastToastId { get; set; }
    }
}

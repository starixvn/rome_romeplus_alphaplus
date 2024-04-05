namespace AlphaPlus.Features.Messages.Params.ToastMessage
{
    public class GetToastMessagesParams
    {
        public int CustId { get; set; }
        public int SubAccId { get; set; }
        public string Language { get; set; }
        public int LastToastId { get; set; }
    }
}

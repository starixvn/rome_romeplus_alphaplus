namespace AlphaPlus.Settings.Site.SubAccount.Requests
{
    public abstract class SubAccountBaseRequest
    {
        public int SubAccId { get; set; }

        public virtual bool IsValid() => SubAccId > 0;
    }
}
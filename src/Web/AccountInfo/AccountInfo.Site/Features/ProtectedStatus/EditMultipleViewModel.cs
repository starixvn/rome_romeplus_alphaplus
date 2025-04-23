namespace AlphaPlus.AccountInfo.Site.Features.ProtectedStatus
{
    public class EditMultipleViewModel(IEnumerable<CustomerProtectedStatus> protectedStatus)
    {
        public IEnumerable<CustomerProtectedStatus> ProtectedStatus { get; } = protectedStatus;

    }
}
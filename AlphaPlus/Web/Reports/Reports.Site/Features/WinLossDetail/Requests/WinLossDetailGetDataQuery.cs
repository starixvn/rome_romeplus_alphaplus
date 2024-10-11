using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail;

public class WinLossDetailGetDataQuery : WinLossDetailGetWeeksQuery, IRequest<WinLossDetailGetDataViewModel>
{
    public WinLossDetailGetDataQuery() : base()
    {
        IsProtectedMode = Viewer.IsProtectedMode;
        CanUpdateAccount = Viewer.SubPageAccess.UpdateAccount;
        IsInternal = Viewer.IsInternal;
    }

    public bool IsProtectedMode { get; }

    public bool CanUpdateAccount { get; }

    public bool IsInternal { get; }
}
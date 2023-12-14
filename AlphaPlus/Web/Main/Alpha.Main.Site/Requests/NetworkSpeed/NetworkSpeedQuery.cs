using MediatR;
using Alpha.Main.Site.ViewModels.NetworkSpeed;

namespace Alpha.Main.Site.Requests.NetworkSpeed
{
    public class NetworkSpeedQuery : IRequest<NetworkSpeedViewModel>
    {
        public NetworkSpeedQuery(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }
    }
}
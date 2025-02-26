using MediatR;

namespace AlphaPlus.Main.Site.Requests.NetworkSpeed
{
    public class SaveNetworkSpeedCommand : IRequest<Unit>
    {
        public float SpeedKbps { get; set; }
    }
}
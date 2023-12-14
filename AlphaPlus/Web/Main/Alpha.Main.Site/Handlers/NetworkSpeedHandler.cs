using Alpha.Main.Site.Requests.NetworkSpeed;
using Alpha.Main.Site.ViewModels.NetworkSpeed;
using AlphaPlus.Core.Models;
using Alpha.Main.Configurations;
using MediatR;

namespace Alpha.Main.Site.Handlers
{
    public class NetworkSpeedHandler(ILogger logger, IUserSession userSession) :
        IRequestHandler<NetworkSpeedQuery, NetworkSpeedViewModel>,
        IRequestHandler<SaveNetworkSpeedCommand, Unit>
    {
        private static readonly IEnumerable<int> TrackingConnectionCustIds = AppSettings.TrackingConnectionCustIds;
        private readonly ILogger logger = logger;
        private readonly IUserSession userSession = userSession;

        public Task<NetworkSpeedViewModel> Handle(NetworkSpeedQuery request, CancellationToken cancellationToken)
            => Task.FromResult(new NetworkSpeedViewModel(IsNeedToCalculateUserConnectionSpeed(request.CustId)));

        public Task<Unit> Handle(SaveNetworkSpeedCommand request, CancellationToken cancellationToken)
        {
            if (request.SpeedKbps > 0)
            {
                LogUserInformation(request);
            }

            return Task.FromResult(default(Unit));
        }

        public NetworkSpeedViewModel Handle(NetworkSpeedQuery message)
            => new NetworkSpeedViewModel(IsNeedToCalculateUserConnectionSpeed(message.CustId));

        private static bool IsNeedToCalculateUserConnectionSpeed(int custId)
        {
            if (!TrackingConnectionCustIds.Any())
            {
                return false;
            }

            return TrackingConnectionCustIds.Contains(custId);
        }

        internal virtual void LogUserInformation(SaveNetworkSpeedCommand command)
        {
            // TODO: network speed
            // not unittestable here
            // var request = HttpContext.Current.Request;
            // var logMessage = new LogMessage(string.Join(
            //                         "\r\n",
            //                         "Customer connection information:",
            //                         $"Connection Speed: {command.SpeedKbps} kbps",
            //                         $"Cust Id: {userSession.CustId}",
            //                         $"Username: {userSession.UserName}",
            //                         $"Sub Account Id: {userSession.SubAccId}",
            //                         $"Sub Account Name: {userSession.SubUserName}"))
            //                     .AddSection("request info", () => WebInfoGetters.GetRequestInfo(request))
            //                     .AddSection("browser info", () => WebInfoGetters.GetBrowserInfo(request.Browser))
            //                     .AddSection("server info", () => WebInfoGetters.GetRequestHeader(request.Headers))
            //                     .ToString();

            // logger.Info(logMessage);
        }


    }
}
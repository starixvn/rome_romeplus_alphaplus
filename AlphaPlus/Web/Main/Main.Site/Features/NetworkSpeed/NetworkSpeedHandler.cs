using AlphaPlus.Main.Site.Requests.NetworkSpeed;
using AlphaPlus.Core.Models;
using MediatR;
using Fanex.Logging.Extensions;
using Fanex.Logging.Extensions.AspNetCore;
using AlphaPlus.Core.Helpers;

namespace AlphaPlus.Main.Site.Features.NetworkSpeed
{
    public class NetworkSpeedHandler(Fanex.Logging.ILogger logger, IUserSession userSession) :
        IRequestHandler<NetworkSpeedQuery, NetworkSpeedViewModel>,
        IRequestHandler<SaveNetworkSpeedCommand, Unit>
    {
        private static readonly IEnumerable<int> TrackingConnectionCustIds = AppSettings.TrackingConnectionCustIds;
        private readonly Fanex.Logging.ILogger logger = logger;
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
            => new(IsNeedToCalculateUserConnectionSpeed(message.CustId));

        private static bool IsNeedToCalculateUserConnectionSpeed(int custId)
        {
            if (!TrackingConnectionCustIds.Any())
            {
                return false;
            }

            return TrackingConnectionCustIds.Contains(custId);
        }

        internal virtual Task LogUserInformation(SaveNetworkSpeedCommand command)
        {
            var context = AlphaContext.Current;
            var logMessage = new LogMessage(string.Join(
                                    "\r\n",
                                    "Customer connection information:",
                                    $"Connection Speed: {command.SpeedKbps} kbps",
                                    $"Cust Id: {userSession.CustId}",
                                    $"Username: {userSession.UserName}",
                                    $"Sub Account Id: {userSession.SubAccId}",
                                    $"Sub Account Name: {userSession.SubUserName}"))
                                .AddSection("request info", () => WebInfoGetters.GetRequestInfo(context))
                                .AddSection("browser info", () => WebInfoGetters.GetBrowserInfo(context.Request))
                                .ToString();

            return logger.InfoAsync(logMessage);
        }
    }
}
using AlphaPlus.RequestInfo;
using Fanex.Alpha.Before.Configurations;
using FPS.Browserless.Detection.Client.NetStandard;
using Microsoft.Extensions.Options;

namespace Fanex.Alpha.Before.Services.FPS
{
    public interface IRobotDetector
    {
        Task<int> IsRobot(HttpRequest request);
    }

    public class RobotDetector : IRobotDetector
    {
        private readonly Logging.ILogger logger;
        private readonly RobotOptions robotOptions;
        private readonly IPService ipService;
        private readonly IPHelper ipHelper;

        public RobotDetector(
            Logging.ILogger logger,
            IOptions<RobotOptions> robotOptions,
            IPHelper ipHelper,
            IPService ipService)
        {
            this.logger = logger;
            this.robotOptions = robotOptions.Value;
            this.ipHelper = ipHelper;
            this.ipService = ipService;
        }

        public async Task<int> IsRobot(HttpRequest request)
        {
            if (!robotOptions.EnableDetectBot)
            {
                return 0;
            }

            try
            {
                if (robotOptions.IsRobotAlways)
                {
                    return 1;
                }

                var ipInfo = await ipService.GetIPInformation(ipHelper.GetClientIP());
                if (ipInfo != null && ipInfo.CountryShort == "MY")
                {
                    return 0;
                }

                return BotDetection.IsBot(request, robotOptions.FPSBotDetectionApiUrl);
            }
            catch (Exception ex)
            {
                logger.Warn(ex.Message);
                return 0;
            }
        }
    }
}

using Fanex.Alpha.Before.Configurations;
using Fanex.Alpha.Before.Models;
using ILogger = Fanex.Logging.ILogger;

namespace Fanex.Alpha.Before.Helpers
{
    public interface IFPSHelper
    {
        void FPSTracking(HttpRequest request, SignInResult loginResult);

        string GetDeviceID(HttpRequest request);
    }

    public class FPSHelper : IFPSHelper
    {
        private readonly ILogger logger;
        private readonly FPS.Client.NetCore.Service fpsClient;

        public FPSHelper(ILogger logger)
        {
            this.logger = logger;
            this.fpsClient = new FPS.Client.NetCore.Service();
        }

        public void FPSTracking(HttpRequest request, SignInResult loginResult)
        {
            try
            {
                if (!AppSetting.IsEnableFPS)
                {
                    return;
                }                               

                fpsClient
                    .InitialRequest(AppSetting.FpsApi, request)
                    .CheckTransaction(
                    loginResult.Username,
                    AppSetting.FpsSubscriber,
                    "login",
                    loginResult.LoginTime);

            }
            catch (Exception ex)
            {
                logger.Warn(ex.ToString());
            }
        }

        public string GetDeviceID(HttpRequest request)
        {
            try
            {
                if (!AppSetting.IsEnableFPS)
                {
                    return "N/A";
                }

                var deviceId = fpsClient
                    .InitialRequest(AppSetting.FpsApi, request)
                    .GetDeviceID();

                return deviceId;

            }
            catch (Exception ex)
            {
                logger.Warn(ex.ToString());
                return "N/A";
            }
        }
    }
}

namespace AlphaPlus.Features.Messages.Services
{
    using System.Threading;
    using Fanex.RemotingRef;

    public static class AlphaRemotingRefService
    {
        private static RemotingRefService service = new RemotingRefService();

        public static string GetBetTypeName(string betTypeId)
            => service.GetBetTypeName(betTypeId, Thread.CurrentThread.CurrentCulture.Name);

        public static string GetSportTypeName(int sportTypeId)
            => service.GetSportName(sportTypeId.ToString(), Thread.CurrentThread.CurrentCulture.Name);
    }
}

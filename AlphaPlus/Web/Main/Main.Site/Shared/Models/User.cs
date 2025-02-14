using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Main.Site.Shared.Constants;

namespace AlphaPlus.Main.Site.Shared.Models
{
    public interface IUser
    {
        bool IsEnabledJackpot { get; }

        bool ShowProgressiveJackpotGames { get; }

        bool IsAdminCanUpdateSubAccount { get; }

        void RemoveNewCaptchaByCurrency();

        bool IsEnabledTelegramOTP { get; set; }

        int CustId { get; set; }

        bool IsSyncedNovaCasino { get; set; }
    }

    public class User : IUser
    {
        private const string Jackpot = "Jackpot";
        private const string GameEnableSessionKey = "GameEnable_";
        private const string IsAdminCanUpdateSubAccountSessionKey = "IsAdminCanUpdateSubAccount";
        private const string IsEnabledTelegramOTPSessionKey = "IsEnabledTelegramOTP";
        private const string CustIdSessionKey = "CustId";
        public const string IsSyncedNovaCasinoSessionKey = "IsSyncedNovaCasino";

        public bool ShowProgressiveJackpotGames => GetEnableProductsByGameName(Jackpot) != null && GetEnableProductsByGameName(Jackpot).Any(i => i != ProductId.Pragmatic);

        public bool IsEnabledJackpot => GetEnableProductsByGameName(Jackpot) != null && GetEnableProductsByGameName(Jackpot).Any();

        private static IList<int> GetEnableProductsByGameName(string gameName)
            => AlphaContext.Current.Session.Get<IList<int>>(GameEnableSessionKey + gameName, []);

        public bool IsAdminCanUpdateSubAccount
            => AlphaContext.Current.Session.Get<bool>(IsAdminCanUpdateSubAccountSessionKey, false);

        public bool IsEnabledTelegramOTP
        {
            get => AlphaContext.Current.Session.Get<bool>(IsEnabledTelegramOTPSessionKey);
            set => AlphaContext.Current.Session.Set(IsEnabledTelegramOTPSessionKey, value);
        }

        public int CustId
        {
            get => AlphaContext.Current.Session.Get<int>(CustIdSessionKey);
            set => AlphaContext.Current.Session.Set(CustIdSessionKey, value);
        }

        public bool IsSyncedNovaCasino
        {
            get => AlphaContext.Current.Session.Get<bool>(IsSyncedNovaCasinoSessionKey);
            set => AlphaContext.Current.Session.Set(IsSyncedNovaCasinoSessionKey, value);
        }

        public void RemoveNewCaptchaByCurrency()
            => AlphaContext.Current.Session.Set("CreatedSettingLastDate", DateTime.MinValue);
    }
}
using AlphaPlus.Settings.Site.Features.MemberInfo.Features.QRCodeMemberSignIn.Requests;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Shared.AppResources.Settings.MemberInfo;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.QRCodeMemberSignIn.ViewModels
{
    public class QRCodeMemberSignInViewModel
    {
        public QRCodeMemberSignInViewModel(
            QRCodeMemberSignInRequest request,
            string memberNameSuffix,
            bool onlyShowErrormessage)
        {
            MemberNameCopyFrom = request.MemberName;
            ActorName = request.CustName;
            ErrorMessage = request.ErrorMessage;
            Credit = request.Credit;
            MemberNameSuffix = memberNameSuffix;
            OnlyShowErrorMessage = onlyShowErrormessage;
            CustLevel = request.CustLevel.DisplayName;
            PageTitle = request.CustLevel.IsSuperLevel()
                ? Resources.AddNewMaster
                : (request.CustLevel.IsMasterLevel() ? Resources.AddNewAgent : Resources.AddNewMember);
            Language = request.Language;
            IsDarkMode = request.IsDarkMode;
        }

        public string MemberNameCopyFrom { get; }

        public string ActorName { get; }

        public decimal Credit { get; }

        public string[] NameSuffix => [
            "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J",
            "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T",
            "U", "V", "W", "X", "Y", "Z" ];

        public string MemberNameSuffix { get; }

        public string ThirdChar => MemberNameSuffix.Substring(2, 1).ToUpper();

        public string SecondChar => MemberNameSuffix.Substring(1, 1).ToUpper();

        public string FirstChar => MemberNameSuffix.Substring(0, 1).ToUpper();

        public string ErrorMessage { get; }

        public string ShowQRCodeLink
            => $"{MicroSites.SettingUrl()}QRCodeMemberSignIn/ShowQRCode";

        public bool OnlyShowErrorMessage { get; }

        public string CustLevel { get; }

        public string PageTitle { get; }

        public string Language { get; }

        public bool IsDarkMode { get; }

    }
}
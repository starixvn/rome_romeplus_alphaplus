namespace AlphaPlus.Settings.MemberInfo.Domain.Features.CustomerList.Models
{
    public class NonDCSProductStatusModel
    {
        public NonDCSProductStatusModel() { }

        public NonDCSProductStatusModel(
            bool disableEsports,
            bool uplineDisableEsports
            )
        {
            DisableEsports = disableEsports;
            UplineDisableEsports = uplineDisableEsports;
        }

        public bool DisableEsports { get; set; }

        public bool UplineDisableEsports { get; set; }
    }
}

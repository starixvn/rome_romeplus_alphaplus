namespace AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Models
{
    public class PersonalPresetModel
    {
        public PersonalPresetModel()
        {
        }

        public PersonalPresetModel(int id, int sporttype, string presetTitle, decimal winningThreshold, decimal losingThreshold)
        {
            ID = id;
            Sporttype = sporttype;
            PresetTitle = presetTitle;
            WinningThreshold = winningThreshold;
            LosingThreshold = losingThreshold;
        }

        public int ID { get; set; }

        public int Sporttype { get; set; }

        public string PresetTitle { get; set; }

        public decimal WinningThreshold { get; set; }

        public decimal LosingThreshold { get; set; }
    }
}

namespace AlphaPlus.Settings.MemberInfo.Domain.Models.Winloss
{
    public class DailyEnteredSport
    {
        public DailyEnteredSport()
        {
        }

        public DailyEnteredSport(int entered, int nonEntered)
        {
            Entered = entered;
            NonEntered = nonEntered;
        }

        public DailyEnteredSport(
            bool isToday,
            DateTime eventDate,
            int sportType,
            int entered,
            int nonEntered)
        {
            IsToday = isToday;
            EventDate = eventDate;
            SportType = sportType;
            Entered = entered;
            NonEntered = nonEntered;
        }

        public bool IsToday { get; set; }

        public DateTime EventDate { get; set; }

        public int SportType { get; set; }

        public int Entered { get; set; }

        public int NonEntered { get; set; }
    }
}
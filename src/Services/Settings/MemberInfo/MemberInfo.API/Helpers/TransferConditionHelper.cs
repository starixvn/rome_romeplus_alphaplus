namespace AlphaPlus.Settings.MemberInfo.API.Helpers
{
    public static class TransferConditionHelper
    {
        private const int indexOfWeeklyCondition = 7;

        public static List<int> GetTransferrableDatesFromSchedule(string schedule)
        {
            var sc = schedule ?? String.Empty;

            var ds = sc.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => int.Parse(s))
                .ToList();

            return ds;
        }

        public static List<int> GetTransferrableDatesFromCondition(string condition)
        {
            var transferrableDates = new List<int>();
            if (string.IsNullOrWhiteSpace(condition))
            {
                return transferrableDates;
            }

            var allDates = new List<int> { 2, 3, 4, 5, 6, 7, 1 };
            var arrchar = condition.Trim().ToCharArray();
            var isDaily = arrchar[indexOfWeeklyCondition] == '0';
            if (isDaily)
            {
                return allDates;
            }

            for (int i = 1; i < arrchar.Length; i++)
            {
                var dateIndex = i % indexOfWeeklyCondition;
                if (arrchar[dateIndex] == '1')
                {
                    transferrableDates.Add(dateIndex + 1);
                }
            }

            return transferrableDates;
        }
    }
}

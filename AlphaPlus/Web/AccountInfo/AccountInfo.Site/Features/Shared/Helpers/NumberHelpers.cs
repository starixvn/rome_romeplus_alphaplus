namespace AlphaPlus.Settings.Site.Features.Shared.Helpers
{
    public static class NumberHelpers
    {
        private static int DevidedNumber = 100;
        public static decimal RoundUp(decimal value) => Math.Ceiling(value * DevidedNumber) / DevidedNumber;
        public static decimal RoundDown(decimal value) => Math.Floor(value * DevidedNumber) / DevidedNumber;
    }
}
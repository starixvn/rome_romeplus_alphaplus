namespace AlphaPlus.CashService.API.Constants
{
    public static class PaymentStatusConstants
    {
        public const int Pending = 1;
        public const int Completed = 2;
        public const int Failed = 3;
        public const int Cancelled = 4;
        public const int Refunded = 5;
        public const int PartiallyRefunded = 6;
        public const int Processing = 7;
        public const int OnHold = 8;
    }
} 
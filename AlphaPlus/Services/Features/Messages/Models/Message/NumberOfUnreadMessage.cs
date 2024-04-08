namespace AlphaPlus.Features.Messages.API.Models.Message
{
    using System;

    [Serializable]
    public class NumberOfUnreadMessage
    {
        public int Normal { get; set; }

        public int Special { get; set; }

        public int System { get; set; }

        public int Personal { get; set; }

        public int Total => Normal + Special + System + Personal;
    }
}

namespace AlphaPlus.Features.Messages.Models.Announcement
{
    using System;
    using System.Collections.Generic;
    using Messages.Constants;

    public class LastSeenAnnouncement
    {
        private readonly Dictionary<int, DateTime> lastSeen = new Dictionary<int, DateTime>();
        public LastSeenAnnouncement() : 
            this(0, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue)
        {
        }

        public LastSeenAnnouncement(
            int custId,
            DateTime lastSeenNormal,
            DateTime lastSeenPersonal,
            DateTime lastSeenSpecial,
            DateTime lastSeenSystem)
        {
            CustId = custId;
            LastSeenNormal = lastSeenNormal;
            LastSeenPersonal = lastSeenPersonal;
            LastSeenSystem = lastSeenSystem;
            LastSeenSpecial = lastSeenSpecial;
        }

        public int CustId { get; set; }

        public DateTime LastSeenNormal
        {
            get
            {
                return lastSeen[MessageType.Normal];
            }
            set
            {
                lastSeen[MessageType.Normal] = value;
            }
        }

        public DateTime LastSeenSpecial
        {
            get
            {
                return lastSeen[MessageType.Special];
            }
            set
            {
                lastSeen[MessageType.Special] = value;
            }
        }

        public DateTime LastSeenPersonal
        {
            get
            {
                return lastSeen[MessageType.Personal];
            }
            set
            {
                lastSeen[MessageType.Personal] = value;
            }
        }

        public DateTime LastSeenSystem
        {
            get
            {
                return lastSeen[MessageType.System];
            }
            set
            {
                lastSeen[MessageType.System] = value;
            }
        }

        public DateTime GetLastSeenByType(int messageType)
            => lastSeen[messageType];
    }
}

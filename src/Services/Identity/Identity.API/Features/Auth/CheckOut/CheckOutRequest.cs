namespace AlphaPlus.Identity.API.Features.Auth
{
    public class CheckOutRequest
    {
        public int OnlineUserId { get; set; }

        public string Username { get; set; }

        public byte RoleId { get; set; }

        public int ServerId { get; set; }

        public string Nickname { get; set; }

        public string Reason { get; set; }
    }
}
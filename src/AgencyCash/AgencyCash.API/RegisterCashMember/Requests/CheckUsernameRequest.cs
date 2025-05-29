using System.ComponentModel.DataAnnotations;

namespace AgencyCash.API.RegisterCashMember.Requests
{
    public class CheckUsernameRequest
    {
        [Required]
        public string Username { get; set; }
    }
}

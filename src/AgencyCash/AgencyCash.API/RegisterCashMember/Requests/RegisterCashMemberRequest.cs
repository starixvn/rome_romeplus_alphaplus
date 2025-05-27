using System.ComponentModel.DataAnnotations;

namespace AgencyCash.API.RegisterCashMember.Requests;

public class RegisterCashMemberRequest
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "AgentId must be greater than 0.")]
    public int AgentId { get; set; }

    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    [RegularExpression("^[0-9]+$", ErrorMessage = "Mobile must be numeric only.")]
    public string Mobile { get; set; }
}

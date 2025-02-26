using MediatR;

namespace AlphaPlus.Main.Site.Features.Password
{
    public class ResetSubAccountPasswordQuery : IRequest<PasswordViewModel>
    {
        public int SubAccountId { get; set; }

        public string Username { get; set; }
    }
}
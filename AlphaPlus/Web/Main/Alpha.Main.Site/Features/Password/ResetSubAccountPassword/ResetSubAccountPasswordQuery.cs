namespace Alpha.Main.Site.Features.Password
{
    using MediatR;

    public class ResetSubAccountPasswordQuery : IRequest<PasswordViewModel>
    {
        public int SubAccountId { get; set; }

        public string Username { get; set; }
    }
}
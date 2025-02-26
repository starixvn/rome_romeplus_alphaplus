using MediatR;
using AlphaPlus.Shared.AppResources.Main;

namespace AlphaPlus.Main.Site.Features.WhiteLabel
{
    public class WhiteLabelHandler : IRequestHandler<WhiteLabelSSORequest, WhiteLabelSSOResponseViewModel>
    {
        private readonly IWhiteLabelService whiteLabelService;

        public WhiteLabelHandler(IWhiteLabelService whiteLabelService)
        {
            this.whiteLabelService = whiteLabelService;
        }

        public async Task<WhiteLabelSSOResponseViewModel> Handle(WhiteLabelSSORequest request, CancellationToken cancellationToken)
        {
            var isOffered = await whiteLabelService.IsOfferedWhiteLabel(int.Parse(request.ReferenceId));
            if (isOffered)
            {
                var ssoResponse = await whiteLabelService.SendSSORequest(request);
                return new WhiteLabelSSOResponseViewModel { ReturnUrl = ssoResponse.ReturnUrl };
            }

            return new WhiteLabelSSOResponseViewModel
            {
                Code = 1,
                Message = Resources.NotSetupWhiteLabelMessage,
            };
        }
    }
}
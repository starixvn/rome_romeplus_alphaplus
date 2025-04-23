using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Main.Site.Features.ApiKey
{
    public class ApiKeyHandler(IApiKeyService apiKeyService) :
        IRequestHandler<CreateApiKeyCommand, CreateApiKeyResult>,
        IRequestHandler<GetApiKeyQuery, EditApiKeyViewModel>,
        IRequestHandler<GetCustomerApiKeyQuery, ApiKeyManagementViewModel>,
        IRequestHandler<RevokeApiKeyCommand, Result>,
        IRequestHandler<UpdateApiKeyCommand, Result>
    {
        private readonly IApiKeyService apiKeyService = apiKeyService;

        public Task<CreateApiKeyResult> Handle(CreateApiKeyCommand request, CancellationToken cancellationToken)
            => apiKeyService.CreateApiKey(request.CustId, request.ApiName, request.PassPhrase, request.Scopes, request.IP, request.Language, request.ExpirationDate);


        public async Task<EditApiKeyViewModel> Handle(GetApiKeyQuery request, CancellationToken cancellationToken)
        {
            var result = await apiKeyService.GetApiKeyByKey(request.Key);
            var scopes = new List<string> { ApiKeyScope.WinLossDetail, ApiKeyScope.Outstanding };
            return new EditApiKeyViewModel(result, scopes, request.Language);
        }

        public async Task<ApiKeyManagementViewModel> Handle(GetCustomerApiKeyQuery request, CancellationToken cancellationToken)
        {
            var result = await apiKeyService.GetApiKeyByCustId(request.CustId);
            return new ApiKeyManagementViewModel(request.CustLevel.DisplayName, result, request.Language);
        }

        public async Task<Result> Handle(RevokeApiKeyCommand request, CancellationToken cancellationToken)
        {
            await apiKeyService.RevokeApiKey(request.Key, request.IP);
            return new Result(0, "Success");
        }

        public Task<Result> Handle(UpdateApiKeyCommand request, CancellationToken cancellationToken)
        {
            var result = apiKeyService.UpdateApiKey(request.ApiKey, request.CustId, request.ApiName, request.Scopes, request.IP, request.Language, request.ExpirationDate);
            return Task.FromResult(new Result(result.Result.Code, result.Result.Message));
        }

    }
}
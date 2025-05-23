using AgencyCash.API.Payments.Models;
using AgencyCash.API.Shared.Models;

namespace AgencyCash.API.Payments.Services;

public class DirePayProviderService : IPaymentProviderService
{
    private readonly IPaymentProviderConfigService _configService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly Fanex.Logging.ILogger _logger;

    public PaymentProvider Provider => PaymentProvider.DirePay;

    public DirePayProviderService(
        IPaymentProviderConfigService configService,
        IHttpClientFactory httpClientFactory,
        Fanex.Logging.ILogger logger)
    {
        _configService = configService;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<Result> ProcessWithdrawal(string accountNumber, decimal amount, string currency, bool isAutoApprove)
    {
        try
        {
            var config = await _configService.GetConfig(Provider, 0); // TODO: Get agentId from context
            if (config == null || !config.IsEnabled)
            {
                return new Result(602, "DirePay provider is not enabled");
            }

            if (!await ValidateAccount(accountNumber))
            {
                return new Result(603, "Invalid account number");
            }

            var fee = await CalculateFee(amount);
            var totalAmount = amount + fee;

            if (totalAmount < config.MinAmount || totalAmount > config.MaxAmount)
            {
                return new Result(604, $"Amount must be between {config.MinAmount} and {config.MaxAmount}");
            }

            // TODO: Implement actual DirePay API call
            // This is a placeholder for the actual implementation
            var client = _httpClientFactory.CreateClient("DirePay");
            // var response = await client.PostAsync(...);

            return new Result(0, "Withdrawal processed successfully");
        }
        catch (Exception ex)
        {
            await _logger.ErrorAsync($"DirePay withdrawal failed: {ex.Message}", ex);
            return new Result(605, "Failed to process withdrawal");
        }
    }

    public async Task<Result> ProcessDeposit(string accountNumber, decimal amount, string currency)
    {
        try
        {
            var config = await _configService.GetConfig(Provider, 0); // TODO: Get agentId from context
            if (config == null || !config.IsEnabled)
            {
                return new Result(602, "DirePay provider is not enabled");
            }

            if (!await ValidateAccount(accountNumber))
            {
                return new Result(603, "Invalid account number");
            }

            var fee = await CalculateFee(amount);
            var totalAmount = amount + fee;

            if (totalAmount < config.MinAmount || totalAmount > config.MaxAmount)
            {
                return new Result(604, $"Amount must be between {config.MinAmount} and {config.MaxAmount}");
            }

            // TODO: Implement actual DirePay API call
            // This is a placeholder for the actual implementation
            var client = _httpClientFactory.CreateClient("DirePay");
            // var response = await client.PostAsync(...);

            return new Result(0, "Deposit processed successfully");
        }
        catch (Exception ex)
        {
            await _logger.ErrorAsync($"DirePay deposit failed: {ex.Message}", ex);
            return new Result(605, "Failed to process deposit");
        }
    }

    public async Task<decimal> CalculateFee(decimal amount)
    {
        var config = await _configService.GetConfig(Provider, 0); // TODO: Get agentId from context
        if (config == null)
        {
            return 0;
        }

        if (config.FeeType.ToLower() == "percentage")
        {
            return amount * (config.Fee / 100);
        }
        return config.Fee;
    }

    public async Task<bool> ValidateAccount(string accountNumber)
    {
        try
        {
            var config = await _configService.GetConfig(Provider, 0); // TODO: Get agentId from context
            if (config == null)
            {
                return false;
            }

            // TODO: Implement actual DirePay account validation
            // This is a placeholder for the actual implementation
            var client = _httpClientFactory.CreateClient("DirePay");
            // var response = await client.GetAsync($"/validate-account/{accountNumber}");
            // return response.IsSuccessStatusCode;

            return true;
        }
        catch (Exception ex)
        {
            await _logger.ErrorAsync($"DirePay account validation failed: {ex.Message}", ex);
            return false;
        }
    }
} 
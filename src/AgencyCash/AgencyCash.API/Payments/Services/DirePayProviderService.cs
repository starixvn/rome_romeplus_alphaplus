using AgencyCash.API.Payments.Models;
using AgencyCash.API.Shared.Models;
using System.Text;
using System.Text.Json;

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

            var client = _httpClientFactory.CreateClient("DirePay");
            
            // Prepare withdrawal request
            var request = new DirePayPayoutRequest
            {
                MerchantId = config.ApiKey,
                OrderId = Guid.NewGuid().ToString("N"),
                Amount = totalAmount,
                Currency = currency,
                AccountNumber = accountNumber,
                CreatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                CallbackUrl = config.BaseUrl + "/api/payments/direpay/callback",
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };

            // Build parameters dictionary for signature
            var parameters = new Dictionary<string, object>
            {
                ["merchantId"] = request.MerchantId,
                ["orderId"] = request.OrderId,
                ["amount"] = request.Amount,
                ["currency"] = request.Currency,
                ["accountNumber"] = request.AccountNumber,
                ["createdAt"] = request.CreatedAt,
                ["callbackUrl"] = request.CallbackUrl,
                ["timestamp"] = request.Timestamp
            };
            request.Signature = DirePaySignatureUtil.GenerateSignature(parameters, config.ApiSecret);

            // Send request
            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("/api/v1/payouts", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await _logger.ErrorAsync($"DirePay withdrawal failed: {responseContent}");
                return new Result(605, "Failed to process withdrawal");
            }

            // Parse response
            var result = JsonSerializer.Deserialize<DirePayPayoutResponse>(responseContent);
            if (result?.StatusCode != 0)
            {
                return new Result(605, result?.Message ?? "Failed to process withdrawal");
            }

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

            var client = _httpClientFactory.CreateClient("DirePay");
            
            // Prepare deposit request
            var request = new DirePayPayoutRequest
            {
                MerchantId = config.ApiKey,
                OrderId = Guid.NewGuid().ToString("N"),
                Amount = totalAmount,
                Currency = currency,
                AccountNumber = accountNumber,
                CreatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                CallbackUrl = config.BaseUrl + "/api/payments/direpay/callback",
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };

            // Build parameters dictionary for signature
            var parameters = new Dictionary<string, object>
            {
                ["merchantId"] = request.MerchantId,
                ["orderId"] = request.OrderId,
                ["amount"] = request.Amount,
                ["currency"] = request.Currency,
                ["accountNumber"] = request.AccountNumber,
                ["createdAt"] = request.CreatedAt,
                ["callbackUrl"] = request.CallbackUrl,
                ["timestamp"] = request.Timestamp
            };
            request.Signature = DirePaySignatureUtil.GenerateSignature(parameters, config.ApiSecret);

            // Send request
            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("/api/v1/deposits", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await _logger.ErrorAsync($"DirePay deposit failed: {responseContent}");
                return new Result(605, "Failed to process deposit");
            }

            // Parse response
            var result = JsonSerializer.Deserialize<DirePayPayoutResponse>(responseContent);
            if (result?.StatusCode != 0)
            {
                return new Result(605, result?.Message ?? "Failed to process deposit");
            }

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

            var client = _httpClientFactory.CreateClient("DirePay");
            
            // Prepare validation request
            var request = new
            {
                merchantId = config.ApiKey,
                accountNumber,
                timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };

            // Generate signature
            var parameters = new Dictionary<string, object>
            {
                ["merchantId"] = request.merchantId,
                ["accountNumber"] = request.accountNumber,
                ["timestamp"] = request.timestamp
            };
            var signature = DirePaySignatureUtil.GenerateSignature(parameters, config.ApiSecret);

            // Add headers
            client.DefaultRequestHeaders.Add("X-Signature", signature);

            // Send request
            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("/api/v1/validate-account", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await _logger.ErrorAsync($"DirePay account validation failed: {responseContent}");
                return false;
            }

            // Parse response
            var result = JsonSerializer.Deserialize<DirePayPayoutResponse>(responseContent);
            return result?.StatusCode == 0;
        }
        catch (Exception ex)
        {
            await _logger.ErrorAsync($"DirePay account validation failed: {ex.Message}", ex);
            return false;
        }
    }

    public async Task<Result<IPayoutStatusResponse>> CheckPayoutStatusAsync(string tradeNo, string orderNo, PaymentProviderConfig config)
    {
        try
        {
            var request = new DirePayPayoutStatusRequest
            {
                MerchantId = config.ApiKey,
                TransactionId = tradeNo,
                OrderId = orderNo,
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };

            // Build parameters dictionary for signature
            var parameters = new Dictionary<string, object>
            {
                ["merchantId"] = request.MerchantId,
                ["transactionId"] = request.TransactionId,
                ["orderId"] = request.OrderId,
                ["timestamp"] = request.Timestamp
            };
            request.Signature = DirePaySignatureUtil.GenerateSignature(parameters, config.ApiSecret);

            var client = _httpClientFactory.CreateClient("DirePay");
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/payouts/status", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await _logger.ErrorAsync($"DirePay payout status check failed: {responseContent}");
                return new Result<IPayoutStatusResponse>(605, "Failed to check payout status");
            }

            var statusResponse = JsonSerializer.Deserialize<DirePayPayoutStatusResponse>(responseContent);
            if (statusResponse == null || statusResponse.StatusCode != 0)
            {
                return new Result<IPayoutStatusResponse>(605, statusResponse?.Message ?? "Failed to check payout status");
            }

            var result = new Result<IPayoutStatusResponse>(0, "Payout status checked successfully");
            result.Data = statusResponse;
            return result;
        }
        catch (Exception ex)
        {
            await _logger.ErrorAsync($"DirePay payout status check failed: {ex.Message}", ex);
            return new Result<IPayoutStatusResponse>(605, "Failed to check payout status");
        }
    }

    public async Task<Result<IPayoutBalanceResponse>> GetBalanceAsync(PaymentProviderConfig config)
    {
        try
        {
            var request = new DirePayPayoutBalanceRequest
            {
                MerchantId = config.ApiKey,
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };

            // Build parameters dictionary for signature
            var parameters = new Dictionary<string, object>
            {
                ["merchantId"] = request.MerchantId,
                ["timestamp"] = request.Timestamp
            };
            request.Signature = DirePaySignatureUtil.GenerateSignature(parameters, config.ApiSecret);

            var client = _httpClientFactory.CreateClient("DirePay");
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/balance", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await _logger.ErrorAsync($"DirePay balance check failed: {responseContent}");
                return new Result<IPayoutBalanceResponse>(605, "Failed to check balance");
            }

            var balanceResponse = JsonSerializer.Deserialize<DirePayPayoutBalanceResponse>(responseContent);
            if (balanceResponse == null || balanceResponse.StatusCode != 0)
            {
                return new Result<IPayoutBalanceResponse>(605, balanceResponse?.Message ?? "Failed to check balance");
            }

            var result = new Result<IPayoutBalanceResponse>(0, "Balance checked successfully");
            result.Data = balanceResponse;
            return result;
        }
        catch (Exception ex)
        {
            await _logger.ErrorAsync($"DirePay balance check failed: {ex.Message}", ex);
            return new Result<IPayoutBalanceResponse>(605, "Failed to check balance");
        }
    }
} 
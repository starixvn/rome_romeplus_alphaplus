using AgencyCash.API.Payments.Models;
using AgencyCash.API.Shared.Models;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using System.Collections.Generic;

namespace AgencyCash.API.Payments.Services;

public class U2PayProviderService : IPaymentProviderService
{
    private readonly IPaymentProviderConfigService _configService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly Fanex.Logging.ILogger _logger;

    public PaymentProvider Provider => PaymentProvider.U2Pay;

    public U2PayProviderService(
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
                return new Result(602, "U2Pay provider is not enabled");
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

            var client = _httpClientFactory.CreateClient("U2Pay");
            
            // Prepare withdrawal request
            var request = new
            {
                accountNumber,
                amount = totalAmount,
                currency,
                autoApprove = isAutoApprove,
                timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };

            // Generate signature
            var signature = GenerateSignature(request, config.ApiSecret);

            // Add headers
            client.DefaultRequestHeaders.Add("X-Signature", signature);

            // Send request
            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("/api/v1/withdrawals", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await _logger.ErrorAsync($"U2Pay withdrawal failed: {responseContent}");
                return new Result(605, "Failed to process withdrawal");
            }

            // Parse response
            var result = JsonSerializer.Deserialize<U2PayResponse>(responseContent);
            if (result?.Status != "success")
            {
                return new Result(605, result?.Message ?? "Failed to process withdrawal");
            }

            return new Result(0, "Withdrawal processed successfully");
        }
        catch (Exception ex)
        {
            await _logger.ErrorAsync($"U2Pay withdrawal failed: {ex.Message}", ex);
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
                return new Result(602, "U2Pay provider is not enabled");
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

            var client = _httpClientFactory.CreateClient("U2Pay");
            
            // Prepare deposit request
            var request = new
            {
                accountNumber,
                amount = totalAmount,
                currency,
                timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };

            // Generate signature
            var signature = GenerateSignature(request, config.ApiSecret);

            // Add headers
            client.DefaultRequestHeaders.Add("X-Signature", signature);

            // Send request
            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("/api/v1/deposits", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await _logger.ErrorAsync($"U2Pay deposit failed: {responseContent}");
                return new Result(605, "Failed to process deposit");
            }

            // Parse response
            var result = JsonSerializer.Deserialize<U2PayResponse>(responseContent);
            if (result?.Status != "success")
            {
                return new Result(605, result?.Message ?? "Failed to process deposit");
            }

            return new Result(0, "Deposit processed successfully");
        }
        catch (Exception ex)
        {
            await _logger.ErrorAsync($"U2Pay deposit failed: {ex.Message}", ex);
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

            var client = _httpClientFactory.CreateClient("U2Pay");
            
            // Prepare validation request
            var request = new
            {
                accountNumber,
                timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };

            // Generate signature
            var signature = GenerateSignature(request, config.ApiSecret);

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
                await _logger.ErrorAsync($"U2Pay account validation failed: {responseContent}");
                return false;
            }

            // Parse response
            var result = JsonSerializer.Deserialize<U2PayResponse>(responseContent);
            return result?.Status == "success";
        }
        catch (Exception ex)
        {
            await _logger.ErrorAsync($"U2Pay account validation failed: {ex.Message}", ex);
            return false;
        }
    }

    private string GenerateSignature(object request, string secret)
    {
        var json = JsonSerializer.Serialize(request);
        using var hmac = new System.Security.Cryptography.HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(json));
        return Convert.ToBase64String(hash);
    }

    public async Task<Result> ProcessPayoutAsync(U2PayPayoutRequest payoutRequest, PaymentProviderConfig config)
    {
        try
        {
            // Build parameters dictionary for signature
            var parameters = new Dictionary<string, object>
            {
                ["merchantNo"] = payoutRequest.MerchantNo,
                ["orderNo"] = payoutRequest.OrderNo,
                ["amount"] = payoutRequest.Amount,
                ["name"] = payoutRequest.Name,
                ["bankName"] = payoutRequest.BankName,
                ["bankAccount"] = payoutRequest.BankAccount,
                ["bankBranch"] = payoutRequest.BankBranch,
                ["memo"] = payoutRequest.Memo,
                ["mobile"] = payoutRequest.Mobile,
                ["datetime"] = payoutRequest.Datetime,
                ["notifyUrl"] = payoutRequest.NotifyUrl,
                ["reverseUrl"] = payoutRequest.ReverseUrl,
                ["extra"] = payoutRequest.Extra,
                ["time"] = payoutRequest.Time
            };
            payoutRequest.Sign = U2PaySignatureUtil.GenerateSignature(parameters, config.ApiSecret);

            // Remove appSecret from payload
            payoutRequest.AppSecret = null;

            var client = _httpClientFactory.CreateClient("U2Pay");
            var content = new StringContent(JsonSerializer.Serialize(payoutRequest), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/payout/create", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await _logger.ErrorAsync($"U2Pay payout failed: {responseContent}");
                return new Result(605, "Failed to process payout");
            }

            var payoutResponse = JsonSerializer.Deserialize<U2PayPayoutResponse>(responseContent);
            if (payoutResponse == null || payoutResponse.Code != 0)
            {
                return new Result(605, payoutResponse?.Text ?? "Failed to process payout");
            }

            return new Result(0, "Payout processed successfully");
        }
        catch (Exception ex)
        {
            await _logger.ErrorAsync($"U2Pay payout failed: {ex.Message}", ex);
            return new Result(605, "Failed to process payout");
        }
    }

    public async Task<Result<IPayoutStatusResponse>> CheckPayoutStatusAsync(string tradeNo, string orderNo, PaymentProviderConfig config)
    {
        try
        {
            var request = new U2PayPayoutStatusRequest
            {
                MerchantNo = config.ApiKey,
                TradeNo = tradeNo,
                OrderNo = orderNo,
                Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };

            // Build parameters dictionary for signature
            var parameters = new Dictionary<string, object>
            {
                ["merchantNo"] = request.MerchantNo,
                ["tradeNo"] = request.TradeNo,
                ["orderNo"] = request.OrderNo,
                ["time"] = request.Time
            };
            request.Sign = U2PaySignatureUtil.GenerateSignature(parameters, config.ApiSecret);

            var client = _httpClientFactory.CreateClient("U2Pay");
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/payout/status", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await _logger.ErrorAsync($"U2Pay payout status check failed: {responseContent}");
                return new Result<IPayoutStatusResponse>(605, "Failed to check payout status");
            }

            var statusResponse = JsonSerializer.Deserialize<U2PayPayoutStatusResponse>(responseContent);
            if (statusResponse == null || statusResponse.Code != 0)
            {
                return new Result<IPayoutStatusResponse>(605, statusResponse?.Text ?? "Failed to check payout status");
            }

            var result = new Result<IPayoutStatusResponse>(0, "Payout status checked successfully");
            result.Data = statusResponse;
            return result;
        }
        catch (Exception ex)
        {
            await _logger.ErrorAsync($"U2Pay payout status check failed: {ex.Message}", ex);
            return new Result<IPayoutStatusResponse>(605, "Failed to check payout status");
        }
    }

    public async Task<Result<IPayoutBalanceResponse>> GetBalanceAsync(PaymentProviderConfig config)
    {
        try
        {
            var request = new U2PayPayoutBalanceRequest
            {
                MerchantNo = config.ApiKey,
                Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };

            // Build parameters dictionary for signature
            var parameters = new Dictionary<string, object>
            {
                ["merchantNo"] = request.MerchantNo,
                ["time"] = request.Time
            };
            request.Sign = U2PaySignatureUtil.GenerateSignature(parameters, config.ApiSecret);

            var client = _httpClientFactory.CreateClient("U2Pay");
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/payout/balance", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await _logger.ErrorAsync($"U2Pay balance check failed: {responseContent}");
                return new Result<IPayoutBalanceResponse>(605, "Failed to check balance");
            }

            var balanceResponse = JsonSerializer.Deserialize<U2PayPayoutBalanceResponse>(responseContent);
            if (balanceResponse == null || balanceResponse.Code != 0)
            {
                return new Result<IPayoutBalanceResponse>(605, balanceResponse?.Text ?? "Failed to check balance");
            }

            var result = new Result<IPayoutBalanceResponse>(0, "Balance checked successfully");
            result.Data = balanceResponse;
            return result;
        }
        catch (Exception ex)
        {
            await _logger.ErrorAsync($"U2Pay balance check failed: {ex.Message}", ex);
            return new Result<IPayoutBalanceResponse>(605, "Failed to check balance");
        }
    }

    private class U2PayResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string TransactionId { get; set; }
    }
} 
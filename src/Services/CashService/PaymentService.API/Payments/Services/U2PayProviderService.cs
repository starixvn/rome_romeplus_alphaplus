using AlphaPlus.PaymentService.API.Payments.Models;
using AlphaPlus.PaymentService.API.Shared.Models;
using System.Text;
using System.Text.Json;

namespace AlphaPlus.PaymentService.API.Payments.Services;

public class U2PayProviderService : IPaymentProviderService
{
    private readonly HttpClient _httpClient;
    private readonly Fanex.Logging.ILogger _logger;

    public PaymentProvider Provider => PaymentProvider.U2Pay;

    public U2PayProviderService(
        HttpClient httpClient,
        Fanex.Logging.ILogger logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<Result> ProcessDeposit(string accountNumber, decimal amount, string currency, PaymentProviderConfig config)
    {
        try
        {
            if (config == null || !config.U2Pay.IsEnabled)
            {
                return new Result(601, "U2Pay provider is not enabled");
            }

            if (!await ValidateAccount(accountNumber, config))
            {
                return new Result(602, "Invalid account number");
            }

            var fee = CalculateFee(amount, config);
            var totalAmount = amount + fee;

            if (totalAmount < config.U2Pay.MinAmount || totalAmount > config.U2Pay.MaxAmount)
            {
                return new Result(603, $"Amount must be between {config.U2Pay.MinAmount} and {config.U2Pay.MaxAmount}");
            }

            // Prepare deposit request
            var request = new
            {
                accountNumber,
                amount = totalAmount,
                currency,
                timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };

            // Generate signature
            var signature = GenerateSignature(request, config.U2Pay.ApiSecret);

            // Add headers
            _httpClient.DefaultRequestHeaders.Add("X-Signature", signature);

            // Send request as form data
            var formData = new Dictionary<string, string>
            {
                ["accountNumber"] = request.accountNumber,
                ["amount"] = request.amount.ToString(),
                ["currency"] = request.currency,
                ["timestamp"] = request.timestamp.ToString()
            };

            var content = new FormUrlEncodedContent(formData);
            var response = await _httpClient.PostAsync("/api/v1/deposits", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await _logger.ErrorAsync($"U2Pay deposit failed: {responseContent}");
                return new Result(604, "Failed to process deposit");
            }

            // Parse response
            var result = JsonSerializer.Deserialize<U2PayResponse>(responseContent);
            if (result?.Status != "success")
            {
                return new Result(604, result?.Message ?? "Failed to process deposit");
            }

            return new Result(0, "Deposit processed successfully");
        }
        catch (Exception ex)
        {
            await _logger.ErrorAsync($"U2Pay deposit failed: {ex.Message}", ex);
            return new Result(500, "An error occurred while processing deposit request.");
        }
    }

    public decimal CalculateFee(decimal amount, PaymentProviderConfig config)
    {
        if (config == null)
        {
            return 0;
        }

        if (config.U2Pay.FeeType.ToLower() == "percentage")
        {
            return amount * (config.U2Pay.Fee / 100);
        }
        return config.U2Pay.Fee;
    }

    public async Task<bool> ValidateAccount(string accountNumber, PaymentProviderConfig config)
    {
        try
        {
            if (config == null)
            {
                return false;
            }

            // Prepare validation request
            var request = new
            {
                accountNumber,
                timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };

            // Generate signature
            var signature = GenerateSignature(request, config.U2Pay.ApiSecret);

            // Add headers
            _httpClient.DefaultRequestHeaders.Add("X-Signature", signature);

            // Send request as form data
            var formData = new Dictionary<string, string>
            {
                ["accountNumber"] = request.accountNumber,
                ["timestamp"] = request.timestamp.ToString()
            };

            var content = new FormUrlEncodedContent(formData);
            var response = await _httpClient.PostAsync("/api/v1/validate-account", content);
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

    public async Task<Result> ProcessPayout(PayoutRequest payoutRequest, PaymentProviderConfig config)
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
            payoutRequest.Sign = PaySignatureUtil.GenerateSignature(parameters, config.U2Pay.ApiSecret);

            // Remove appSecret from payload
            payoutRequest.AppSecret = null;

            // Convert to form data
            var formData = new Dictionary<string, string>
            {
                ["merchantNo"] = payoutRequest.MerchantNo,
                ["orderNo"] = payoutRequest.OrderNo,
                ["amount"] = payoutRequest.Amount.ToString(),
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
                ["time"] = payoutRequest.Time.ToString(),
                ["sign"] = payoutRequest.Sign
            };

            var content = new FormUrlEncodedContent(formData);
            var response = await _httpClient.PostAsync("/payout/create", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await _logger.ErrorAsync($"U2Pay payout failed: {responseContent}");
                return new Result(601, "Failed to process payout");
            }

            var payoutResponse = JsonSerializer.Deserialize<PayoutResponse>(responseContent);
            if (payoutResponse == null || payoutResponse.Code != 0)
            {
                return new Result(601, payoutResponse?.Text ?? "Failed to process payout");
            }

            return new Result(0, "Payout processed successfully");
        }
        catch (Exception ex)
        {
            await _logger.ErrorAsync($"U2Pay payout failed: {ex.Message}", ex);
            return new Result(500, "An error occurred while processing payout request.");
        }
    }

    public async Task<Result<IPayoutStatusResponse>> CheckPayoutStatusAsync(string tradeNo, string orderNo, PaymentProviderConfig config)
    {
        try
        {
            var request = new PayoutStatusRequest
            {
                MerchantNo = config.U2Pay.ApiKey,
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
            request.Sign = PaySignatureUtil.GenerateSignature(parameters, config.U2Pay.ApiSecret);

            // Convert to form data
            var formData = new Dictionary<string, string>
            {
                ["merchantNo"] = request.MerchantNo,
                ["tradeNo"] = request.TradeNo,
                ["orderNo"] = request.OrderNo,
                ["time"] = request.Time.ToString(),
                ["sign"] = request.Sign
            };

            var content = new FormUrlEncodedContent(formData);
            var response = await _httpClient.PostAsync("/payout/status", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await _logger.ErrorAsync($"U2Pay payout status check failed: {responseContent}");
                return new Result<IPayoutStatusResponse>(601, "Failed to check payout status");
            }

            var statusResponse = JsonSerializer.Deserialize<PayoutStatusResponse>(responseContent);
            if (statusResponse == null || statusResponse.Code != 0)
            {
                return new Result<IPayoutStatusResponse>(601, statusResponse?.Text ?? "Failed to check payout status");
            }

            var result = new Result<IPayoutStatusResponse>(0, "Payout status checked successfully");
            result.Data = statusResponse;
            return result;
        }
        catch (Exception ex)
        {
            await _logger.ErrorAsync($"U2Pay payout status check failed: {ex.Message}", ex);
            return new Result<IPayoutStatusResponse>(500, "An error occurred while processing payout status check.");
        }
    }

    public async Task<Result<IPayoutBalanceResponse>> GetBalanceAsync(PaymentProviderConfig config)
    {
        try
        {
            var request = new PayoutBalanceRequest
            {
                MerchantNo = config.U2Pay.ApiKey,
                Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };

            // Build parameters dictionary for signature
            var parameters = new Dictionary<string, object>
            {
                ["merchantNo"] = request.MerchantNo,
                ["time"] = request.Time
            };
            request.Sign = PaySignatureUtil.GenerateSignature(parameters, config.U2Pay.ApiSecret);

            // Convert to form data
            var formData = new Dictionary<string, string>
            {
                ["merchantNo"] = request.MerchantNo,
                ["time"] = request.Time.ToString(),
                ["sign"] = request.Sign
            };

            var content = new FormUrlEncodedContent(formData);
            var response = await _httpClient.PostAsync("/payout/balance", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await _logger.ErrorAsync($"U2Pay balance check failed: {responseContent}");
                return new Result<IPayoutBalanceResponse>(601, "Failed to check balance");
            }

            var balanceResponse = JsonSerializer.Deserialize<PayoutBalanceResponse>(responseContent);
            if (balanceResponse == null || balanceResponse.Code != 0)
            {
                return new Result<IPayoutBalanceResponse>(601, balanceResponse?.Text ?? "Failed to check balance");
            }

            var result = new Result<IPayoutBalanceResponse>(0, "Balance checked successfully");
            result.Data = balanceResponse;
            return result;
        }
        catch (Exception ex)
        {
            await _logger.ErrorAsync($"U2Pay balance check failed: {ex.Message}", ex);
            return new Result<IPayoutBalanceResponse>(500, "An error occurred while processing withdrawal request.");
        }
    }

    private class U2PayResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string TransactionId { get; set; }
    }
} 
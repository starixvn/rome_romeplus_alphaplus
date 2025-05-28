using AlphaPlus.PaymentService.API.Payments.Models;
using AlphaPlus.PaymentService.API.Shared.Models;
using System.Text;
using System.Text.Json;

namespace AlphaPlus.PaymentService.API.Payments.Services;

public class DirePayProviderService : IPaymentProviderService
{
    private readonly HttpClient _httpClient;
    private readonly Fanex.Logging.ILogger _logger;

    public PaymentProvider Provider => PaymentProvider.DirePay;

    public DirePayProviderService(
        HttpClient httpClient,
        Fanex.Logging.ILogger logger)
    {
        _httpClient = httpClient;
        _logger = logger;
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
            payoutRequest.Sign = DirePaySignatureUtil.GenerateSignature(parameters, config.DirePay.ApiSecret);

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
                await _logger.ErrorAsync($"DirePay payout failed: {responseContent}");
                return new Result(605, "Failed to process payout");
            }

            var payoutResponse = JsonSerializer.Deserialize<PayoutResponse>(responseContent);
            if (payoutResponse == null || payoutResponse.Code != 0)
            {
                return new Result(605, payoutResponse?.Text ?? "Failed to process payout");
            }

            return new Result(0, "Payout processed successfully");
        }
        catch (Exception ex)
        {
            await _logger.ErrorAsync($"DirePay payout failed: {ex.Message}", ex);
            return new Result(605, "Failed to process payout");
        }
    }

    public async Task<Result> ProcessDeposit(string accountNumber, decimal amount, string currency, PaymentProviderConfig config)
    {
        try
        {
            if (config == null || !config.DirePay.IsEnabled)
            {
                return new Result(602, "DirePay provider is not enabled");
            }

            if (!await ValidateAccount(accountNumber, config))
            {
                return new Result(603, "Invalid account number");
            }

            var fee = CalculateFee(amount, config);
            var totalAmount = amount + fee;

            if (totalAmount < config.DirePay.MinAmount || totalAmount > config.DirePay.MaxAmount)
            {
                return new Result(604, $"Amount must be between {config.DirePay.MinAmount} and {config.DirePay.MaxAmount}");
            }

            // Prepare deposit request
            var request = new DirePayPayoutRequest
            {
                MerchantId = config.DirePay.ApiKey,
                OrderId = Guid.NewGuid().ToString("N"),
                Amount = totalAmount,
                Currency = currency,
                AccountNumber = accountNumber,
                CreatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                CallbackUrl = config.DirePay.BaseUrl + "/api/payments/direpay/callback",
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
            request.Signature = DirePaySignatureUtil.GenerateSignature(parameters, config.DirePay.ApiSecret);

            // Convert to form data
            var formData = new Dictionary<string, string>
            {
                ["merchantId"] = request.MerchantId,
                ["orderId"] = request.OrderId,
                ["amount"] = request.Amount.ToString(),
                ["currency"] = request.Currency,
                ["accountNumber"] = request.AccountNumber,
                ["createdAt"] = request.CreatedAt,
                ["callbackUrl"] = request.CallbackUrl,
                ["timestamp"] = request.Timestamp.ToString(),
                ["signature"] = request.Signature
            };

            var content = new FormUrlEncodedContent(formData);
            var response = await _httpClient.PostAsync("/api/v1/deposits", content);
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

    public decimal CalculateFee(decimal amount, PaymentProviderConfig config)
    {
        if (config == null)
        {
            return 0;
        }

        if (config.DirePay.FeeType.ToLower() == "percentage")
        {
            return amount * (config.DirePay.Fee / 100);
        }
        return config.DirePay.Fee;
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
                merchantId = config.DirePay.ApiKey,
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
            var signature = DirePaySignatureUtil.GenerateSignature(parameters, config.DirePay.ApiSecret);

            // Add headers
            _httpClient.DefaultRequestHeaders.Add("X-Signature", signature);

            // Convert to form data
            var formData = new Dictionary<string, string>
            {
                ["merchantId"] = request.merchantId,
                ["accountNumber"] = request.accountNumber,
                ["timestamp"] = request.timestamp.ToString()
            };

            var content = new FormUrlEncodedContent(formData);
            var response = await _httpClient.PostAsync("/api/v1/validate-account", content);
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
                MerchantId = config.DirePay.ApiKey,
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
            request.Signature = DirePaySignatureUtil.GenerateSignature(parameters, config.DirePay.ApiSecret);

            // Convert to form data
            var formData = new Dictionary<string, string>
            {
                ["merchantId"] = request.MerchantId,
                ["transactionId"] = request.TransactionId,
                ["orderId"] = request.OrderId,
                ["timestamp"] = request.Timestamp.ToString(),
                ["signature"] = request.Signature
            };

            var content = new FormUrlEncodedContent(formData);
            var response = await _httpClient.PostAsync("/api/v1/payouts/status", content);
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
                MerchantId = config.DirePay.ApiKey,
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };

            // Build parameters dictionary for signature
            var parameters = new Dictionary<string, object>
            {
                ["merchantId"] = request.MerchantId,
                ["timestamp"] = request.Timestamp
            };
            request.Signature = DirePaySignatureUtil.GenerateSignature(parameters, config.DirePay.ApiSecret);

            // Convert to form data
            var formData = new Dictionary<string, string>
            {
                ["merchantId"] = request.MerchantId,
                ["timestamp"] = request.Timestamp.ToString(),
                ["signature"] = request.Signature
            };

            var content = new FormUrlEncodedContent(formData);
            var response = await _httpClient.PostAsync("/api/v1/balance", content);
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
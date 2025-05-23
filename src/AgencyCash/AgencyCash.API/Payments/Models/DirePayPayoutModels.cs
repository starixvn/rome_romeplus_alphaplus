using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Linq;
using System.Text.Json.Serialization;
using AgencyCash.API.Payments.Services;

namespace AgencyCash.API.Payments.Models
{
    public class DirePayPayoutRequest
    {
        public string MerchantId { get; set; }
        public string OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string BeneficiaryName { get; set; }
        public string BankCode { get; set; }
        public string AccountNumber { get; set; }
        public string BranchCode { get; set; } // optional
        public string Description { get; set; } // optional
        public string PhoneNumber { get; set; } // optional
        public string CreatedAt { get; set; } // yyyy-MM-dd HH:mm:ss
        public string CallbackUrl { get; set; }
        public string CancelUrl { get; set; } // optional
        public string Metadata { get; set; } // optional
        public long Timestamp { get; set; } // Unix timestamp
        [JsonIgnore] public string ApiSecret { get; set; } // not sent
        public string Signature { get; set; }
    }

    public class DirePayPayoutResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string TransactionId { get; set; }
        public string OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
    }

    public class DirePayPayoutStatusRequest
    {
        public string MerchantId { get; set; }
        public string TransactionId { get; set; }
        public string OrderId { get; set; }
        public long Timestamp { get; set; }
        [JsonIgnore] public string ApiSecret { get; set; }
        public string Signature { get; set; }
    }

    public class DirePayPayoutStatusResponse : IPayoutStatusResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Status { get; set; } // PENDING, COMPLETED, FAILED, etc.
        public decimal PaidAmount { get; set; }
        public string TransactionId { get; set; }
        public string OrderId { get; set; }
        public string CompletedAt { get; set; }
    }

    public class DirePayPayoutBalanceRequest
    {
        public string MerchantId { get; set; }
        public long Timestamp { get; set; }
        [JsonIgnore] public string ApiSecret { get; set; }
        public string Signature { get; set; }
    }

    public class DirePayPayoutBalanceResponse : IPayoutBalanceResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public decimal AvailableBalance { get; set; }
        public decimal PendingBalance { get; set; }
        public decimal ReservedBalance { get; set; }
        public string Currency { get; set; }
        public string LastUpdated { get; set; }
    }

    public static class DirePaySignatureUtil
    {
        public static string GenerateSignature(IDictionary<string, object> parameters, string secret)
        {
            // Exclude branchCode, description, apiSecret from signing
            var exclude = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "branchCode", "description", "apiSecret" };
            var sorted = parameters
                .Where(kv => !exclude.Contains(kv.Key) && kv.Value != null)
                .OrderBy(kv => kv.Key, StringComparer.Ordinal)
                .Select(kv => $"{kv.Key}={kv.Value}");
            var keyValueString = string.Join("&", sorted);
            var signSource = keyValueString + secret;
            // SHA256
            using var sha256 = SHA256.Create();
            var sha256Bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(signSource));
            // MD5
            using var md5 = MD5.Create();
            var md5Bytes = md5.ComputeHash(sha256Bytes);
            var sign = BitConverter.ToString(md5Bytes).Replace("-", string.Empty).ToUpperInvariant();
            return sign;
        }
    }
} 
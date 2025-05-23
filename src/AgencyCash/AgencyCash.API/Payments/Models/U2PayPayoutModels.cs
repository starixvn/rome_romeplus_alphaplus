using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Linq;
using System.Text.Json.Serialization;
using AgencyCash.API.Payments.Services;

namespace AgencyCash.API.Payments.Models
{
    public class U2PayPayoutRequest
    {
        public string MerchantNo { get; set; }
        public string OrderNo { get; set; }
        public int Amount { get; set; } // Integer, no decimals
        public string Name { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string BankBranch { get; set; } // optional
        public string Memo { get; set; } // optional
        public string Mobile { get; set; } // optional
        public string Datetime { get; set; } // yyyy-MM-dd HH:mm:ss
        public string NotifyUrl { get; set; }
        public string ReverseUrl { get; set; } // optional
        public string Extra { get; set; } // optional
        public long Time { get; set; } // Unix timestamp
        [JsonIgnore] public string AppSecret { get; set; } // not sent
        public string Sign { get; set; }
    }

    public class U2PayPayoutResponse
    {
        public int Code { get; set; }
        public string Text { get; set; }
        public string TradeNo { get; set; }
        public string OrderNo { get; set; }
        public int Amount { get; set; }
    }

    public class U2PayPayoutStatusRequest
    {
        public string MerchantNo { get; set; }
        public string TradeNo { get; set; }
        public string OrderNo { get; set; }
        public long Time { get; set; }
        [JsonIgnore] public string AppSecret { get; set; }
        public string Sign { get; set; }
    }

    public class U2PayPayoutStatusResponse : IPayoutStatusResponse
    {
        public int Code { get; set; }
        public string Text { get; set; }
        public string Status { get; set; } // PENDING, PAID, etc.
        public int Paid { get; set; }
        public string TradeNo { get; set; }
        public string OrderNo { get; set; }

        int IPayoutStatusResponse.StatusCode => Code;
        string IPayoutStatusResponse.Message => Text;
        string IPayoutStatusResponse.TransactionId => TradeNo;
        string IPayoutStatusResponse.OrderId => OrderNo;
    }

    public class U2PayPayoutBalanceRequest
    {
        public string MerchantNo { get; set; }
        public long Time { get; set; }
        [JsonIgnore] public string AppSecret { get; set; }
        public string Sign { get; set; }
    }

    public class U2PayPayoutBalanceResponse : IPayoutBalanceResponse
    {
        public int Code { get; set; }
        public string Text { get; set; }
        public decimal Balance { get; set; }
        public decimal Balance0 { get; set; }
        public decimal Balance1 { get; set; }
        public decimal Balance2 { get; set; }
        public decimal Balance3 { get; set; }
        public decimal Balance4 { get; set; }
        public decimal Balance5 { get; set; }
        public string Datetime { get; set; }

        int IPayoutBalanceResponse.StatusCode => Code;
        string IPayoutBalanceResponse.Message => Text;
        decimal IPayoutBalanceResponse.AvailableBalance => Balance;
        string IPayoutBalanceResponse.Currency => "THB"; // U2Pay uses THB by default
        string IPayoutBalanceResponse.LastUpdated => Datetime;
    }

    public static class U2PaySignatureUtil
    {
        public static string GenerateSignature(IDictionary<string, object> parameters, string secret)
        {
            // Exclude bankBranch, memo, appSecret from signing
            var exclude = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "bankBranch", "memo", "appSecret" };
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
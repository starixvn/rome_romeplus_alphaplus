using AlphaPlus.CashService.API.Constants;
using AlphaPlus.CashService.API.Models.Payments;

namespace AlphaPlus.CashService.API.Services;

public class DummyDataService
{
    public static async Task<IEnumerable<ProviderSettingsModel>> GetDummyProviderSettings(int providerId, int method, int status, int transactionType)
    {
        return new List<ProviderSettingsModel>
        {
            new ProviderSettingsModel
            {
                ProviderId = providerId,
                Method = method,
                Status = status,
                TransactionType = transactionType,
                MinAmount = 10.00m,
                MaxAmount = 1000.00m,
                IsEnabled = true,
                BankRequired = false
            },
            new ProviderSettingsModel
            {
                ProviderId = providerId,
                Method = method,
                Status = status,
                TransactionType = transactionType,
                MinAmount = 20.00m,
                MaxAmount = 2000.00m,
                IsEnabled = true,
                BankRequired = true
            }
        };
    }

    public static async Task<IEnumerable<ProviderFeeModel>> GetDummyProviderFees(int providerId)
    {
        return new List<ProviderFeeModel>
        {
            new ProviderFeeModel
            {
                ProviderId = 1,
                TransactionType = "Deposit",
                Method = "Credit Card",
                Rate = 2.5m
            },
            new ProviderFeeModel
            {
                ProviderId = 2,
                TransactionType = "Withdrawal",
                Method = "Bank Transfer",
                Rate = 1.5m
            },
            new ProviderFeeModel
            {
                ProviderId = 3,
                TransactionType = "Deposit",
                Method = "E-Wallet",
                Rate = 1.0m
            },
            new ProviderFeeModel
            {
                ProviderId = 4,
                TransactionType = "Withdrawal",
                Method = "Cryptocurrency",
                Rate = 3.0m
            },
            new ProviderFeeModel
            {
                ProviderId = 5,
                TransactionType = "Deposit",
                Method = "PayPal",
                Rate = 2.0m
            }
        };
    }
    
    public static async Task<IEnumerable<ProviderModel>> GetDummyProviders()
    {
        return new List<ProviderModel>
        {
            new ProviderModel
            {
                Id = 1,
                Name = "Credit Card",
                Status = PaymentProviderStatusConstants.Active,
                LastUpdated = DateTime.Now.AddDays(-1)
            },
            new ProviderModel
            {
                Id = 2,
                Name = "Bank Transfer",
                Status = PaymentProviderStatusConstants.Active,
                LastUpdated = DateTime.Now.AddDays(-2)
            },
            new ProviderModel
            {
                Id = 3,
                Name = "E-Wallet",
                Status = PaymentProviderStatusConstants.Inactive,
                LastUpdated = DateTime.Now.AddDays(-5)
            },
            new ProviderModel
            {
                Id = 4,
                Name = "Cryptocurrency",
                Status = PaymentProviderStatusConstants.Active,
                LastUpdated = DateTime.Now.AddDays(-3)
            },
            new ProviderModel
            {
                Id = 5,
                Name = "PayPal",
                Status = PaymentProviderStatusConstants.Active,
                LastUpdated = DateTime.Now.AddDays(-1)
            }
        };
    }

    public static async Task<IEnumerable<PaymentMethodModel>> GetDummyPaymentMethods()
    {
        return new List<PaymentMethodModel>
        {
            new PaymentMethodModel
            {
                Id = 1,
                Name = "Credit Card",
                ProviderId = 1,
                TransactionType = 1, // Assuming 1 for Deposit
                Status = PaymentProviderStatusConstants.Active
            },
            new PaymentMethodModel
            {
                Id = 2,
                Name = "Bank Transfer",
                ProviderId = 2,
                TransactionType = 2, // Assuming 2 for Withdrawal
                Status = PaymentProviderStatusConstants.Active
            },
            new PaymentMethodModel
            {
                Id = 3,
                Name = "E-Wallet",
                ProviderId = 3,
                TransactionType = 1,
                Status = PaymentProviderStatusConstants.Inactive
            },
            new PaymentMethodModel
            {
                Id = 4,
                Name = "Cryptocurrency",
                ProviderId = 4,
                TransactionType = 1,
                Status = PaymentProviderStatusConstants.Active
            },
            new PaymentMethodModel
            {
                Id = 5,
                Name = "PayPal",
                ProviderId = 5,
                TransactionType = 1,
                Status = PaymentProviderStatusConstants.Active
            }
        };
    }
} 
export interface BankInfo {
  bankId: number;
  bankAccountNumber: string;
  bankCode: string;
}

export interface ProviderInfo {
  providerId: number;
  providerName: string;
  paymentMethodId: number;
  methodName: string;
}

export interface WithdrawalAgentBankDropDown {
  value: string;
  label: string;
  code: string;
  providerId: number;
  paymentMethodId: number;
}

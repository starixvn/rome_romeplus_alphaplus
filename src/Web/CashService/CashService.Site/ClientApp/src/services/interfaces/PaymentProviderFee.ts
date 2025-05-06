// Payment Provider Fee interface
export interface PaymentProviderFee {
    transactionType: string;
    method: string;
    rate: number;
    rateText: string;
} 
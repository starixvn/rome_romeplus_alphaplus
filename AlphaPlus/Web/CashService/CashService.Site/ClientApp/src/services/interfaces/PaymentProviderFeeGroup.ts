import { PaymentProviderFee } from './PaymentProviderFee';

// Payment Provider Fee Group interface
export interface PaymentProviderFeeGroup {
    transactionType: string;
    fees: PaymentProviderFee[];
} 
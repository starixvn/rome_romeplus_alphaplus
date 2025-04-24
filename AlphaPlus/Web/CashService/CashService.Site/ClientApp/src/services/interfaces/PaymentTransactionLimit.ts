// Payment Transaction Limit interface
export interface PaymentTransactionLimit {
    dailyWithdrawalCount: number;
    minPerDepositTransaction: number;
    maxPerDepositTransaction: number;
    minPerWithdrawalTransaction: number;
    maxPerWithdrawalTransaction: number;
} 
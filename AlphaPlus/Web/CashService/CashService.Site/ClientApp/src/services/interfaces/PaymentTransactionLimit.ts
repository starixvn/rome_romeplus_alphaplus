// Payment Transaction Limit interface
export interface PaymentTransactionLimit {
    minPerDepositTransaction: number;
    maxPerDepositTransaction: number;
    minPerWithdrawalTransaction: number;
    maxPerWithdrawalTransaction: number;
    dailyWithdrawalCount: number;
} 
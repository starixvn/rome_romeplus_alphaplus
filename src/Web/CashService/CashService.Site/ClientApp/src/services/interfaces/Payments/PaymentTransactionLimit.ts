// Payment Transaction Limit interface
export interface PaymentTransactionLimit {
    dailyWithdrawalCount: number;
    minPerDepositTransaction: number;
    maxPerDepositTransaction: number;
    minPerWithdrawalTransaction: number;
    maxPerWithdrawalTransaction: number;
} 

export const createDefaultPaymentTransactionLimit = (): PaymentTransactionLimit => {
    return {
        dailyWithdrawalCount: 0,
        minPerDepositTransaction: 0,
        maxPerDepositTransaction: 0,
        minPerWithdrawalTransaction: 0,
        maxPerWithdrawalTransaction: 0
    };
}; 
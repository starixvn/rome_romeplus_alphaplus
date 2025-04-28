export interface WithdrawalAccountInfo {
    id: number;
    username: string;
    loginName: string;
    firstName: string;
    lastName: string;
    amount: number;
    currency: string;
    from: number;
    to: number;
    referenceNo: string;
    dateAndTime: Date;
}
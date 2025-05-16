export interface CreateManualModel {
    custId: number;
    paymentFrom: string;
    paymentTo: string;
    amount: number;
    paySlipBase64: string;
    remark: string;
}


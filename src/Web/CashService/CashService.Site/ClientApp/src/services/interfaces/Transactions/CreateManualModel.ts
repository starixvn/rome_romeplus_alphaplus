export interface CreateManualModel {
    custId: number;
    uplineId: number;
    subAccId: number;
    agentGivenCredit: number;
    memberUsedCredit: number;
    minLimitAmt: number;
    maxLimitAmt: number;
    withdrawalLimitCnt: number;
    paymentFrom: string;
    paymentTo: string;
    amount: number;
    paySlipBase64: string;
    ip: string;
    remark: string;
    providerId: number;
}


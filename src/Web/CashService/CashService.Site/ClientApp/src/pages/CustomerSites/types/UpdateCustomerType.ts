export interface MemberBankAccountRecord {
    agentBankAccountID: number;
    agentBankAccountNumber: string;
    agentBankAccountStatus: string;
    agentBankCode: string;
    agentBankID: number;
    memberBankAccountID: number;
    memberBankAccountNumber: string;
    memberBankCode: string;
    memberBankID: number;
}

export interface CustomerInfoData {
    firstName: string;
    lastName: string;
    mobilePhone: string;
    mobileCode: string;
    mobileNumber: string;
    phone: string;
    email: string;
    fax: string;
}

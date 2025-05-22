import { DepositMatchingMode } from './DepositMatchingMode';

// Bank Account interface
export interface BankAccount {
  id: number;
  accountNumber: string;
  accountNickname: string;
  accountHolderName: string;
  transactionTypes: string;
  status: number;
  code: string;
  bankId: number;
  bankName: string;
  lastUpdated: string;
  lastUpdatedStr: string;
  depositMatchingMode: number;
  promptPay: boolean;
  mobile: string;
  countryCode: string;

  bank_TransTypeList: string;
  agent_TransTypeList: string;
  agent_DepositMatchingMode: number;
  bankDepositMatchingMode: DepositMatchingMode;
  creator: string;
}

export const createDefaultBankAccount = (): BankAccount => {
  return {
    id: 0,
    accountNumber: '',
    accountNickname: '',
    accountHolderName: '',
    transactionTypes: '',
    status: 0,
    code: '',
    bankId: 0,
    bankName: '',
    lastUpdated: '',
    lastUpdatedStr: '',
    depositMatchingMode: 0,
    promptPay: false,
    mobile: '',
    countryCode: '',
    bank_TransTypeList: '',
    agent_TransTypeList: '',
    agent_DepositMatchingMode: 0,
    bankDepositMatchingMode: {
      sourceType: '',
      mappingTypes: [],
      sourceTypeName: ''
    },
    creator: ''
  };
}; 
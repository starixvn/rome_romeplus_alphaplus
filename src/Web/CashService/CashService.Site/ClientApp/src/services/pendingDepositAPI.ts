import axios from 'axios';
import { DepositAccountInfo } from './interfaces/DepositAccountInfo';
import { CreditAmount } from './interfaces/Transactions/CreditAmount';
import { ResultModel } from './interfaces/ResultModel';
import { CreateManualModel } from './interfaces/Transactions/CreateManualModel';

const api = axios.create({
  baseURL: "",
  headers: {
    'Content-Type': 'application/json'
  },
  withCredentials: true
});

export const getMemberBankAccounts = async (): Promise<DepositAccountInfo[]> => {
  try {
    const response = await api.get<DepositAccountInfo[]>('GetMemberBankAccounts');
    return response.data;
  } catch (error) {
    console.error('Error fetching member bank accounts:', error);
    return [];
  }
};

export const getAccumulateCredit = async (): Promise<number> => {
  try {
    const response = await api.get<number>('GetAccumulateCredit');
    return response.data;
  } catch (error) {
    console.error('Error fetching accumulate credit:', error);
    return 0;
  }
};

export const getLimitAmtDeposit = async (accumCreditReturn: number): Promise<CreditAmount> => {
  try {
    const response = await api.get<CreditAmount>('GetLimitAmtDeposit', { params: { accumCreditReturn } });
    return response.data;
  } catch (error) {
    console.error('Error fetching credit amount:', error);
    return { minAmt: 0, maxAmt: 0 };
  }
};

export const createDepositManual = async (createManual: CreateManualModel): Promise<ResultModel> => {
  try {
    const response = await api.post<ResultModel>('CreateManual', createManual);
    return response.data;
  } catch (error) {
    console.error('Error creating manual:', error);
    throw error;
  }
};

export default api; 
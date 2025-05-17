import { api } from '../config/axios';
import { DepositAccountInfo } from './interfaces/DepositAccountInfo';
import { CreditAmount } from './interfaces/Transactions/CreditAmount';
import { ResultModel } from './interfaces/ResultModel';
import { AddDepositManualModel } from './interfaces/AddDepositManualModel';

export const pendingDepositAPI = {
  getMemberBankAccounts: async (): Promise<DepositAccountInfo[]> => {
    try {
      const response = await api.get<DepositAccountInfo[]>('GetMemberBankAccounts');
      return response.data;
    } catch (error) {
      console.error('Error fetching member bank accounts:', error);
      return [];
    }
  },

  getLimitAmtDeposit: async (custId: number): Promise<CreditAmount> => {
    try {
      const response = await api.get<CreditAmount>(`GetLimitAmtDeposit?custId=${custId}`);
      return response.data;
    } catch (error) {
      console.error('Error fetching credit amount:', error);
      return { minAmt: 0, maxAmt: 0 };
    }
  },

  createDepositManual: async (manualDepositAdd: AddDepositManualModel): Promise<ResultModel> => {
    try {
      const response = await api.post<ResultModel>('AddManualDeposit', manualDepositAdd);
      return response.data;
    } catch (error) {
      console.error('Error creating manual:', error);
      return { code: 1, isSuccessful: false, message: 'Error add manual deposit' };
    }
  }
};

export default pendingDepositAPI;
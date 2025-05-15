import { api } from '../config/axios';
import { DepositAccountInfo } from './interfaces/DepositAccountInfo';
import { CreditAmount } from './interfaces/Transactions/CreditAmount';
import { ResultModel } from './interfaces/ResultModel';
import { CreateManualModel } from './interfaces/Transactions/CreateManualModel';

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

  createDepositManual: async (createManual: CreateManualModel): Promise<ResultModel> => {
    try {
      const response = await api.post<ResultModel>('CreateManual', createManual);
      return response.data;
    } catch (error) {
      console.error('Error creating manual:', error);
      throw error;
    }
  }
};

export default pendingDepositAPI; 
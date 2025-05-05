import axios from 'axios';
import { WithdrawalInfoDetail } from './interfaces/WithdrawalInfoDetail';
import { WithdrawalAccountInfo } from './interfaces/WithdrawalAccountInfo';
import { WithdrawalMemberLoginName } from './interfaces/WithdrawalMemberLoginName';
import { ResultModel } from './interfaces/ResultModel';


const api = axios.create({
  baseURL: "",
  headers: {
    'Content-Type': 'application/json'
  },
  withCredentials: true
});

export const pendingWithdrawalService = {
  getMemberWithdrawLoginNames: async (): Promise<WithdrawalMemberLoginName[]> => {
    try {
      const response = await api.get<WithdrawalMemberLoginName[]>('GetMemberWithdrawLoginNames');
      return response.data;
    } catch (error) {
      console.error('Error fetching member login names:', error);
      return [];
    }
  },
  
  getBankWithdrawalAccounts: async (): Promise<WithdrawalAccountInfo[]> => {
    try {
      const response = await api.get<WithdrawalAccountInfo[]>('GetBankWithdrawalAccounts');
      return response.data;
    } catch (error) {
      console.error('Error fetching member bank accounts:', error);
      return [];
    }
  },
  
  insertWithdrawAccountInfo: async (accountInfo: WithdrawalInfoDetail): Promise<ResultModel> => {
    try {
      const response = await api.post<ResultModel>('InsertWithdrawAccountInfo', accountInfo);
      return response.data;
    } catch (error) {
      console.error('Error inserting account information:', error);
      throw error;
    }
  }
};

export default api;
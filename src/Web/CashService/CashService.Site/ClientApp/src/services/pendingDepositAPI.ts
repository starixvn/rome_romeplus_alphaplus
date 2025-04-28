import axios from 'axios';
import { DepositInfoDetail } from './interfaces/DepositInfoDetail';
import { DepositAccountInfo } from './interfaces/DepositAccountInfo';
import { DepositMemberLoginName } from './interfaces/DepositMemberLoginName';
import { ResultModel } from './interfaces/ResultModel';

const api = axios.create({
  baseURL: "",
  headers: {
    'Content-Type': 'application/json'
  },
  withCredentials: true
});

export const pendingDepositService = {
  getMemberLoginNames: async (): Promise<DepositMemberLoginName[]> => {
    try {
      const response = await api.get<DepositMemberLoginName[]>('GetMemberLoginNames');
      return response.data;
    } catch (error) {
      console.error('Error fetching member login names:', error);
      return [];
    }
  },
  
  getMemberBankAccounts: async (): Promise<DepositAccountInfo[]> => {
    try {
      const response = await api.get<DepositAccountInfo[]>('GetMemberBankAccounts');
      return response.data;
    } catch (error) {
      console.error('Error fetching member bank accounts:', error);
      return [];
    }
  },
  
  insertAccountInfo: async (accountInfo: DepositInfoDetail): Promise<ResultModel> => {
    try {
      const response = await api.post<ResultModel>('InsertAccountInfo', accountInfo);
      return response.data;
    } catch (error) {
      console.error('Error inserting account information:', error);
      throw error;
    }
  }
};

export default api; 
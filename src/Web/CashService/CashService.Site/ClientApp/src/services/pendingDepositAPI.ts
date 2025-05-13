import axios from 'axios';
import { DepositAccountInfo } from './interfaces/DepositAccountInfo';

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

export default api; 
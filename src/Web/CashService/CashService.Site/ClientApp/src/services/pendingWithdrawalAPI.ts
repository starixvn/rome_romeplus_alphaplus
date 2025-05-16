import axios from 'axios';
import { WithdrawalAccountInfo } from './interfaces/WithdrawalAccountInfo';
import { CreditAmount } from './interfaces/Transactions/CreditAmount';
import { CashLimitAmt } from './interfaces/Transactions/CashLimitAmt';
import { ResultModel } from './interfaces/ResultModel';
import { AddWithdrawalModel } from './interfaces/AddWithdrawalModel';

const api = axios.create({
  baseURL: '',
  headers: {
    'Content-Type': 'application/json',
  },
  withCredentials: true,
});

export const getBankWithdrawalAccounts = async (): Promise<WithdrawalAccountInfo[]> => {
  try {
    const response = await api.get<WithdrawalAccountInfo[]>('GetBankWithdrawalAccounts');
    return response.data;
  } catch (error) {
    console.error('Error fetching member bank accounts:', error);
    return [];
  }
};

export const getCashBalance = async (): Promise<number> => {
  try {
    const response = await api.get<number>('GetCashBalance');
    return response.data;
  } catch (error) {
    console.error('Error fetching cash balance:', error);
    return 0;
  }
};

export const getSettingLimitAmtWithdrawal = async (
  cashLimitAmt: CashLimitAmt
): Promise<CreditAmount> => {
  try {
    const response = await api.get<CreditAmount>('GetSettingLimitAmtWithdrawal', {
      params: cashLimitAmt,
    });
    return response.data;
  } catch (error) {
    console.error('Error fetching credit amount:', error);
    return { minAmt: 0, maxAmt: 0 };
  }
};

export const manualAddWithdrawal = async (
  manualAddWithdrawal: AddWithdrawalModel
): Promise<ResultModel> => {
  try {
    const response = await api.post<ResultModel>('AddManualWithdrawal', manualAddWithdrawal);
    return response.data;
  } catch (error) {
    console.error('Error fetching credit amount:', error);
    return { code: 1, isSuccessful: false, message: 'Error add manual withdrawal' };
  }
};

export default api;

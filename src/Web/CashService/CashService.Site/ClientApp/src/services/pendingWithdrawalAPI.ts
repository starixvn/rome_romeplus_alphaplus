import axios from 'axios';
import { WithdrawalAccountInfo } from './interfaces/WithdrawalAccountInfo';
import { CreditAmount } from './interfaces/Transactions/CreditAmount';
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

export const getSettingLimitAmtWithdrawal = async ({
  custId,
  providerId,
  paymentMethodId,
}: {
  custId: number;
  providerId: number;
  paymentMethodId: number;
}): Promise<CreditAmount> => {
  try {
    const response = await api.get<CreditAmount>('GetSettingLimitAmtWithdrawal', {
      params: {
        custId,
        providerId,
        paymentMethodId,
      },
    });
    return response.data;
  } catch (error) {
    console.error('Error fetching credit amount:', error);
    return { minAmt: 0, maxAmt: null };
  }
};

export const getWithdrawalMemberCashBalance = async (custId: number): Promise<number> => {
  try {
    const response = await api.get<number>('GetWithdrawalMemberCashBalance', {
      params: { custId },
    });
    return response.data;
  } catch (error) {
    console.error('Error fetching withdrawl member cash balance:', error);
    return 0;
  }
};

export const manualAddWithdrawal = async (
  manualAddWithdrawal: AddWithdrawalModel
): Promise<ResultModel> => {
  try {
    const formData = new FormData();
    formData.append('custId', manualAddWithdrawal.custId.toString());
    formData.append('paymentFrom', manualAddWithdrawal.paymentFrom);
    formData.append('paymentTo', manualAddWithdrawal.paymentTo);
    formData.append('amount', manualAddWithdrawal.amount.toString());
    formData.append('remark', manualAddWithdrawal.remark);
    formData.append('providerId', manualAddWithdrawal.providerId.toString());

    if (manualAddWithdrawal.image) {
      formData.append('image', manualAddWithdrawal.image);
    }

    const response = await api.post<ResultModel>('AddManualWithdrawal', formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });
    return response.data;
  } catch (error) {
    console.error('Error fetching credit amount:', error);
    return { code: 1, isSuccessful: false, message: 'Error add manual withdrawal' };
  }
};

export const approveWithdrawal = async ({
  transId,
  paymentFrom,
  remark,
  image,
}: {
  transId: number;
  paymentFrom: string;
  remark: string;
  image: File | null;
}): Promise<ResultModel> => {
  try {
    const formData = new FormData();
    formData.append('transactionId', transId.toString());
    formData.append('paymentFrom', paymentFrom);
    formData.append('remark', remark);
    if (image) {
      formData.append('image', image);
    }

    const response = await api.post<ResultModel>('ApproveWithdrawal', formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });
    return response.data;
  } catch (error) {
    console.error('Error approving withdrawal:', error);
    return { code: 1, isSuccessful: false, message: 'Error approving withdrawal' };
  }
};

export const rejectWithdrawal = async ({
  transId,
  paymentFrom,
  remark,
  image,
}: {
  transId: number;
  paymentFrom: string;
  remark: string;
  image: File | null;
}): Promise<ResultModel> => {
  try {
    const formData = new FormData();
    formData.append('transactionId', transId.toString());
    formData.append('paymentFrom', paymentFrom);
    formData.append('remark', remark);
    if (image) {
      formData.append('image', image);
    }

    const response = await api.post<ResultModel>('RejectWithdrawal', formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });
    return response.data;
  } catch (error) {
    console.error('Error rejecting withdrawal:', error);
    return { code: 1, isSuccessful: false, message: 'Error rejecting withdrawal' };
  }
};

export default api;

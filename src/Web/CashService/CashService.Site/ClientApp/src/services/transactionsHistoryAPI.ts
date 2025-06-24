import { axiosInstance as api } from './axios';
import {
  TransactionItem,
  TransactionsHistoryResponse,
  TransactionsHistoryParams,
  ExportExcelParams,
} from './interfaces/Transactions/TransactionHistory';
import { AccountOption } from './interfaces/Transactions/AccountOption';
import { ResultModel } from './interfaces/ResultModel';
import { BankInfo, ProviderInfo } from './interfaces/Transactions/BankInfo';
import { MemberLoginName } from './interfaces/Transactions/MemberLoginName';

export const transactionAPI = {
  getTransactions: async (
    params: TransactionsHistoryParams
  ): Promise<TransactionsHistoryResponse> => {
    try {
      const queryParams = new URLSearchParams({
        pageIndex: params.pageIndex.toString(),
        pageSize: params.pageSize.toString(),
        paymentStatus: params.status.toString(),
        searchText: params.searchText,
        account: params.account,
        fromDate: params.dateRange[0],
        toDate: params.dateRange[1],
      });

      if (params.transactionType) {
        queryParams.append('transactionType', params.transactionType.toString());
      }

      const response = await api.get<TransactionsHistoryResponse>(
        `GetHistory?${queryParams.toString()}`
      );
      response.data.items.forEach((item) => {
        if (
          item.paymentFrom === null ||
          item.paymentFrom === undefined ||
          item.paymentFrom === ''
        ) {
          item.paymentFrom = '-';
        }
        if (item.paymentTo === null || item.paymentTo === undefined || item.paymentTo === '') {
          item.paymentTo = '-';
        }
      });
      return response.data;
    } catch (error) {
      console.error('Error fetching transactions:', error);
      return {
        items: [],
        totalCount: 0,
      };
    }
  },

  getTransactionDetail: async (transactionId: number, custId: number): Promise<any> => {
    try {
      const response = await api.get<TransactionItem>(
        `Detail?transactionId=${transactionId}&custId=${custId}`
      );
      return response;
    } catch (error) {
      console.error(
        `Error fetching transaction detail for ID ${transactionId}:, custId: ${custId}`,
        error
      );
      throw error;
    }
  },

  revertTransaction: async (
    transactionId: number,
    remarks: string,
    isDeposit: boolean
  ): Promise<ResultModel> => {
    const url = isDeposit
      ? 'RevertDepositTransactionToPending'
      : 'RevertWithdrawalTransactionToPending';
    try {
      const response = await api.post<ResultModel>(url, {
        transId: transactionId,
        remark: remarks,
      });
      return response.data;
    } catch (error) {
      console.error(`Error reverting transaction for ID ${transactionId}:`, error);
      throw error;
    }
  },

  getAccount: async (transactionType: number): Promise<AccountOption[]> => {
    try {
      const response = await api.get<AccountOption[]>(
        `GetFilterAccount?transactionType=${transactionType}`
      );
      return response.data;
    } catch (error) {
      console.error('Error fetching accounts:', error);
      return [];
    }
  },

  getCashMemberBankInfo: async (custId: number): Promise<BankInfo[]> => {
    try {
      const response = await api.get<BankInfo[]>(`GetCashMemberBankInfo?custId=${custId}`);
      return response.data;
    } catch (error) {
      console.error('Error fetching cash member bank info:', error);
      return [];
    }
  },

  getCashAgentProvider: async (transactionType: number): Promise<ProviderInfo[]> => {
    try {
      const response = await api.get<ProviderInfo[]>(
        `GetCashAgentProvider?transactionType=${transactionType}`
      );
      return response.data;
    } catch (error) {
      console.error('Error fetching cash agent provider info:', error);
      return [];
    }
  },

  getMemberLoginName: async (): Promise<MemberLoginName[]> => {
    try {
      const response = await api.get<MemberLoginName[]>('GetMemberLoginName', { method: 'GET' });
      return response.data;
    } catch (error) {
      console.error('Error fetching member login names:', error);
      return [];
    }
  },

  getCashAgentBankInfo: async (transactionType: number): Promise<BankInfo[]> => {
    try {
      const response = await api.get<BankInfo[]>(
        `GetCashAgentBankInfo?transactionType=${transactionType}`,
        { method: 'GET' }
      );
      return response.data;
    } catch (error) {
      console.error('Error fetching cash agent bank info:', error);
      return [];
    }
  },

  exportExcel: async (params: ExportExcelParams): Promise<void> => {
    try {
      var formData = new FormData();
      formData.append('transactionType', params.transactionType.toString());
      formData.append('fromDate', params.fromDate);
      formData.append('toDate', params.toDate);
      formData.append('status', params.status.toString());
      formData.append('account', params.account);

      await api
        .post('ExportExcel', formData, {
          responseType: 'blob',
        })
        .then((response) => {
          const fileName = response.headers['content-disposition']
            .split('filename=')[1]
            .split(';')[0]
            .replace(/"/g, '');

          const url = window.URL.createObjectURL(new Blob([response.data]));
          const link = document.createElement('a');
          link.href = url;
          link.setAttribute('download', fileName);
          document.body.appendChild(link);
          link.click();
          link.parentNode?.removeChild(link);
          window.URL.revokeObjectURL(url);
        });
    } catch (error) {
      console.error('Error exporting excel:', error);
      throw error;
    }
  },
};

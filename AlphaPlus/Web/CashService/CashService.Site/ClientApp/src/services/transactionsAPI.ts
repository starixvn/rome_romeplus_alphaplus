import { api } from '../config/axios';
import { AxiosError } from 'axios';

interface ErrorResponse {
  message?: string;
  [key: string]: any;
}

export interface TransactionModel {
  id: number;
  referenceNumber: string;
  username: string;
  amount: number;
  status: string;
  createdAt: string;
  description: string;
}

export interface TransactionResponse {
  items: TransactionModel[];
  totalCount: number;
  page: number;
  pageSize: number;
}

export interface GetTransactionsParams {
  username?: string;
  referenceNumber?: string;
  fromDate?: string;
  toDate?: string;
  page?: number;
  pageSize?: number;
}

class TransactionsService {
  private readonly BASE_PATH = 'cash-service/Transactions/GetTransactionsAsync';

  private handleError(error: AxiosError<ErrorResponse>): never {
    let message = 'An error occurred';

    if (error.response) {
      // Server responded with error
      message = error.response.data?.message || `Error: ${error.response.status}`;
    } else if (error.request) {
      // Request made but no response
      message = 'No response from server';
    } else {
      // Request setup error
      message = error.message;
    }

    console.error('API Error:', message);
    throw new Error(message);
  }

  async getTransactions(params?: GetTransactionsParams): Promise<TransactionResponse> {
    try {
      const { data } = await api.get<TransactionResponse>(this.BASE_PATH, { params });
      console.log(data);
      return data;
    } catch (error) {
      if (error instanceof AxiosError) {
        this.handleError(error);
      }
      throw error;
    }
  }

  async getTransactionById(id: number): Promise<TransactionModel> {
    try {
      const { data } = await api.get<TransactionModel>(`${this.BASE_PATH}/${id}`);
      return data;
    } catch (error) {
      if (error instanceof AxiosError) {
        this.handleError(error);
      }
      throw error;
    }
  }
}

export const transactionsService = new TransactionsService();

export default api;

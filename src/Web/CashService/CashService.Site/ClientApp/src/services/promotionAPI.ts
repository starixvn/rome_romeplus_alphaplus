import { api, axiosInstance, createHeaderXMLHttp } from './axios';
mport {
  EditPromotionRequest,
  ProductDropdown,
  PromotionDetail,
  PromotionParams,
  PromotionRequest,
  PromotionTransactionRequest,
  PromotionTransactionResponse,
} from './interfaces/Promotion';
import { ResultModel } from './interfaces/ResultModel';

export const promotionService = {
  getLanguages: async (): Promise<Record<string, string>> => {
    const response = await axiosInstance.get<Record<string, string>>(`GetLanguages`);
    return response.data;
  },
  getPromotion: async (promotionParams: PromotionParams): Promise<PromotionDetail[]> => {
    try {
      const queryParams = new URLSearchParams({
        promotionType: promotionParams.type.toString(),
        status: promotionParams.status.toString(),
      });
      const response = await axiosInstance.get<PromotionDetail[]>(
        `GetPromotion?${queryParams.toString()}`
      );
      return response.data;
    } catch (error) {
      console.error('Error fetching promotion:', error);
      return [];
    }
  },

  getPendingPromotionTransactions: async (
    request: PromotionTransactionRequest
  ): Promise<PromotionTransactionResponse> => {
    try {
      const queryParams = new URLSearchParams({
        promotionTypeId: request.promotionTypeId.toString(),
        fromDate: request.fromDate,
        toDate: request.toDate,
      });
      const response = await axiosInstance.get<PromotionTransactionResponse>(
        `GetPendingPromotionTransaction?${queryParams.toString()}`
      );
      return response.data;
    } catch (error) {
      console.error('Error fetching pending promotion:', error);
      return {
        promotionTransactions: [],
        totalCount: 0,
      };
    }
  },

  getPromotionById: async (promotionId: number): Promise<PromotionDetail> => {
    const response = await axiosInstance.get<PromotionDetail>(
      `GetPromotionById?promotionId=${promotionId}`
    );
    return response.data;
  },

  getPromotionProducts: async (): Promise<ProductDropdown[]> => {
    try {
      const response = await axiosInstance.get<ProductDropdown[]>(`GetPromotionProducts`);
      return response.data;
    } catch (error) {
      console.error('Error fetching promotion products:', error);
      return [];
    }
  },
  addPromotion: async (promotionRequest: PromotionRequest): Promise<ResultModel> => {
    try {
      const response = await axiosInstance.post<ResultModel>(`AddPromotion`, promotionRequest, {
        headers: {
          ...createHeaderXMLHttp(),
          'Content-Type': 'application/json',
        },
      });
      return response.data;
    } catch (error) {
      console.error('Error adding promotion:', error);
      throw error;
    }
  },
  editPromotion: async (promotionId: number, promotionRequest: EditPromotionRequest): Promise<ResultModel> => {
    try {
      const response = await axiosInstance.put<ResultModel>(`EditPromotion/${promotionId}`, promotionRequest, {
        headers: {
          ...createHeaderXMLHttp(),
          'Content-Type': 'application/json',
        },
      });
      return response.data;
    } catch (error) {
      console.error('Error adding promotion:', error);
      throw error;
    }
  },
  deletePromotion: async (promotionId: number): Promise<ResultModel> => {
    try {
      const response = await axiosInstance.delete<ResultModel>(
        `DeletePromotion?promotionId=${promotionId}`
      );
      return response.data;
    } catch (error) {
      console.error('Error deleting promotion:', error);
      throw error;
    }
  },

  getMainReport: async (filterParams: {
    fromDate: string;
    toDate: string;
    promotionType: string;
    status: string;
  }): Promise<any> => {
    const url = `GetMainReport?${new URLSearchParams(filterParams).toString()}`;
    const { data, error } = await api.get<object[]>(url);
    if (error) {
      console.error(`Error get promotion report with url=${url}`, error);
      return [];
    }

    return data;
  },
  getDetailsReport: async (filterParams: {
    promotionId: string;
    fromDate: string;
    toDate: string;
    status: string;
  }): Promise<any> => {
    const url = `GetDetailsReport?${new URLSearchParams(filterParams).toString()}`;
    const { data, error } = await api.get<object>(url);
    if (error) {
      console.error(`Error get promotion details report with url=${url}:`, error);
      return {};
    }

    return data;
  },
};

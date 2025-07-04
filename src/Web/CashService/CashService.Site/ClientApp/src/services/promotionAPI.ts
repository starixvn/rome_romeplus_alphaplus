import { axiosInstance, api } from './axios';
import { Product, PromotionDetail, PromotionParams } from './interfaces/Promotion';

export const promotionService = {
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

  getPromotionById: async (promotionId: number): Promise<PromotionDetail> => {
    const response = await axiosInstance.get<PromotionDetail>(
      `GetPromotionById?promotionId=${promotionId}`
    );
    return response.data;
  },

  getPromotionProducts: async (): Promise<Product[]> => {
    try {
      const response = await axiosInstance.get<Product[]>(`GetPromotionProducts`);
      return response.data;
    } catch (error) {
      console.error('Error fetching promotion products:', error);
      return [];
    }
  },

  deletePromotion: async (promotionId: number): Promise<void> => {
    try {
      await axiosInstance.delete(`DeletePromotion/${promotionId}`);
    } catch (error) {
      console.error('Error deleting promotion:', error);
      throw error;
    }
  },
  getMainReport: async (
    fromDate: string,
    toDate: string,
    promotionType: string,
    status: string
  ): Promise<any> => {
    const params = new URLSearchParams({
      fromDate: fromDate,
      toDate: toDate,
      promotionType: promotionType,
      status: status,
    });
    const url = `GetMainReport?${params.toString()}`;
    const { data, error } = await api.get<object>(url);
    if (error) {
      console.error(`Error get promotion report with url=${url}`, error);
      return [];
    }

    return data;
  },
  getDetailsReport: async (
    promotionId: string,
    fromDate: string,
    toDate: string,
    status: string
  ): Promise<any> => {
    const params = new URLSearchParams({
      promotionId: promotionId,
      fromDate: fromDate,
      toDate: toDate,
      status: status,
    });
    const url = `GetDetailsReport?${params.toString()}`;
    const { data, error } = await api.get<object[]>(url);
    if (error) {
      console.error(`Error get promotion details report with url=${url}:`, error);
      return {};
    }

    return data;
  },
};

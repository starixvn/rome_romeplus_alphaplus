import { axiosInstance, api } from './axios';
import { PromotionDetail, PromotionParams } from './interfaces/Promotion';

export const promotionService = {
  getPromotion: async (promotionParams: PromotionParams): Promise<PromotionDetail[]> => {
    try {
      const queryParams = new URLSearchParams({
        promotionType: promotionParams.type,
        status: promotionParams.status,
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
  deletePromotion: async (promotionId: number): Promise<void> => {
    try {
      await axiosInstance.delete(`DeletePromotion/${promotionId}`);
    } catch (error) {
      console.error('Error deleting promotion:', error);
      throw error;
    }
  },
  getMainReport: async (): Promise<any> => await api.get<object>('GetMainReport'),
  getDetailsReport: async (): Promise<any> => await api.get<object[]>('GetDetailsReport'),
};

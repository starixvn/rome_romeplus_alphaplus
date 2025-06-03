import { axiosInstance as api } from './axios';
import { Promotion, PromotionParams } from './interfaces/Promotion';


export const promotionService = {
    getPromotion: async (promotionParams: PromotionParams): Promise<Promotion[]> => {
        try {
            const queryParams = new URLSearchParams(
            {
                promotionType: promotionParams.type,
                status: promotionParams.status
            });
            const response = await api.get<Promotion[]>(`GetPromotion?${queryParams.toString()}`);
      return response.data;
    } catch (error) {
      console.error('Error fetching promotion:', error);
            return [];
    }
  },
  deletePromotion: async (promotionId: number): Promise<void> => {
    try {
      await api.delete(`DeletePromotion/${promotionId}`);
    } catch (error) {
      console.error('Error deleting promotion:', error);
      throw error;
    }
  }
};

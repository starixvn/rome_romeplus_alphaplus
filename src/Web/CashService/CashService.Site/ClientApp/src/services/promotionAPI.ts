import { api } from '../config/axios';
import { Promotion, PromotionParams } from './interfaces/Promotion';


export const promotionService = {
    getPromotion: async (promotionParams: PromotionParams): Promise<Promotion[]> => {
        try {
            const queryParams = new URLSearchParams(
            {
                promotionType: promotionParams.Type,
                status: promotionParams.Status
            });
            const response = await api.get<Promotion[]>(`GetPromotion?${queryParams.toString()}`);
      return response.data;
    } catch (error) {
      console.error('Error fetching promotion:', error);
            return [];
    }
  }
};

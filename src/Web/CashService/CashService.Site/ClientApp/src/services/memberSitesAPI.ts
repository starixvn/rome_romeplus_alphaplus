import { api } from '../config/axios';
import { AxiosError } from 'axios';
import { MemberSiteResponse, ResponseResult } from '../types/memberSite';
// Declare the cashServiceAPIUrl property on the Window interface
declare global {
  interface Window {
    cashServiceAPIUrl: string;
  }
}

interface ErrorResponse {
  message?: string;
  [key: string]: any;
}

class MemberSitesService {
  private readonly BASE_PATH = 'MemberSiteManagement';

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

  async getMemberSiteData(): Promise<MemberSiteResponse> {
    try {
      const { data } = await api.get<MemberSiteResponse>(this.BASE_PATH);
      return data;
    } catch (error) {
      if (error instanceof AxiosError) {
        // Return default data for specific error cases
        if (error.response?.status === 404) {
          return this.getDefaultMemberSiteData();
        }
        this.handleError(error);
      }
      throw error;
    }
  }

  async updateMemberSiteData(data: Partial<MemberSiteResponse>): Promise<ResponseResult> {
    try {
      const { data: responseData } = await api.put<ResponseResult>(
        this.BASE_PATH,
        data
      );
      return responseData;
    } catch (error) {
      if (error instanceof AxiosError) {
        this.handleError(error);
      }
      throw error;
    }
  }

  private getDefaultMemberSiteData(): MemberSiteResponse {
    const defualtMemberSiteData: MemberSiteResponse = {
      domains: [],
      generalSettings: [],
      countryCode: '',
      mobile: '',
      lineId: '',
      announcement: '',
    };
    return defualtMemberSiteData;
  }
}

export const memberSitesService = new MemberSitesService();

export default api; 
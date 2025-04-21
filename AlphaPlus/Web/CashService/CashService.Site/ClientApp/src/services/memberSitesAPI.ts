import { api } from '../config/axios';
import { AxiosError } from 'axios';

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

export interface Domain {
  url: string;
  qrCode: string;
}

export interface MemberSiteData {
  domains: Domain[];
  allowRegistration: boolean;
  allowDeposit: boolean;
  mobile: string;
  lineId: string;
  announcement: string;
}

class MemberSitesService {
  private readonly BASE_PATH = '/api/member-site-info';

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

  async getMemberSiteData(): Promise<MemberSiteData> {
    try {
      const { data } = await api.get<MemberSiteData>(this.BASE_PATH);
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

  async updateMemberSiteData(data: MemberSiteData): Promise<MemberSiteData> {
    try {
      const { data: responseData } = await api.put<MemberSiteData>(
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

  private getDefaultMemberSiteData(): MemberSiteData {
    return {
      domains: [],
      allowRegistration: false,
      allowDeposit: false,
      mobile: '',
      lineId: '',
      announcement: ''
    };
  }
}

export const memberSitesService = new MemberSitesService();

export default api; 
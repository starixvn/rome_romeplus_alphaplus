import { axiosInstance } from '../../../services/axios';
import { AxiosError } from 'axios';
// Declare the cashServiceAPIUrl property on the Window interface
declare global {
  interface Window {
    cashServiceAPIUrl: string;
  }
}


class CustomerInfoServices {
  private readonly CustomerInfoAPI = 'XXXX';
  private readonly BankInfoAPI = 'XXXX';

  async getCustomerInfo(): Promise<any> {
    try {
      const { data } = await axiosInstance.get<any>(this.CustomerInfoAPI);
      console.log("getCustomerInfo", data);
      return data;
    } catch (error) {
      if (error instanceof AxiosError && error.response?.status === 404) {
        return  {

        }
      }
      throw error;
    }
  }

  async getCustomerBanksInfo(): Promise<any> {
    try {
      const { data } = await axiosInstance.get<any>(this.BankInfoAPI);
      console.log("getCustomerBanksInfo", data);
      return {
      
      };
    } catch (error) {
      if (error instanceof AxiosError && error.response?.status === 404) {
        return  {

        }
      }
      throw error;
    }
  }
}

export const customerInfoServices = new CustomerInfoServices();

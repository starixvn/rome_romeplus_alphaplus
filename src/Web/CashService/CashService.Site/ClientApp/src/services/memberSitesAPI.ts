import { axiosInstance } from './axios';
import { AxiosError } from 'axios';
import { MemberSiteManagementData, MemberSiteResponse, ResponseResult } from '../services/interfaces/MemberSites';
// Declare the cashServiceAPIUrl property on the Window interface
declare global {
  interface Window {
    cashServiceAPIUrl: string;
  }
}

interface Contact {
  type: string;
  value: string;
  isRequired: boolean;
}

class MemberSitesService {
  private readonly BASE_PATH = 'MemberSiteManagement';

  private parseContacts(contacts: string): Contact[] {
    try {
      const jsonContacts = JSON.parse(contacts);
      if (Array.isArray(jsonContacts) && jsonContacts.length > 0) {
        return jsonContacts.map(contact => ({
          type: contact.type,
          value: contact.value,
          isRequired: contact.type === 'line' ? true : contact.isRequired
        }));
      }
    } catch (error) {
      console.error('Error parsing contacts:', error);
    }
    return [];
  }

  private parseMobilePhone(mobile: string | undefined, countryCodes: string[]): { code: string; phone: string } {
    if (!mobile) return { code: countryCodes[0] || '', phone: '' };
    
    const [code, phone] = mobile.split('-');
    return {
      code: code || countryCodes[0] || '',
      phone: phone || ''
    };
  }

  private getDefaultMemberSiteData(): MemberSiteManagementData {
    return {
      domains: [],
      generalSettings: [],
      announcement: '',
      mobilePhone: {
        code: '',
        phone: ''
      },
      contacts: [],
      countryCodes: []
    };
  }

  async getMemberSiteData(): Promise<MemberSiteManagementData> {
    try {
      const { data } = await axiosInstance.get<MemberSiteResponse>(this.BASE_PATH);
      
      const mobilePhone = this.parseMobilePhone(data.mobile, data.countryCodes);
      const contacts = typeof data.contacts === 'string' 
        ? this.parseContacts(data.contacts)
        : [];

      return {
        domains: data.domains,
        generalSettings: data.generalSettings,
        mobilePhone,
        contacts,
        announcement: data.announcement,
        countryCodes: data.countryCodes
      };
    } catch (error) {
      if (error instanceof AxiosError && error.response?.status === 404) {
        return this.getDefaultMemberSiteData();
      }
      throw error;
    }
  }

  async updateMemberSiteData(data: Partial<MemberSiteResponse>): Promise<ResponseResult> {
    try {
      const { data: responseData } = await axiosInstance.put<ResponseResult>(
        this.BASE_PATH,
        data
      );
      return responseData;
    } catch (error) {
      console.error('Error updating member site data:', error);
      throw error;
    }
  }
}

export const memberSitesService = new MemberSitesService();

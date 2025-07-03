import { axiosInstance, createHeaderXMLHttp } from '../../../services/axios';

const GetCustomerInfo = async <T>(params?: Record<string, any>): Promise<T> => {
  const { data } = await axiosInstance.get<T>(`${updateInfoCustomerPageConfig.settingSiteUrl}Customer/GetCustomerInfo`, {
    params
  });
  return data;
}
const UpdateCustomerInfo = async <T>(params?: Record<string, any>): Promise<T> => {
  const { data } = await axiosInstance.post<T>(`${updateInfoCustomerPageConfig.settingSiteUrl}Customer/UpdateInfoPost`, params, {
    headers: {
      ...createHeaderXMLHttp(),
      'Content-Type': 'application/json'
    }
  });
  console.log('data', data);
  return data;
}

const GetCustomerBankInfo = async <T>(params?: Record<string, any>): Promise<T> => {
  const { data } = await axiosInstance.get<T>(`${updateInfoCustomerPageConfig.settingSiteUrl}Customer/GetCustomerBankInfo`, {
    params
  });
  return data;
}

const GetBanks = async <T>(params?: Record<string, any>): Promise<T> => {
  const { data } = await axiosInstance.get<T>(`${updateInfoCustomerPageConfig.settingSiteUrl}Customer/GetBanks`, {
    params
  });
  return data;
}

const AddMemberBankInfo = async <T>(params?: Record<string, any>): Promise<T> => {
  const { data } = await axiosInstance.post<T>(`${updateInfoCustomerPageConfig.settingSiteUrl}Customer/AddMemberBankInfo`, params, {
    headers: {
      ...createHeaderXMLHttp(),
      'Content-Type': 'application/json'
    }
  });
  return data;
}

export {
  GetCustomerInfo,
  UpdateCustomerInfo,
  GetCustomerBankInfo,
  GetBanks,
  AddMemberBankInfo
};

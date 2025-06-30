import { axiosInstance, createHeaderXMLHttp } from '../../../services/axios';

const GetCustomerInfo = async <T>(params?: Record<string, any>): Promise<T> =>{
  const { data } = await axiosInstance.get<T>(`${updateInfoCustomerPageConfig.settingSiteUrl}Customer/GetCustomerInfo`, {
    params
  });
  return data;
}
const UpdateCustomerInfo = async <T>(params?: Record<string, any>): Promise<T> =>{
  const { data } = await axiosInstance.post<T>(`${updateInfoCustomerPageConfig.settingSiteUrl}Customer/UpdateInfoPost`, params, {
    headers: {
      ...createHeaderXMLHttp(),
      'Content-Type': 'application/json'
    }
  });
  console.log('data', data);
  return data;
}

export { GetCustomerInfo, UpdateCustomerInfo };

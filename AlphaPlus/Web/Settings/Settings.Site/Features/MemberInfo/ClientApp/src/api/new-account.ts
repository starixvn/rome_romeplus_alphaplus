import axios from 'axios';

import { copyAccountRootUrl, createAccountRootUrl } from '@/constants';
import { SubMessagesResult } from '@/types';
import { GeneralInfo, ProductGroup } from '@/types/new-account';

import apiHelpers from './api-helpers';

type CustomerSettingResponse = {
  generalInfo: GeneralInfo;
  productGroups: ProductGroup[];
};

const getCustomerSetting = async () => axios.get<CustomerSettingResponse>(`${createAccountRootUrl}/GetCustomerInfo`);

const checkExistedCustomer = async (username: string) =>
  axios.post<boolean>(`${copyAccountRootUrl}/CheckExistedCustomer`, { custName: username });

const createAccount = async (uiBehavior: object, generalInfo: GeneralInfo, productGroups: ProductGroup[]) =>
  axios.post<SubMessagesResult>(
    `${createAccountRootUrl}/Index`,
    {
      uiBehavior: JSON.stringify(uiBehavior),
      generalInfo,
      productGroups: JSON.stringify(productGroups),
    },
    {
      headers: apiHelpers.getHeadersForPostRequest(),
    },
  );

export { getCustomerSetting, checkExistedCustomer, createAccount };

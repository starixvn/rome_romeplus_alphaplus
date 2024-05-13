import axios from 'axios';

import Customer from '../models/customer';
import CustomerStatus from '../models/customer-status';
import DownlineListResponse from '../models/DownlineListReponse';
import OnlineUser from '../models/online-user';
import UsernameSuggestion from '../models/username-suggestion';

const axiosInstance = axios.create({
    baseURL: window.top?.alpha.urls['settingsRootPath'],
    timeout: 1000
});

const SettingsService = {
    GetUsernameSuggestion: async (query: string) => {
        return axiosInstance
            .get<UsernameSuggestion[]>(`UserNameSuggestion?isAccountInfo=true&query=${query}`)
            .then((response) => response.data);
    },
    GetCustomerInfo: async (username: string) => {
        return axiosInstance
            .get<Customer>(`AccountInfo/GetAccount?username=${username}`)
            .then((response) => response.data);
    },
    GetDownlineList: async (custId: number, roleId: number, pageIndex = 1, statusId = 1, totalPages = 0) => {
        return axiosInstance
            .get<DownlineListResponse>(`AccountInfo/GetDownlineList`, {
                params: { custId, roleId, pageIndex, statusId, totalPages }
            })
            .then((response) => response.data);
    },
    GetCustomerAllStatus: async (
        custId: number,
        username: string,
        roleId: number,
        uplineId: number,
        uplineName: string
    ) => {
        const params = {
            SearchAccountId: custId,
            SearchAccountName: username,
            SearchAccountRoleId: roleId,
            UplineId: uplineId,
            UplineName: uplineName,
            UplineRoleId: roleId + 1
        };

        return axiosInstance
            .get<CustomerStatus>(`CustomerList/GetCustomerAllStatus`, {
                params
            })
            .then((response) => response.data);
    },
    GetLastTxn: async (username: string) => {
        return axiosInstance
            .get<string>(`AccountInfo/GetLastTxn`, {
                params: { username }
            })
            .then((response) => response.data);
    },
    GetOnlineList: async (custId: number) => {
        return axiosInstance
            .get<OnlineUser[]>('AccountInfo/GetOnlineList', {
                params: { SearchAccountId: custId }
            })
            .then((response) => response.data);
    }
};

export default SettingsService;

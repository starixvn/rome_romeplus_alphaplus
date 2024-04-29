import axios from 'axios';

import Customer from '../models/customer';
import DownlineListResponse from '../models/DownlineListReponse';
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
    }
};

export default SettingsService;

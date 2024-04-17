import axios from 'axios';

import Customer from '../models/customer';
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
        return axiosInstance.get<Customer>(`accounts/${username}`).then((response) => response.data);
    }
};

export default SettingsService;

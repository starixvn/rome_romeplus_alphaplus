import axios from 'axios';

import Customer from '../models/customer';
import CustomerStatus from '../models/customer-status';
import DownlineListResponse from '../models/DownlineListReponse';
import OnlineUser from '../models/online-user';
import Result from '../models/result';
import StatusItem from '../models/status-item';
import UsernameSuggestion from '../models/username-suggestion';

const axiosInstance = axios.create({
    baseURL: window.top?.alpha.urls['accountInfoRootPath'],
    timeout: 1000
});

export const handleError = (err: any) => {
    if (axios.isCancel(err)) {
        return undefined;
    }

    if (err.status === 403) {
        return {
            IsSuccess: false,
            Message: $('#NotAuthorized').val()
        };
    }

    throw err;
};

const settingsRootPath = window.top?.alpha.urls['settingsRootPath'];

const SettingsService = {
    GetUsernameSuggestion: async (query: string) => {
        return axios
            .get<UsernameSuggestion[]>(`${settingsRootPath}UserNameSuggestion?isAccountInfo=true&query=${query}`)
            .then((response) => response.data);
    },
    GetCustomerInfo: async (username: string) => {
        return axiosInstance
            .get<Customer>(`SearchAccount/GetAccount?username=${username}`)
            .then((response) => response.data);
    },
    GetDownlineList: async (custId: number, roleId: number, pageIndex = 1, statusId = 1, totalPages = 0) => {
        return axiosInstance
            .get<DownlineListResponse>(`SearchAccount/GetDownlineList`, {
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

        return axios
            .get<CustomerStatus>(`${settingsRootPath}CustomerList/GetCustomerAllStatus`, {
                params
            })
            .then((response) => response.data);
    },
    GetLastTxn: async (username: string) => {
        return axiosInstance
            .get<string>(`SearchAccount/GetLastTxn`, {
                params: { username }
            })
            .then((response) => response.data);
    },
    GetOnlineList: async (custId: number) => {
        return axiosInstance
            .get<OnlineUser[]>('SearchAccount/GetOnlineList', {
                params: { SearchAccountId: custId }
            })
            .then((response) => response.data);
    },
    UpdateCustomerStatus: async (status: StatusItem) => {
        return axiosInstance
            .post<Result>(`SearchAccount/${status.Url}`, JSON.parse(status.JsonParams), {
                headers: {
                    RequestVerificationToken: $("input[name='__RequestVerificationToken']").val()
                }
            })
            .then((response) => response.data);
    }
};

export default SettingsService;

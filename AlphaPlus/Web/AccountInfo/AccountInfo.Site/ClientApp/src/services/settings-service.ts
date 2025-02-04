import axios from 'axios';

import { GetPictureLibraryResponse } from '../components/ProfilePicture/type';
import CustomerStatus from '../models/customer-status';
import DownlineListResponse from '../models/DownlineListReponse';
import OnlineUser from '../models/online-user';
import Result from '../models/result';
import StatusItem from '../models/status-item';
import UsernameSuggestion from '../models/username-suggestion';
import { axiosInstance } from './axios-helper';

const settingsRootPath = window.top?.alpha.urls['settingsRootPath'];

const SettingsService = {
    GetUsernameSuggestion: async (query: string) => {
        return axios
            .get<UsernameSuggestion[]>(`${settingsRootPath}UserNameSuggestion?isAccountInfo=true&query=${query}`)
            .then((response) => response.data);
    },
    GetCustomerInfo: async (username: string) => {
        const response = await axiosInstance.get(`SearchAccount/GetCustomerInfo?SearchAccountName=${username}`);
        return response.data;
    },
    getTotalDownlines: async (custId: number, roleId: number) => {
        return axiosInstance
            .get('SearchAccount/GetTotalDownlines', { params: { custId, roleId } })
            .then((response) => response.data);
    },
    GetDownlineList: async (custId: number, roleId: number, pageIndex = 1, statusId = 1, totalPages = 0) => {
        return axiosInstance
            .get<DownlineListResponse>(`SearchAccount/GetDownlineList`, {
                params: { custId, roleId, pageIndex, statusId, totalPages }
            })
            .then((response) => response.data);
    },
    GetDirectMembers: async (custId: number, roleId: number, pageIndex = 1, statusId = 1, totalPages = 0) => {
        return axiosInstance
            .get<DownlineListResponse>('SearchAccount/GetDirectMembers', {
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
    },
    GetPictureLibrary: async (currentPage = 1) => {
        return axiosInstance
            .get<GetPictureLibraryResponse>(`ProfilePicture/GetPictureLibrary?currentPage=${currentPage}`)
            .then((response) => response.data);
    },
    UpdateProfilePicture: async (imageId: number, image: string | null) => {
        return axiosInstance
            .post<Result>(
                `ProfilePicture/UpdateActorProfilePicture`,
                { imageId, image },
                {
                    headers: {
                        RequestVerificationToken: $("input[name='__RequestVerificationToken']").val()
                    }
                }
            )
            .then((response) => response.data);
    }
};

export default SettingsService;

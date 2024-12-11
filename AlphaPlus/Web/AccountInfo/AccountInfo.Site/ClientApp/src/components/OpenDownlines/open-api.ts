import Result from '../../models/result';
import { axiosInstance } from '../../services/axios-helper';

export interface OpenSingleModel {
    RoleId: number;
    AffectedDownlines: number;
    UnchangedDownlines: number;
}

export interface OpenSingleDataViewModel {
    SearchAccountId: number;
    SearchAccountName: string;
    Levels: OpenSingleModel[];
    AffectedDownlinesTotal: number;
    UnchangedDownlinesTotal: number;
}

export interface OpenDownlineModel {
    SearchAccountId: number;
    DownlineRoleId: number;
    AffectedDownlines: number;
    UnchangedDownlines: number;
}

export interface OpenMultipleModel {
    SearchAccountId: number;
    Username: string;
    IsClosed: boolean;
    Downlines: OpenDownlineModel[];
}

export interface OpenMultipleResultViewModel {
    RoleId: number;
    Results: Result[];
}

export interface OpenMultipleViewModel {
    SearchAccountRoleId: number;
    Data: OpenMultipleModel[];
}

const OpenAPI = {
    async updateClose<T>(action: string, data: object) {
        const response = await axiosInstance.post<T>(action, data, {
            headers: {
                RequestVerificationToken: $("input[name='__RequestVerificationToken']").val()
            }
        });
        return response.data;
    },
    fetchCustomerList: async (
        custId: number,
        searchAccountName: string,
        pageSize: number,
        pageIndex: number,
        isAffected: boolean,
        downlineRoleId: number
    ) => {
        return axiosInstance
            .get('OpenDownlines/FetchCustomerList', {
                params: { custId, searchAccountName, pageSize, pageIndex, isAffected, downlineRoleId }
            })
            .then((response) => response.data);
    }
};

export default OpenAPI;

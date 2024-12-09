import { axiosInstance } from './axios-helper';

const UnsuspendAPI = {
    async updateSuspend<T>(action: string, data: object) {
        const response = await axiosInstance.post<T>(action, data);
        return response.data;
    },
    fetchCustomerList: async (
        searchAccountId: number,
        searchAccountName: string,
        pageSize: number,
        pageIndex: number,
        isAffected: boolean,
        downlineRoleId: number
    ) => {
        return axiosInstance
            .get('UnsuspendDownlines/FetchCustomerList', {
                params: { searchAccountId, searchAccountName, pageSize, pageIndex, isAffected, downlineRoleId }
            })
            .then((response) => response.data);
    }
};

export default UnsuspendAPI;

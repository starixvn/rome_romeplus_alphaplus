import { axiosInstance } from './axios-helper';

const ProtectedStatusAPI = {
    async editMultiple<T>(data: object) {
        const response = await axiosInstance.post<T>('ProtectedStatus/EditMultiple', data, {
            headers: {
                RequestVerificationToken: $("input[name='__RequestVerificationToken']").val()
            }
        });
        return response.data;
    }
};

export default ProtectedStatusAPI;

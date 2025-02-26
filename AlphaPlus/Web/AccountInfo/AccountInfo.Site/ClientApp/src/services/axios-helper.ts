import axios from 'axios';

export const axiosInstance = axios.create({
    baseURL: window.top?.alpha.urls['accountInfoRootPath']
});

export const handleError = (err: any) => {
    if (axios.isCancel(err)) {
        return undefined;
    }

    if (err.response?.status === 403 || err.Code === 403) {
        return {
            IsSuccess: false,
            Message: $('#NotAuthorized').val()
        };
    }

    throw err;
};

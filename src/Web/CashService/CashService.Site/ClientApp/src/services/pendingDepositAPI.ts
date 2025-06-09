import { axiosInstance as api } from './axios';
import { ResultModel } from './interfaces/ResultModel';
import { AddDepositManualModel } from './interfaces/AddDepositManualModel';
import { ApproveRejectDepositModel } from './interfaces/ApproveRejectDepositModel';

export const pendingDepositAPI = {
  getLimitAmtDeposit: async (custId: number): Promise<number> => {
    try {
      const response = await api.get<number>(`GetLimitAmtDeposit?custId=${custId}`);
      return response.data;
    } catch (error) {
      console.error('Error fetching credit amount:', error);
      return 0;
    }
  },

  createDepositManual: async (manualDepositAdd: AddDepositManualModel): Promise<ResultModel> => {
    try {
      const formData = new FormData();
      formData.append('custId', manualDepositAdd.custId.toString());
      formData.append('paymentFrom', manualDepositAdd.paymentFrom);
      formData.append('paymentTo', manualDepositAdd.paymentTo);
      formData.append('amount', manualDepositAdd.amount.toString());
      formData.append('remark', manualDepositAdd.remark);

      if (manualDepositAdd.image) {
        formData.append('image', manualDepositAdd.image);
      }

      const response = await api.post<ResultModel>('AddManualDeposit', formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });
      return response.data;
    } catch (error) {
      console.error('Error creating manual deposit:', error);
      return { code: 1, isSuccessful: false, message: 'Error add manual deposit' };
    }
  },

  approveDeposit: async (approveRejectModel: ApproveRejectDepositModel): Promise<ResultModel> => {
    try {
      const response = await api.post<ResultModel>('ApproveDeposit', approveRejectModel);
      return response.data;
    } catch (error) {
      console.error('Error approving deposit:', error);
      return { code: 1, isSuccessful: false, message: 'Error approving deposit' };
    }
  },

  rejectDeposit: async (approveRejectModel: ApproveRejectDepositModel): Promise<ResultModel> => {
    try {
      const response = await api.post<ResultModel>('RejectDeposit', approveRejectModel);
      return response.data;
    } catch (error) {
      console.error('Error rejecting deposit:', error);
      return { code: 1, isSuccessful: false, message: 'Error rejecting deposit' };
    }
  },
};

export default pendingDepositAPI;

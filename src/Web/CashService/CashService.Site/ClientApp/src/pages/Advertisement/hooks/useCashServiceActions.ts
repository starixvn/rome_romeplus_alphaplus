import { useCallback } from 'react';
import { axiosInstance, createHeaderXMLHttp } from '../../../services/axios';

declare const appConfig: {
  nickName: string;
};

export const useCashServiceActions = () => {
  const onConfirmEnableCashService = useCallback(async () => {
    const response = await axiosInstance.post(
      `/main/Dashboard/UpdateCashServiceStatus?IsEnableCash=true`,
      {},
      { headers: createHeaderXMLHttp() }
    );

    if (response.data.isSuccessful) {
      window.top?.indexManager.showHideCashServiceChecklist(true);
      window.location.reload();
    } else {
      alert(response.data.message);
    }
  }, []);

  const checkDontShowAgain = useCallback(() => {
    if (appConfig.nickName) {
      localStorage.setItem(`${appConfig.nickName}-hideCashServiceAds`, 'true');
    }
  }, []);

  return { onConfirmEnableCashService, checkDontShowAgain };
};

import { useCallback } from 'react';
import { axiosInstance, createHeaderXMLHttp } from '../../../services/axios';
import { MESSAGE_TYPES } from '../constants/advertisement';

declare const appConfig: {
  nickName: string;
};

declare const homeUrl: string;

export const useCashServiceActions = () => {
  const onConfirmEnableCashService = useCallback(async () => {
    const response = await axiosInstance.post(
      `/main/Dashboard/UpdateCashServiceStatus?IsEnableCash=true`,
      {},
      { headers: createHeaderXMLHttp() }
    );

    if (response.data.isSuccessful) {
      window.top?.indexManager?.showHideCashServiceChecklist(true);
      const mainFrame = document.getElementById('main') as HTMLIFrameElement;
      mainFrame.src = homeUrl;
      window.postMessage({ type: MESSAGE_TYPES.CLOSE_ALL_CASH_ADS_POPUP }, '*');
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

import { useState, useRef, useEffect, useCallback } from 'react';
import { MESSAGE_TYPES } from '../constants/advertisement';
import { CashServiceAdsRef } from '../MainBanner';

export const useCashServiceAds = () => {
  const [isOpen, setIsOpen] = useState(true);
  const [shouldShowPopup, setShouldShowPopup] = useState(false);
  const cashServiceAdsRef = useRef<CashServiceAdsRef | null>(null);

  const handleMessage = useCallback((event: MessageEvent) => {
    const { type, step } = event.data;
    
    switch (type) {
      case MESSAGE_TYPES.OPEN_POPUP:
        setShouldShowPopup(true);
        setIsOpen(true);
        break;
      case MESSAGE_TYPES.FIND_OUT_MORE:
        cashServiceAdsRef.current?.findOutMore();
        break;
      case MESSAGE_TYPES.ACTIVATE_NOW:
        cashServiceAdsRef.current?.activateNow();
        break;
      case MESSAGE_TYPES.GO_TO_STEP:
        cashServiceAdsRef.current?.goToStep(step);
        break;
      case MESSAGE_TYPES.CLOSE:
        cashServiceAdsRef.current?.close();
        break;
    }
  }, []);

  useEffect(() => {
    window.addEventListener('message', handleMessage);
    return () => window.removeEventListener('message', handleMessage);
  }, [handleMessage]);

  return { 
    isOpen, 
    setIsOpen, 
    shouldShowPopup, 
    setShouldShowPopup,
    cashServiceAdsRef 
  };
};

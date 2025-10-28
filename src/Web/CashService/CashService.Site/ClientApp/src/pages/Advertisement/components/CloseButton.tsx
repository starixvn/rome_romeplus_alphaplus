import React from 'react';
import { AnalyzeCashServiceAds } from '../AnalzeCash';
import styles from '../cash-service-ads.module.scss';

interface CloseButtonProps {
  onClose?: () => void;
  nickName: string;
}

export const CloseButton: React.FC<CloseButtonProps> = ({ onClose, nickName }) => {
  const handleClose = () => {
    AnalyzeCashServiceAds({
      name: nickName || '',
      step: 0,
      act: 'close'
    });
    onClose?.();
  };

  if (!onClose) {
    return null;
  }

  return (
    <button onClick={handleClose} className={styles['close-button']}>
      ×
    </button>
  );
};
import { useState } from 'react';
import { CopyToClipboard } from 'react-copy-to-clipboard';
import { useTranslation } from 'react-i18next';

import { IconSuccess } from '@/features/Shared/Icons';
import WrapperTooltip from '@/features/Shared/WrapperTooltip';

import styles from './NewAccountPopup.module.scss';

export default function NewAccountSuccessPopup({ username, handleOk }: { username: string; handleOk: () => void }) {
  const { t } = useTranslation();
  const [copied, setCopied] = useState(false);

  const handleCopyUsername = () => {
    setCopied(true);
    setTimeout(() => {
      setCopied(false);
    }, 1500);
  };

  return (
    <div>
      <CopyToClipboard text={username}>
        <div>
          <div className={styles.icon}>{IconSuccess}</div>
          <div className={styles.success}>{t('success')}!</div>
          <div className={styles.text}>
            <span>{t('newAccount')}</span>
            <span
              className={styles.username}
              data-tooltip-id="copy-username-id"
              onClick={handleCopyUsername}
              aria-hidden
            >
              {' '}
              {username}{' '}
            </span>
            <span>{t('hasBeenCreatedSuccessfully')}</span>
          </div>
        </div>
      </CopyToClipboard>
      <button className={styles.buttonSuccess} type="button" onClick={handleOk}>
        {t('okay')}
      </button>
      <WrapperTooltip content={t('copied') ?? ''} id="copy-username-id" type="default" isOpen={copied} />
    </div>
  );
}

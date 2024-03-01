import { useState } from 'react';

export const useCopyBetSetting = () => {
  const [copyId, setCopyId] = useState('');

  const handleCopyBetSettings = (newCopyId: string) => {
    if (newCopyId === copyId) return;
    setCopyId(newCopyId);
    setTimeout(() => {
      setCopyId('');
    }, 3000);
  };

  return [copyId, handleCopyBetSettings] as const;
};

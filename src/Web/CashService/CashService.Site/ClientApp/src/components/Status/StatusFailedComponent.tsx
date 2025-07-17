import { useTranslation } from 'react-i18next';

import { StatusComponentProps } from './types';

export const StatusFailedComponent: React.FC<StatusComponentProps> = ({ text }) => {
  const { t } = useTranslation('global');
  return (
    <div role="alert">
      <span className=" inline-block font-semibold rounded-[4px] min-w-[90px] text-center px-[10px] py-[3px] bg-[#E0E0E0] dark:bg-[#6f6f6f]">
        {text || t('failed')}
      </span>
    </div>
  );
};

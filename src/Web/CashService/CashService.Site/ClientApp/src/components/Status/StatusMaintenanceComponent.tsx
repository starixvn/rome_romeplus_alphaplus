import { useTranslation } from 'react-i18next';

import { StatusComponentProps } from './types';

export const StatusMaintenanceComponent: React.FC<StatusComponentProps> = ({ text }) => {
  const { t } = useTranslation('global');
  return (
    <div role="alert">
      <span className=" inline-block font-semibold rounded-[4px] min-w-[90px] text-center px-[10px] py-[3px] bg-[#fcae7b] dark:bg-[#955e39]">
        {text || t('maintenance')}
      </span>
    </div>
  );
};

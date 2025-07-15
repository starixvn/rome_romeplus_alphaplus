import { useTranslation } from 'react-i18next';
import { StatusComponentProps } from './types';

export const StatusProcessingComponent: React.FC<StatusComponentProps> = ({ text }) => {
  const { t } = useTranslation('global');
  return (
    <div role="alert">
      <span className=" inline-block font-semibold rounded-[4px] min-w-[90px] text-center px-[10px] py-[3px] bg-[#90d3ff] dark:bg-[#2a668d]">
        {text ? text : t('processing')}
      </span>
    </div>
  );
}; 
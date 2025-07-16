import { useTranslation } from 'react-i18next';
import { StatusComponentProps } from './types';

export const StatusCompletedComponent: React.FC<StatusComponentProps> = ({ text }) => {
  const { t } = useTranslation('global');
  return (
    <div role="alert">
      <span className=" inline-block font-semibold rounded-[4px] min-w-[90px] text-center px-[10px] py-[3px] bg-[#D4E7D9] dark:bg-[#24853e]">
        {text ?? t('status.completed')}
      </span>
    </div>
  );
}; 
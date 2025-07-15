import { useTranslation } from 'react-i18next';

export const StatusCompletedComponent: React.FC<{ text?: string }> = ({ text }) => {
  const { t } = useTranslation('global');
  return (
    <div role="alert">
      <span className=" inline-block font-semibold rounded-[4px] min-w-[90px] text-center px-[10px] py-[3px] bg-[#D4E7D9] dark:bg-[#24853e]">
        {text ? text : t('status.completed')}
      </span>
    </div>
  );
};
export const StatusFailedComponent: React.FC<{ text?: string }> = ({ text }) => {
  const { t } = useTranslation('global');
  return (
    <div role="alert">
      <span className=" inline-block font-semibold rounded-[4px] min-w-[90px] text-center px-[10px] py-[3px] bg-[#E0E0E0] dark:bg-[#6f6f6f]">
        {text ? text : t('failed')}
      </span>
    </div>
  );
};
export const StatusDisabledComponent: React.FC<{ text?: string }> = ({ text }) => {
  const { t } = useTranslation('global');
  return (
    <div role="alert">
      <span className=" inline-block font-semibold rounded-[4px] min-w-[90px] text-center px-[10px] py-[3px] bg-[#FBD8D8] dark:bg-[#af4646]">
        {text ? text : t('disabled')}
      </span>
    </div>
  );
};
export const StatusProcessingComponent: React.FC<{ text?: string }> = ({ text }) => {
  const { t } = useTranslation('global');
  return (
    <div role="alert">
      <span className=" inline-block font-semibold rounded-[4px] min-w-[90px] text-center px-[10px] py-[3px] bg-[#90d3ff] dark:bg-[#2a668d]">
        {text ? text : t('processing')}
      </span>
    </div>
  );
};
export const StatusMaintenanceComponent: React.FC<{ text?: string }> = ({ text }) => {
  const { t } = useTranslation('global');
  return (
    <div role="alert">
      <span className=" inline-block font-semibold rounded-[4px] min-w-[90px] text-center px-[10px] py-[3px] bg-[#fcae7b] dark:bg-[#955e39]">
        {text ? text : t('maintenance')}
      </span>
    </div>
  );
};

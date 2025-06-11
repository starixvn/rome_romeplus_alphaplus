import { useTranslation } from 'react-i18next';

export const StatusEnabledComponent: React.FC = () => {
  const { t } = useTranslation('global');
  return (
    <div role="alert">
      <span className=" inline-block font-semibold rounded-[20px] min-w-[80px] text-center px-[10px] py-[3px] bg-[#D4E7D9] dark:bg-[#24853e]">
        {t('global:enabled')}
      </span>
    </div>
  );
};
export const StatusCompletedComponent: React.FC<{ text?: string }> = ({ text }) => {
  const { t } = useTranslation('global');
  return (
    <div role="alert">
      <span className=" inline-block font-semibold rounded-[20px] min-w-[80px] text-center px-[10px] py-[3px] bg-[#D4E7D9] dark:bg-[#24853e]">
        {text ? t(text) : t('global:status.completed')}
      </span>
    </div>
  );
};
export const StatusNoneComponent: React.FC = () => {
  return (
    <div role="alert">
      <span className=" inline-block font-semibold rounded-[20px] min-w-[80px] text-center px-[10px] py-[3px] bg-[#E0E0E0] dark:bg-[#6f6f6f]">
        -
      </span>
    </div>
  );
};
export const StatusFailedComponent: React.FC<{ text?: string }> = ({ text }) => {
  const { t } = useTranslation('global');
  return (
    <div role="alert">
      <span className=" inline-block font-semibold rounded-[20px] min-w-[80px] text-center px-[10px] py-[3px] bg-[#E0E0E0] dark:bg-[#6f6f6f]">
        {text ? t(text) : t('global:failed')}
      </span>
    </div>
  );
};
export const StatusDisabledComponent: React.FC<{ text?: string }> = ({ text }) => {
  const { t } = useTranslation('global');
  return (
    <div role="alert">
      <span className=" inline-block font-semibold rounded-[20px] min-w-[80px] text-center px-[10px] py-[3px] bg-[#FBD8D8] dark:bg-[#af4646]">
        {text ? t(text) : t('global:disable')}
      </span>
    </div>
  );
};
export const StatusProcessingComponent: React.FC<{ text?: string }> = ({ text }) => {
  const { t } = useTranslation('global');
  return (
    <div role="alert">
      <span className=" inline-block font-semibold rounded-[20px] min-w-[80px] text-center px-[10px] py-[3px] bg-[#90d3ff] dark:bg-[#2a668d]">
        {text ? t(text) : t('global:processing')}
      </span>
    </div>
  );
};
export const StatusMaintenanceComponent: React.FC<{ text?: string }> = ({ text }) => {
  const { t } = useTranslation('global');
  return (
    <div role="alert">
      <span className=" inline-block font-semibold rounded-[20px] min-w-[80px] text-center px-[10px] py-[3px] bg-[#fcae7b] dark:bg-[#955e39]">
        {text ? t(text) : t('global:maintenance')}
      </span>
    </div>
  );
};

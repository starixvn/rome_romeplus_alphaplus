import { useTranslation } from 'react-i18next';

export const StatusEnabledComponent: React.FC = () => {
    const { t } = useTranslation('global');
    return (
    <div role="alert">
      <span className=" inline-block font-semibold rounded-[20px] min-w-[80px] text-center px-[10px] py-[3px] bg-[#D4E7D9]">
        {t('global:enabled')}
      </span>
    </div>
  );
};
export const StatusCompletedComponent: React.FC = () => {
    const { t } = useTranslation('global');
    return (
    <div role="alert">
      <span className=" inline-block font-semibold rounded-[20px] min-w-[80px] text-center px-[10px] py-[3px] bg-[#D4E7D9]">
        {t('global:completed')}
      </span>
    </div>
  );
};
export const StatusNoneComponent: React.FC = () => {
  return (
    <div role="alert">
      <span className=" inline-block font-semibold rounded-[20px] min-w-[80px] text-center px-[10px] py-[3px] bg-[#E0E0E0]">
        -
      </span>
    </div>
  );
};
export const StatusFailedComponent: React.FC = () => {
    const { t } = useTranslation('global');
    return (
      <div role="alert">
        <span className=" inline-block font-semibold rounded-[20px] min-w-[80px] text-center px-[10px] py-[3px] bg-[#E0E0E0]">
        {t('global:failed')}
        </span>
      </div>
    );
  };
export const StatusDisabledComponent: React.FC = () => {
    const { t } = useTranslation('global');
  return (
    <div role="alert">
      <span className=" inline-block font-semibold rounded-[20px] min-w-[80px] text-center px-[10px] py-[3px] bg-[#FBD8D8]">
        {t('global:disable')}
      </span>
    </div>
  );
};
export const StatusProcessingComponent: React.FC = () => {
    const { t } = useTranslation('global');
    return (
      <div role="alert">
        <span className=" inline-block font-semibold rounded-[20px] min-w-[80px] text-center px-[10px] py-[3px] bg-[#90d3ff]">
          {t('global:processing')}
        </span>
      </div>
    );
  };
  export const StatusMaintenanceComponent: React.FC = () => {
    const { t } = useTranslation('global');
    return (
      <div role="alert">
        <span className=" inline-block font-semibold rounded-[20px] min-w-[80px] text-center px-[10px] py-[3px] bg-[#fcae7b]">
          {t('global:maintenance')}
        </span>
      </div>
    );
  };
  
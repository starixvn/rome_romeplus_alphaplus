import React from 'react';
import { Setting } from '../../../types/memberSite';
import { useTranslation } from 'react-i18next';

interface GeneralSettingsProps {
  settings: Setting[];
  onSettingsChange: (settingId: string, value: boolean) => void;
  loading: boolean;
}

const GeneralSettingsSkeleton = () => {
  return (
    <div>
      {/* Title skeleton */}
      <div className="flex items-center justify-between">
        <div className="h-[22px] w-[150px] bg-gray-200 rounded mb-2 animate-pulse"></div>
      </div>
      {/* Switch skeletons */}
      <div className="flex">
        <button className="text-left w-[25%] min-w-[200px] max-[768px]:w-[48%]">
          <div className="flex-1 min-w-0">
            <div className="h-[12px] w-[90px] bg-gray-100 rounded my-[8px] animate-pulse"></div>
            <div className="h-[12px] w-[120px] bg-gray-100 rounded mb-[8px] animate-pulse"></div>
          </div>
          <span className="block w-[60px] h-[30px] rounded-[20px] bg-gray-200 relative animate-pulse"></span>
        </button>
        <button className="text-left w-[25%] min-w-[200px] max-[768px]:w-[48%]">
          <div className="flex-1 min-w-0">
            <div className="h-[12px] w-[70px] bg-gray-100 rounded my-[8px] animate-pulse"></div>
            <div className="h-[12px] w-[100px] bg-gray-100 rounded mb-[8px] animate-pulse"></div>
          </div>
          <span className="block w-[60px] h-[30px] rounded-[20px] bg-gray-200 relative animate-pulse"></span>
        </button>
      </div>
    </div>
  );
};

const GeneralSettings: React.FC<GeneralSettingsProps> = ({ 
  settings,
  onSettingsChange,
  loading
}) => {
  const { t } = useTranslation('memberSite');

  // Group settings by category
  const groupedSettings = settings.reduce((acc, setting) => {
    const category = setting.category || 'General';
    if (!acc[category]) {
      acc[category] = [];
    }
    acc[category].push(setting);
    return acc;
  }, {} as Record<string, Setting[]>);

  if (loading) {
    return <GeneralSettingsSkeleton />
  }

  return (
    <div>
      <div className="flex items-center justify-between">
        <h2 className="text-[16px] text-[var(--text-color-primary)] font-semibold leading-[21.6px] tracking-[0.2px] max-[768px]:text-[20px] max-[768px]:leading-[26px]">{t('generalSettings.title')}</h2>
      </div>
    
      {Object.entries(groupedSettings).map(([category, categorySettings]) => (
        <div key={category}>
          <div className="flex ">
            {categorySettings.map((setting) => (
              <button 
                key={setting.id} 
                className="text-left w-[25%] min-w-[200px] max-[768px]:w-[48%]"
                onClick={(e) =>{
                  e.preventDefault();
                  onSettingsChange(setting.id, !setting.value);
                }}
                aria-pressed={setting.value}
              >
                <div className="flex-1 min-w-0">
                  <p className="text-[12px] font-semibold my-[8px] leading-[14px] max-[768px]:text-[16px] max-[768px]:leading-[16px]">
                    {t(`generalSettings.${setting.id}.title`)}
                  </p>
                  {setting.description && (
                    <p className="text-[12px] leading-[12px] mb-[8px] max-[768px]:text-[16px] max-[768px]:leading-[16px]">
                      {t(`generalSettings.${setting.id}.description`)}
                    </p>
                  )}
                </div>  
                <label>                  
                  <input
                    type="checkbox"
                    checked={setting.value}   
                    className="opacity-0 absolute hidden peer"
                    onChange={(e) => {
                      e.preventDefault();
                      onSettingsChange(setting.id, !setting.value);
                    }}
                  />
                  <span className="block cursor-pointer w-[60px] h-[30px] rounded-[20px] bg-[var(--button-switch-background)] relative after:content-[&quot;&quot;] after:absolute after:top-[3px] after:left-[3px] after:w-[24px] after:h-[24px] after:rounded-[50%] after:bg-[var(--button-switch-dot-background)] after:transition-all after:duration-300 after:ease-in-out after:shadow-[0_2px_4px_0_rgba(0,0,0,0.25)] peer-checked:after:left-[32px] peer-checked:bg-[var(--button-switch-background-active)]"></span>
                  </label>
              </button>
            ))}
          </div>
        </div>
      ))}
    </div>
  );
};

export default GeneralSettings; 
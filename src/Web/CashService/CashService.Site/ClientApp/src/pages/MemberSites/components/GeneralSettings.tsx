import React from 'react';
import { Setting } from '../../../services/interfaces/MemberSites';
import { useTranslation } from 'react-i18next';
import { useFormContext } from 'react-hook-form';
import { MediumTitleComponent } from '../../../components/TitleComponents';

interface GeneralSettingsProps {loading: boolean;}

const GeneralSettingsSkeleton = () => {
  return (
    <div>
      {/* Title skeleton */}
      <div className="flex items-center justify-between">
        <div className="h-[22px] w-[150px] bg-gray-200 dark:bg-gray-700 rounded mb-2 animate-pulse"></div>
      </div>
      {/* Switch skeletons */}
      <div className="flex">
        <button className="text-left w-[25%] min-w-[200px] max-[768px]:w-[48%]">
          <div className="flex-1 min-w-0">
            <div className="h-[12px] w-[90px] bg-gray-100 dark:bg-gray-700 rounded my-[8px] animate-pulse"></div>
            <div className="h-[12px] w-[120px] bg-gray-100 dark:bg-gray-700 rounded mb-[8px] animate-pulse"></div>
          </div>
          <span className="block w-[60px] h-[30px] rounded-[20px] bg-gray-200 dark:bg-gray-700 relative animate-pulse"></span>
        </button>
        <button className="text-left w-[25%] min-w-[200px] max-[768px]:w-[48%]">
          <div className="flex-1 min-w-0">
            <div className="h-[12px] w-[70px] bg-gray-100 dark:bg-gray-700 rounded my-[8px] animate-pulse"></div>
            <div className="h-[12px] w-[100px] bg-gray-100 dark:bg-gray-700 rounded mb-[8px] animate-pulse"></div>
          </div>
          <span className="block w-[60px] h-[30px] rounded-[20px] bg-gray-200 dark:bg-gray-700 relative animate-pulse"></span>
        </button>
      </div>
    </div>
  );
};

const GeneralSettings: React.FC<GeneralSettingsProps> = ({loading}) => {
  const { t } = useTranslation('memberSite');
  const { setValue, watch } = useFormContext();

  // Group settings by category
  const groupedSettings = watch('generalSettings').reduce((acc: Record<string, Setting[]>, setting: Setting) => {
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
        <MediumTitleComponent text={t('generalSettings.title')} />
      </div>

      {Object.entries(groupedSettings).map(([category, categorySettings]) => (
        <div key={category}>
          <div className="flex ">
            {(categorySettings as Setting[]).map((setting: Setting, index: number) => {
              const currentValue = setting.value;
              return (
                <button
                  key={setting.id}
                  className="text-left w-[25%] min-w-[200px] max-[768px]:w-[48%]"
                  onClick={(e) => {
                    e.preventDefault();
                    setValue(`generalSettings.${index}.value`, !currentValue, { shouldDirty: true });
                  }}
                  aria-pressed={currentValue}
                >
                  <div className="flex-1 min-w-0">
                    <p className="font-semibold my-[8px]">
                      {t(`generalSettings.${setting.id}.title`)}
                    </p>
                    {setting.description && (
                      <p className="mb-[8px]">
                        {t(`generalSettings.${setting.id}.description`)}
                      </p>
                    )}
                  </div>
                  <label className='inline-block'>
                    <input
                      type="checkbox"
                      checked={currentValue}
                      className="opacity-0 absolute hidden peer"
                      onChange={(e) => {
                        e.preventDefault();
                        setValue(`generalSettings.${index}.value`, !currentValue, { shouldDirty: true });
                      }}
                    />
                    <span className="block cursor-pointer w-[60px] h-[30px] rounded-[20px] bg-[var(--button-switch-background)] relative after:content-[&quot;&quot;] after:absolute after:top-[3px] after:left-[3px] after:w-[24px] after:h-[24px] after:rounded-[50%] after:bg-[var(--button-switch-dot-background)] after:transition-all after:duration-300 after:ease-in-out after:shadow-[0_2px_4px_0_rgba(0,0,0,0.25)] peer-checked:after:left-[32px] peer-checked:bg-[var(--button-switch-background-active)]"></span>
                  </label>
                </button>
              );
            })}
          </div>
        </div>
      ))}
    </div>
  );
};

export default GeneralSettings; 
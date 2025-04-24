import React from 'react';
import { Setting } from '../../../types/memberSite';
import { useTranslation } from 'react-i18next';

interface GeneralSettingsProps {
  settings: Setting[];
  onSettingsChange: (settingId: string, value: boolean) => void;
}

const GeneralSettings: React.FC<GeneralSettingsProps> = ({ 
  settings,
  onSettingsChange 
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

  return (
    <div className="space-y-4">
      <div className="flex items-center justify-between">
        <h2 className="text-lg text-sky-700 font-semibold">{t('generalSettings.title')}</h2>
      </div>
    
      {Object.entries(groupedSettings).map(([category, categorySettings]) => (
        <div key={category} className="space-y-3">
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-3">
            {categorySettings.map((setting) => (
              <button 
                key={setting.id} 
                className="w-full text-left flex items-center justify-between p-4 bg-gray-50 rounded-lg cursor-pointer hover:bg-gray-100 transition-colors"
                onClick={(e) =>{
                  e.preventDefault();
                  onSettingsChange(setting.id, !setting.value);
                }}
                aria-pressed={setting.value}
              >
                <div className="flex-1 min-w-0">
                  <p className="text-sm font-medium text-gray-900">
                    {t(`generalSettings.${setting.id}.title`)}
                  </p>
                  {setting.description && (
                    <p className="mt-1 text-xs text-gray-500 line-clamp-2">
                      {t(`generalSettings.${setting.id}.description`)}
                    </p>
                  )}
                </div>
                <label className="relative inline-flex items-center flex-shrink-0">
                  <input
                    type="checkbox"
                    checked={setting.value}   
                    className="sr-only peer"
                    onChange={(e) => {
                      e.preventDefault();
                      onSettingsChange(setting.id, !setting.value);
                    }}
                  />
                  <div className="w-11 h-6 bg-gray-200 peer-focus:outline-none rounded-full peer peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:left-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all peer-checked:bg-[#1e4d8c]"></div>
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